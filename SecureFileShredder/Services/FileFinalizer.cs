namespace SecureFileShredder.Services;

public static class FileFinalizer
{
    public static string? FinalizeFile(string path, bool rename, bool timestamps)
    {
        string? note = null;
        string current = path;
        if (rename && File.Exists(current))
        {
            try
            {
                string? directory = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directory))
                {
                    directory = ".";
                }

                for (int attempt = 0; attempt < 3; attempt++)
                {
                    string next = Path.Combine(directory, "sfs-" + Guid.NewGuid().ToString("N"));
                    File.Move(current, next);
                    current = next;
                }
            }
            catch (Exception ex)
            {
                note = "Rename failed: " + ex.Message;
                current = File.Exists(path) ? path : current;
            }
        }

        if (timestamps && File.Exists(current))
        {
            try
            {
                var when = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 4000));
                File.SetCreationTimeUtc(current, when);
                File.SetLastWriteTimeUtc(current, when);
                File.SetLastAccessTimeUtc(current, when);
            }
            catch (Exception ex)
            {
                string timestampNote = "Timestamp change failed: " + ex.Message;
                note = string.IsNullOrEmpty(note) ? timestampNote : note + " " + timestampNote;
            }
        }

        if (File.Exists(current))
        {
            File.Delete(current);
        }
        else if (!string.Equals(current, path, StringComparison.OrdinalIgnoreCase) && File.Exists(path))
        {
            File.Delete(path);
        }

        return note;
    }
}
