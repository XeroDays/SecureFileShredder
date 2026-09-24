using System.Text.Json;
using SecureFileShredder.Models;

namespace SecureFileShredder.Services;

public static class AppSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string SettingsPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SecureFileShredder", "settings.json");

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var loaded = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath));
                if (loaded != null)
                {
                    EnsureProfiles(loaded);
                    return loaded;
                }
            }
        }
        catch (Exception)
        {
        }

        var settings = new AppSettings();
        EnsureProfiles(settings);
        return settings;
    }

    public static void Save(AppSettings settings)
    {
        EnsureProfiles(settings);
        string? directory = Path.GetDirectoryName(SettingsPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(settings, JsonOptions));
    }

    public static void EnsureProfiles(AppSettings settings)
    {
        settings.Profiles ??= new List<ShredProfile>();
        UpsertBuiltIn(settings, new ShredProfile
        {
            Name = "Quick",
            BuiltIn = true,
            PassIndex = 0,
            BufferIndex = 2,
            RenameBeforeDelete = true,
            RandomizeTimestamps = true,
            VerifyOverwrite = false,
            WipeAlternateStreams = true
        });
        UpsertBuiltIn(settings, new ShredProfile
        {
            Name = "Thorough",
            BuiltIn = true,
            PassIndex = 2,
            BufferIndex = 6,
            RenameBeforeDelete = true,
            RandomizeTimestamps = true,
            VerifyOverwrite = true,
            WipeAlternateStreams = true
        });
    }

    private static void UpsertBuiltIn(AppSettings settings, ShredProfile profile)
    {
        if (settings.Profiles.Any(item => string.Equals(item.Name, profile.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        settings.Profiles.Add(profile);
    }
}
