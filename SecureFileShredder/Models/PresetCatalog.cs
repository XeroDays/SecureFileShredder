namespace SecureFileShredder.Models;

public static class PresetCatalog
{
    private const string SsdNote = " On SSDs, wear leveling can leave old data on retired flash cells.";

    public static IReadOnlyList<ShredPreset> PassPresets { get; } = new ShredPreset[]
    {
        new()
        {
            Label = "Normal ( 1 Pass )",
            Tooltip = "1 pass of cryptographic random data." + SsdNote,
            Patterns = new[] { PassPattern.Random() }
        },
        new()
        {
            Label = "DoD ( 3 Passes )",
            Tooltip = "3 passes: 0x00, 0xFF, then cryptographic random." + SsdNote,
            Patterns = new[] { PassPattern.Fixed(0x00), PassPattern.Fixed(0xFF), PassPattern.Random() }
        },
        new()
        {
            Label = "DoD 5220.M ( 7 Passes )",
            Tooltip = "7 passes: 0x00, 0xFF, random, 0x00, 0xFF, random, random." + SsdNote,
            Patterns = new[]
            {
                PassPattern.Fixed(0x00), PassPattern.Fixed(0xFF), PassPattern.Random(),
                PassPattern.Fixed(0x00), PassPattern.Fixed(0xFF), PassPattern.Random(), PassPattern.Random()
            }
        },
        new()
        {
            Label = "NSA ( 12 Passes )",
            Tooltip = "12 passes repeating 0x00, 0xFF, and random, ending in random. The name matches earlier versions of this app; it is not a published NSA procedure." + SsdNote,
            Patterns = Repeat(12, PassPattern.Fixed(0x00), PassPattern.Fixed(0xFF), PassPattern.Random())
        },
        new()
        {
            Label = "GUTTMAN ( 35 Passes )",
            Tooltip = "Gutmann 35-pass sequence: 4 random, 27 fixed patterns, 4 random. Those fixed patterns target old magnetic disks, not modern SSDs." + SsdNote,
            Patterns = Gutmann()
        },
        new()
        {
            Label = "SFIK V1 ( 55 Passes )",
            Tooltip = "55 passes repeating 0x00, 0xFF, 0x55, 0xAA, and random. The sequence ends with random data." + SsdNote,
            Patterns = Repeat(55, PassPattern.Fixed(0x00), PassPattern.Fixed(0xFF), PassPattern.Fixed(0x55), PassPattern.Fixed(0xAA), PassPattern.Random())
        }
    };

    public static IReadOnlyList<BufferOption> Buffers { get; } = new BufferOption[]
    {
        new() { Label = "1024 bytes ( 1 KB )", Bytes = 1024 },
        new() { Label = "2048 bytes ( 2 KB )", Bytes = 2048 },
        new() { Label = "Default ( 4 KB )", Bytes = 4096 },
        new() { Label = "8192 bytes ( 8 KB )", Bytes = 8192 },
        new() { Label = "16384 bytes ( 16 KB )", Bytes = 16384 },
        new() { Label = "32768 bytes ( 32 KB )", Bytes = 32768 },
        new() { Label = "65536 bytes ( 64 KB )", Bytes = 65536 },
        new() { Label = "131072 bytes ( 128 KB )", Bytes = 131072 },
        new() { Label = "262144 bytes ( 256 KB )", Bytes = 262144 },
        new() { Label = "524288 bytes ( 512 KB )", Bytes = 524288 }
    };

    public static string BufferTooltip(BufferOption option) =>
        $"Each write uses {option.Label}. Larger buffers finish big files faster and use more memory.";

    private static PassPattern[] Repeat(int count, params PassPattern[] cycle)
    {
        var patterns = new PassPattern[count];
        for (int i = 0; i < count; i++)
        {
            patterns[i] = cycle[i % cycle.Length];
        }

        if (patterns[^1].Kind != PassKind.Random)
        {
            patterns[^1] = PassPattern.Random();
        }

        return patterns;
    }

    private static PassPattern[] Gutmann()
    {
        var patterns = new List<PassPattern>();
        for (int i = 0; i < 4; i++)
        {
            patterns.Add(PassPattern.Random());
        }

        patterns.Add(PassPattern.Fixed(0x55));
        patterns.Add(PassPattern.Fixed(0xAA));
        patterns.Add(PassPattern.Repeating(0x92, 0x49, 0x24));
        patterns.Add(PassPattern.Repeating(0x49, 0x24, 0x92));
        patterns.Add(PassPattern.Repeating(0x24, 0x92, 0x49));
        for (int value = 0x00; value <= 0xFF; value += 0x11)
        {
            patterns.Add(PassPattern.Fixed((byte)value));
        }

        patterns.Add(PassPattern.Repeating(0x92, 0x49, 0x24));
        patterns.Add(PassPattern.Repeating(0x49, 0x24, 0x92));
        patterns.Add(PassPattern.Repeating(0x24, 0x92, 0x49));
        patterns.Add(PassPattern.Repeating(0x6D, 0xB6, 0xDB));
        patterns.Add(PassPattern.Repeating(0xB6, 0xDB, 0x6D));
        patterns.Add(PassPattern.Repeating(0xDB, 0x6D, 0xB6));
        for (int i = 0; i < 4; i++)
        {
            patterns.Add(PassPattern.Random());
        }

        return patterns.ToArray();
    }
}

public static class ByteFormat
{
    public static string Format(long bytes)
    {
        if (bytes < 1024)
        {
            return bytes + " B";
        }

        double value = bytes;
        string[] units = { "KB", "MB", "GB", "TB" };
        int unit = -1;
        do
        {
            value /= 1024;
            unit++;
        }
        while (value >= 1024 && unit < units.Length - 1);

        return value.ToString("0.#") + " " + units[unit];
    }

    public static string Speed(double bytesPerSecond)
    {
        if (bytesPerSecond < 1)
        {
            return "--";
        }

        return Format((long)bytesPerSecond) + "/s";
    }

    public static string Eta(long remaining, double bytesPerSecond)
    {
        if (bytesPerSecond < 1 || remaining <= 0)
        {
            return remaining <= 0 ? "ETA 00:00:00" : "ETA --";
        }

        var span = TimeSpan.FromSeconds(remaining / bytesPerSecond);
        if (span.TotalHours >= 100)
        {
            return "ETA >99h";
        }

        return "ETA " + span.ToString(@"hh\:mm\:ss");
    }
}
