namespace SecureFileShredder.Models;

public sealed class ShredQueue
{
    private readonly HashSet<string> files = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> directories = new(StringComparer.OrdinalIgnoreCase);

    public List<QueueEntry> Rows { get; } = new();
    public IReadOnlyCollection<string> Files => files;
    public IReadOnlyCollection<string> Directories => directories;

    public long TotalBytes
    {
        get
        {
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
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            return total;
        }
    }

    public QueueAddResult Add(IEnumerable<string> paths)
    {
        var added = new List<QueueEntry>();
        int unreadable = 0;
        foreach (string raw in paths)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            string full;
            try
            {
                full = Path.GetFullPath(raw);
            }
            catch (Exception)
            {
                unreadable++;
                continue;
            }

            if (Directory.Exists(full))
            {
                if (directories.Add(full))
                {
                    var entry = new QueueEntry { FullPath = full, IsDirectory = true };
                    Rows.Add(entry);
                    added.Add(entry);
                }

                try
                {
                    foreach (string file in Directory.EnumerateFiles(full, "*", SearchOption.AllDirectories))
                    {
                        AddFile(file, added);
                    }
                }
                catch (Exception)
                {
                    unreadable++;
                }
            }
            else if (File.Exists(full))
            {
                AddFile(full, added);
            }
        }

        return new QueueAddResult { Added = added, Unreadable = unreadable };
    }

    public List<string> Remove(QueueEntry entry)
    {
        var removed = new List<string>();
        if (entry.IsDirectory)
        {
            directories.Remove(entry.FullPath);
            removed.Add(entry.FullPath);
            foreach (string file in files.Where(file => IsUnder(file, entry.FullPath)).ToList())
            {
                files.Remove(file);
                removed.Add(file);
            }

            Rows.RemoveAll(row => removed.Any(path => PathsEqual(row.FullPath, path)));
            return removed;
        }

        if (files.Remove(entry.FullPath))
        {
            removed.Add(entry.FullPath);
            Rows.RemoveAll(row => !row.IsDirectory && PathsEqual(row.FullPath, entry.FullPath));
        }

        return removed;
    }

    public void MarkShredded(string path) => files.Remove(path);

    public void DropVisual(string path) =>
        Rows.RemoveAll(row => !row.IsDirectory && PathsEqual(row.FullPath, path));

    public void ForgetDirectory(string path)
    {
        directories.Remove(path);
        Rows.RemoveAll(row => row.IsDirectory && PathsEqual(row.FullPath, path));
    }

    public void Clear()
    {
        files.Clear();
        directories.Clear();
        Rows.Clear();
    }

    public List<string> SnapshotFiles() => files.ToList();

    public static bool IsUnder(string filePath, string directory)
    {
        string root = Path.GetFullPath(directory)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        string full = Path.GetFullPath(filePath);
        return full.StartsWith(root, StringComparison.OrdinalIgnoreCase);
    }

    private void AddFile(string full, List<QueueEntry> added)
    {
        if (!files.Add(full))
        {
            return;
        }

        var entry = new QueueEntry { FullPath = full, IsDirectory = false };
        Rows.Add(entry);
        added.Add(entry);
    }

    private static bool PathsEqual(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
}
