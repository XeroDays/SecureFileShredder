using System.Security.Cryptography;
using SecureFileShredder.Controllers;
using SecureFileShredder.Models;
using SecureFileShredder.Services;

int failed = 0;

void Check(bool condition, string message)
{
    if (condition)
    {
        Console.WriteLine("OK  " + message);
        return;
    }

    failed++;
    Console.WriteLine("FAIL  " + message);
}

ShredPreset three = PresetCatalog.PassPresets[1];
Check(three.Patterns.Count == 3, "DoD preset has 3 passes");
Check(three.Patterns[0].Kind == PassKind.Byte && three.Patterns[0].Value == 0x00, "DoD first pass is 0x00");
Check(three.Patterns[2].Kind == PassKind.Random, "DoD last pass is random");

ShredPreset gutmann = PresetCatalog.PassPresets[4];
Check(gutmann.Patterns.Count == 35, "Gutmann preset has 35 passes");
Check(gutmann.Patterns[4].Kind == PassKind.Byte && gutmann.Patterns[4].Value == 0x55, "Gutmann pass 5 is 0x55");
Check(gutmann.Patterns[0].Kind == PassKind.Random && gutmann.Patterns[^1].Kind == PassKind.Random, "Gutmann starts and ends with random");

Check(FreeSpaceWiper.PlannedBytes(100, 256) == 0, "No wipe when free space is below the margin");
Check(FreeSpaceWiper.PlannedBytes(300, 256) == 44, "Planned bytes keep the reserved margin");

string root = Path.Combine(Path.GetTempPath(), "sfs-tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
try
{
    string file = Path.Combine(root, "same.txt");
    File.WriteAllText(file, "hello");
    string nestedDir = Path.Combine(root, "folder");
    Directory.CreateDirectory(nestedDir);
    string nested = Path.Combine(nestedDir, "nested.txt");
    File.WriteAllText(nested, "nested");
    var queue = new ShredQueue();
    queue.Add(new[] { file, file, nestedDir, nested });
    Check(queue.Files.Count == 2, "Queue keeps each file once");
    Check(queue.Rows.Count(row => row.IsDirectory) == 1, "Folder row is added once");

    string target = Path.Combine(root, "shred.bin");
    byte[] original = new byte[2000];
    RandomNumberGenerator.Fill(original);
    File.WriteAllBytes(target, original);
    File.SetAttributes(target, FileAttributes.ReadOnly);
    var options = new ShredJobOptions
    {
        Patterns = new[] { PassPattern.Fixed(0xA5) },
        BufferBytes = 512,
        VerifyOverwrite = true
    };
    new ShredderController().ShredFile(target, options, CancellationToken.None, null);
    byte[] after = File.ReadAllBytes(target);
    Check(after.Length == original.Length && after.All(value => value == 0xA5), "Fixed pass overwrites every byte");
    Check((File.GetAttributes(target) & FileAttributes.ReadOnly) == 0, "Read-only attribute is cleared");
}
finally
{
    try
    {
        if (Directory.Exists(root))
        {
            foreach (string path in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }

            Directory.Delete(root, true);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Cleanup: " + ex.Message);
    }
}

return failed == 0 ? 0 : 1;
