using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SecureFileShredder.Models;

namespace SecureFileShredder.Controllers;

public sealed class ShredderController
{
    public void ShredFile(
        string filePath,
        ShredJobOptions options,
        CancellationToken cancellationToken,
        Action<int, int, long>? onChunk)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }

        ClearReadOnly(filePath);
        var patterns = options.Patterns.Count > 0
            ? options.Patterns
            : new[] { PassPattern.Random() };
        using var rng = RandomNumberGenerator.Create();
        if (options.WipeAlternateStreams)
        {
            WipeAlternateStreams(filePath, patterns, Math.Max(1, options.BufferBytes), rng, cancellationToken);
        }

        long length = new FileInfo(filePath).Length;
        if (length == 0)
        {
            onChunk?.Invoke(patterns.Count, patterns.Count, 0);
            return;
        }

        Overwrite(filePath, length, patterns, Math.Max(1, options.BufferBytes), options.VerifyOverwrite, rng, cancellationToken, onChunk);
    }

    private static void Overwrite(
        string path,
        long length,
        IReadOnlyList<PassPattern> patterns,
        int bufferSize,
        bool verify,
        RandomNumberGenerator rng,
        CancellationToken cancellationToken,
        Action<int, int, long>? onChunk)
    {
        var buffer = new byte[bufferSize];
        var samples = new List<(long Offset, byte[] Data)>();
        for (int pass = 0; pass < patterns.Count; pass++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int passNumber = pass + 1;
            onChunk?.Invoke(passNumber, patterns.Count, 0);
            bool last = pass == patterns.Count - 1;
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None);
            long position = 0;
            while (position < length)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int count = (int)Math.Min(buffer.Length, length - position);
                FillPattern(buffer, count, patterns[pass], position, rng);
                stream.Write(buffer, 0, count);
                if (verify && last && samples.Count < 4)
                {
                    int take = Math.Min(16, count);
                    var copy = new byte[take];
                    Buffer.BlockCopy(buffer, 0, copy, 0, take);
                    samples.Add((position, copy));
                }

                position += count;
                onChunk?.Invoke(passNumber, patterns.Count, count);
            }

            stream.Flush(true);
        }

        if (!verify || length == 0)
        {
            return;
        }

        if (samples.Count == 0)
        {
            throw new IOException("Verification failed: no samples were written.");
        }

        using var read = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        foreach ((long offset, byte[] expected) in samples)
        {
            read.Position = offset;
            var actual = new byte[expected.Length];
            int readCount = read.Read(actual, 0, actual.Length);
            if (readCount != expected.Length || !actual.AsSpan().SequenceEqual(expected))
            {
                throw new IOException("Verification failed: overwritten data did not match.");
            }
        }
    }

    internal static void FillPattern(byte[] data, int count, PassPattern pattern, long filePosition, RandomNumberGenerator rng)
    {
        if (pattern.Kind == PassKind.Random)
        {
            rng.GetBytes(data.AsSpan(0, count));
            return;
        }

        if (pattern.Cycle is { Length: > 0 } cycle)
        {
            for (int i = 0; i < count; i++)
            {
                data[i] = cycle[(int)((filePosition + i) % cycle.Length)];
            }

            return;
        }

        Array.Fill(data, pattern.Value, 0, count);
    }

    private static void ClearReadOnly(string filePath)
    {
        FileAttributes attributes = File.GetAttributes(filePath);
        if (attributes.HasFlag(FileAttributes.ReadOnly))
        {
            File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
        }
    }

    private static void WipeAlternateStreams(
        string filePath,
        IReadOnlyList<PassPattern> patterns,
        int bufferSize,
        RandomNumberGenerator rng,
        CancellationToken cancellationToken)
    {
        if (!IsNtfs(filePath))
        {
            return;
        }

        foreach ((string streamPath, long streamLength) in ListAlternateStreams(filePath))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (streamLength <= 0)
            {
                continue;
            }

            Overwrite(streamPath, streamLength, patterns, bufferSize, false, rng, cancellationToken, null);
        }
    }

    private static bool IsNtfs(string filePath)
    {
        try
        {
            string? root = Path.GetPathRoot(filePath);
            if (string.IsNullOrEmpty(root))
            {
                return false;
            }

            return new DriveInfo(root).DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static List<(string Path, long Length)> ListAlternateStreams(string filePath)
    {
        var streams = new List<(string, long)>();
        IntPtr handle = FindFirstStreamW(filePath, 0, out WIN32_FIND_STREAM_DATA data, 0);
        if (handle == InvalidHandle)
        {
            return streams;
        }

        try
        {
            do
            {
                string? streamPath = ToStreamPath(filePath, data.cStreamName);
                if (streamPath != null)
                {
                    streams.Add((streamPath, data.StreamSize));
                }
            }
            while (FindNextStreamW(handle, out data));
        }
        finally
        {
            FindClose(handle);
        }

        return streams;
    }

    private static string? ToStreamPath(string filePath, string? streamName)
    {
        if (string.IsNullOrEmpty(streamName))
        {
            return null;
        }

        const string suffix = ":$DATA";
        string name = streamName;
        if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            name = name[..^suffix.Length];
        }

        if (name.Length <= 1)
        {
            return null;
        }

        return filePath + name;
    }

    private static readonly IntPtr InvalidHandle = new(-1);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WIN32_FIND_STREAM_DATA
    {
        public long StreamSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 296)]
        public string cStreamName;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindFirstStreamW(string lpFileName, int infoLevel, out WIN32_FIND_STREAM_DATA data, int flags);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool FindNextStreamW(IntPtr handle, out WIN32_FIND_STREAM_DATA data);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FindClose(IntPtr handle);
}
