namespace SecureFileShredder.Models;

public enum PassKind
{
    Random,
    Byte,
    Cycle
}

public enum ShredNoticeKind
{
    Started,
    Progress,
    Completed,
    Failed
}

public sealed class PassPattern
{
    private PassPattern(PassKind kind, byte value, byte[]? cycle)
    {
        Kind = kind;
        Value = value;
        Cycle = cycle;
    }

    public PassKind Kind { get; }
    public byte Value { get; }
    public byte[]? Cycle { get; }

    public static PassPattern Random() => new(PassKind.Random, 0, null);

    public static PassPattern Fixed(byte value) => new(PassKind.Byte, value, null);

    public static PassPattern Repeating(params byte[] cycle) => new(PassKind.Cycle, 0, cycle);
}

public sealed class ShredPreset
{
    public required string Label { get; init; }
    public required string Tooltip { get; init; }
    public required IReadOnlyList<PassPattern> Patterns { get; init; }
    public int Passes => Patterns.Count;
    public override string ToString() => Label;
}

public sealed class BufferOption
{
    public required string Label { get; init; }
    public required int Bytes { get; init; }
    public override string ToString() => Label;
}

public sealed class ShredJobOptions
{
    public required IReadOnlyList<PassPattern> Patterns { get; init; }
    public required int BufferBytes { get; init; }
    public bool VerifyOverwrite { get; init; }
    public bool WipeAlternateStreams { get; init; }
    public bool RenameBeforeDelete { get; init; } = true;
    public bool RandomizeTimestamps { get; init; } = true;
    public int MaxDegreeOfParallelism { get; init; } = 1;
}

public sealed class ShredProgressUpdate
{
    public ShredNoticeKind Kind { get; init; }
    public string FilePath { get; init; } = "";
    public int PassNumber { get; init; }
    public int PassCount { get; init; }
    public long BytesCompleted { get; init; }
    public long BytesTotal { get; init; }
    public string? Detail { get; init; }
}

public sealed class FileOutcome
{
    public required string Path { get; init; }
    public required bool Success { get; init; }
    public long Bytes { get; init; }
    public string? Detail { get; init; }
}

public sealed class ShredBatchResult
{
    public List<FileOutcome> Succeeded { get; } = new();
    public List<FileOutcome> Failed { get; } = new();
    public TimeSpan Elapsed { get; set; }
    public long BytesOverwritten { get; set; }
    public bool Cancelled { get; set; }
}

public sealed class ShredProfile
{
    public string Name { get; set; } = "";
    public int PassIndex { get; set; }
    public int BufferIndex { get; set; } = 2;
    public bool RenameBeforeDelete { get; set; } = true;
    public bool RandomizeTimestamps { get; set; } = true;
    public bool VerifyOverwrite { get; set; }
    public bool WipeAlternateStreams { get; set; } = true;
    public bool BuiltIn { get; set; }
}

public sealed class AppSettings
{
    public int PassIndex { get; set; }
    public int BufferIndex { get; set; } = 2;
    public int WindowX { get; set; } = int.MinValue;
    public int WindowY { get; set; } = int.MinValue;
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }
    public bool DarkTheme { get; set; }
    public bool RenameBeforeDelete { get; set; } = true;
    public bool RandomizeTimestamps { get; set; } = true;
    public bool VerifyOverwrite { get; set; }
    public bool WipeAlternateStreams { get; set; } = true;
    public string SelectedProfile { get; set; } = "Quick";
    public List<ShredProfile> Profiles { get; set; } = new();
}

public sealed class ShredHistoryEntry
{
    public DateTime TimeUtc { get; set; }
    public string Path { get; set; } = "";
    public string Result { get; set; } = "";
    public int Passes { get; set; }
    public long Bytes { get; set; }
    public string? Detail { get; set; }
}

public sealed class QueueEntry
{
    public required string FullPath { get; init; }
    public required bool IsDirectory { get; init; }
    public override string ToString() => FullPath;
}

public readonly struct QueueAddResult
{
    public List<QueueEntry> Added { get; init; }
    public int Unreadable { get; init; }
}

public sealed class FreeSpaceProgress
{
    public string Phase { get; init; } = "";
    public long BytesCompleted { get; init; }
    public long BytesTotal { get; init; }
    public string? Detail { get; init; }
}

public sealed class FreeSpaceWipeResult
{
    public bool Cancelled { get; init; }
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public long BytesWritten { get; init; }
}
