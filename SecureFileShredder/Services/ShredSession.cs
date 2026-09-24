using SecureFileShredder.Models;

namespace SecureFileShredder.Services;

public sealed class ShredSession
{
    public async Task<ShredBatchResult> RunAsync(
        IReadOnlyList<string> files,
        ShredJobOptions options,
        Action<ShredProgressUpdate> publish,
        CancellationToken cancellationToken)
    {
        var result = new ShredBatchResult();
        long total = 0;
        foreach (string file in files)
        {
            try
            {
                if (File.Exists(file))
                {
                    total += new FileInfo(file).Length;
                }
            }
            catch (Exception)
            {
            }
        }

        int passes = Math.Max(1, options.Patterns.Count);
        total *= passes;
        long completed = 0;
        long lastReportTick = 0;
        var gate = new object();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var parallel = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Max(1, options.MaxDegreeOfParallelism),
            CancellationToken = cancellationToken
        };

        try
        {
            await Parallel.ForEachAsync(files, parallel, (file, token) =>
            {
                try
                {
                    long length = 0;
                    try
                    {
                        if (File.Exists(file))
                        {
                            length = new FileInfo(file).Length;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    publish(new ShredProgressUpdate
                    {
                        Kind = ShredNoticeKind.Started,
                        FilePath = file,
                        PassNumber = 1,
                        PassCount = passes,
                        BytesCompleted = Volatile.Read(ref completed),
                        BytesTotal = total
                    });

                    var controller = new Controllers.ShredderController();
                    controller.ShredFile(file, options, token, (passNumber, passCount, bytes) =>
                    {
                        long done = bytes > 0
                            ? Interlocked.Add(ref completed, bytes)
                            : Volatile.Read(ref completed);
                        long now = Environment.TickCount64;
                        bool passBoundary = bytes == 0;
                        long previous = Volatile.Read(ref lastReportTick);
                        if (!passBoundary && now - previous < 80)
                        {
                            return;
                        }

                        if (!passBoundary && Interlocked.CompareExchange(ref lastReportTick, now, previous) != previous)
                        {
                            return;
                        }

                        publish(new ShredProgressUpdate
                        {
                            Kind = ShredNoticeKind.Progress,
                            FilePath = file,
                            PassNumber = passNumber,
                            PassCount = passCount,
                            BytesCompleted = done,
                            BytesTotal = total
                        });
                    });

                    string? note = File.Exists(file)
                        ? FileFinalizer.FinalizeFile(file, options.RenameBeforeDelete, options.RandomizeTimestamps)
                        : null;
                    var outcome = new FileOutcome
                    {
                        Path = file,
                        Success = true,
                        Bytes = length,
                        Detail = note
                    };
                    lock (gate)
                    {
                        result.Succeeded.Add(outcome);
                        result.BytesOverwritten += length * passes;
                    }

                    publish(new ShredProgressUpdate
                    {
                        Kind = ShredNoticeKind.Completed,
                        FilePath = file,
                        PassCount = passes,
                        BytesCompleted = Volatile.Read(ref completed),
                        BytesTotal = total,
                        Detail = note
                    });
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    long length = 0;
                    try
                    {
                        if (File.Exists(file))
                        {
                            length = new FileInfo(file).Length;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    lock (gate)
                    {
                        result.Failed.Add(new FileOutcome
                        {
                            Path = file,
                            Success = false,
                            Bytes = length,
                            Detail = ex.Message
                        });
                    }

                    publish(new ShredProgressUpdate
                    {
                        Kind = ShredNoticeKind.Failed,
                        FilePath = file,
                        PassCount = passes,
                        BytesCompleted = Volatile.Read(ref completed),
                        BytesTotal = total,
                        Detail = ex.Message
                    });
                }

                return ValueTask.CompletedTask;
            });
        }
        catch (OperationCanceledException)
        {
            result.Cancelled = true;
        }
        catch (AggregateException ex) when (ex.InnerExceptions.All(item => item is OperationCanceledException) || cancellationToken.IsCancellationRequested)
        {
            result.Cancelled = true;
        }

        watch.Stop();
        result.Elapsed = watch.Elapsed;
        return result;
    }
}
