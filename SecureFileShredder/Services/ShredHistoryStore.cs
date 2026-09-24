using System.Text.Json;
using SecureFileShredder.Models;

namespace SecureFileShredder.Services;

public static class ShredHistoryStore
{
    private static readonly JsonSerializerOptions JsonOptions = new();

    public static string HistoryPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SecureFileShredder", "history.jsonl");

    public static void Append(IEnumerable<ShredHistoryEntry> entries)
    {
        var lines = entries.Select(entry => JsonSerializer.Serialize(entry, JsonOptions)).ToList();
        if (lines.Count == 0)
        {
            return;
        }

        string? directory = Path.GetDirectoryName(HistoryPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.AppendAllLines(HistoryPath, lines);
    }

    public static List<ShredHistoryEntry> ReadAll()
    {
        var entries = new List<ShredHistoryEntry>();
        if (!File.Exists(HistoryPath))
        {
            return entries;
        }

        foreach (string line in File.ReadLines(HistoryPath))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var entry = JsonSerializer.Deserialize<ShredHistoryEntry>(line);
                if (entry != null)
                {
                    entries.Add(entry);
                }
            }
            catch (JsonException)
            {
            }
        }

        entries.Reverse();
        return entries;
    }

    public static void Clear()
    {
        if (File.Exists(HistoryPath))
        {
            File.Delete(HistoryPath);
        }
    }
}
