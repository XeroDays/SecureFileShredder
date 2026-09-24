using SecureFileShredder.Controllers;
using SecureFileShredder.Models;

namespace SecureFileShredder.Services;

public static class FreeSpaceWiper
{
    public const long ReservedMarginBytes = 256L * 1024 * 1024;

    public static long PlannedBytes(long availableFree, long margin)
    {
        if (availableFree <= margin)
        {
            return 0;
        }

        return availableFree - margin;
    }

    public static async Task<FreeSpaceWipeResult> WipeAsync(
        DriveInfo drive,
        ShredJobOptions options,
        IProgress<FreeSpaceProgress>? progress,
        CancellationToken cancellationToken)
    {
        return await Task.Run(() => Wipe(drive, options, progress, cancellationToken));
    }

    private static FreeSpaceWipeResult Wipe(
        DriveInfo drive,
        ShredJobOptions options,
        IProgress<FreeSpaceProgress>? progress,
        CancellationToken cancellationToken)
    {
        string tempPath = Path.Combine(drive.RootDirectory.FullName, "sfs-freespace-" + Guid.NewGuid().ToString("N") + ".bin");
        long written = 0;
        try
        {
            long planned = PlannedBytes(new DriveInfo(drive.Name).AvailableFreeSpace, ReservedMarginBytes);
            if (planned <= 0)
            {
                return new FreeSpaceWipeResult
                {
                    Success = false,
                    Message = "Not enough free space. Wipe keeps 256 MB free and this drive is already below that."
                };
            }

            progress?.Report(new FreeSpaceProgress { Phase = "Writing", BytesTotal = planned });
            var buffer = new byte[1024 * 1024];
            using (var stream = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    long room = PlannedBytes(new DriveInfo(drive.Name).AvailableFreeSpace, ReservedMarginBytes);
                    if (room <= 0)
                    {
                        break;
                    }

                    int count = (int)Math.Min(buffer.Length, room);
                    stream.Write(buffer, 0, count);
                    written += count;
                    if (written % (8 * 1024 * 1024) < buffer.Length)
                    {
                        progress?.Report(new FreeSpaceProgress
                        {
                            Phase = "Writing",
                            BytesCompleted = written,
                            BytesTotal = planned
                        });
                    }
                }

                stream.Flush(true);
            }

            if (written <= 0 || !File.Exists(tempPath))
            {
                return new FreeSpaceWipeResult
                {
                    Success = false,
                    Message = "No free space was written."
                };
            }

            progress?.Report(new FreeSpaceProgress { Phase = "Shredding", BytesCompleted = 0, BytesTotal = written, Detail = tempPath });
            long shredded = 0;
            long shredTotal = written * Math.Max(1, options.Patterns.Count);
            var controller = new ShredderController();
            controller.ShredFile(tempPath, options, cancellationToken, (_, _, bytes) =>
            {
                if (bytes <= 0)
                {
                    return;
                }

                shredded += bytes;
                progress?.Report(new FreeSpaceProgress
                {
                    Phase = "Shredding",
                    BytesCompleted = shredded,
                    BytesTotal = shredTotal
                });
            });
            FileFinalizer.FinalizeFile(tempPath, options.RenameBeforeDelete, options.RandomizeTimestamps);
            return new FreeSpaceWipeResult
            {
                Success = true,
                BytesWritten = written,
                Message = "Free space wipe finished. " + ByteFormat.Format(written) + " was written, overwritten, and removed."
            };
        }
        catch (OperationCanceledException)
        {
            return new FreeSpaceWipeResult
            {
                Cancelled = true,
                BytesWritten = written,
                Message = "Free space wipe was cancelled. The temporary file was removed. Space already written may not have been fully overwritten."
            };
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            string message = written > 0
                ? "Wipe stopped early (" + ex.Message + "). Written data will be overwritten if the temporary file is still present."
                : ex.Message;
            if (written > 0 && File.Exists(tempPath))
            {
                try
                {
                    var controller = new ShredderController();
                    controller.ShredFile(tempPath, options, CancellationToken.None, null);
                    FileFinalizer.FinalizeFile(tempPath, false, false);
                    message = "Wipe stopped early (" + ex.Message + "). " + ByteFormat.Format(written) + " already written was overwritten and removed.";
                }
                catch (Exception shredEx)
                {
                    message = "Wipe stopped early (" + ex.Message + "). Cleanup failed: " + shredEx.Message;
                }
            }

            return new FreeSpaceWipeResult { Success = false, BytesWritten = written, Message = message };
        }
        finally
        {
            try
            {
                if (File.Exists(tempPath))
                {
                    File.SetAttributes(tempPath, FileAttributes.Normal);
                    File.Delete(tempPath);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
