using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public sealed class FreeSpaceForm : Form
{
    private readonly ShredJobOptions options;
    private readonly ComboBox drives = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label details = new() { AutoSize = false };
    private readonly Label status = new() { AutoSize = false };
    private readonly Controls.ShredProgressBar bar = new();
    private readonly Button start = new() { Text = "Wipe free space" };
    private CancellationTokenSource? cancellation;
    private bool running;

    public FreeSpaceForm(ShredJobOptions options, bool dark)
    {
        this.options = options;
        Text = "Free space wipe";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(520, 280);
        Font = new Font("Segoe UI", 9f);
        var title = new Label
        {
            Text = "Fill free space, overwrite it, then delete the temporary file. 256 MB is left free.",
            Location = new Point(16, 16),
            Size = new Size(488, 40)
        };
        drives.Location = new Point(16, 68);
        drives.Width = 488;
        details.Location = new Point(16, 104);
        details.Size = new Size(488, 40);
        bar.Location = new Point(16, 156);
        bar.Size = new Size(488, 16);
        status.Location = new Point(16, 180);
        status.Size = new Size(488, 36);
        start.Location = new Point(250, 230);
        start.Size = new Size(140, 32);
        var close = new Button { Text = "Close", Location = new Point(400, 230), Size = new Size(104, 32) };
        drives.SelectedIndexChanged += (_, _) => ShowDrive();
        start.Click += async (_, _) => await StartWipeAsync();
        close.Click += (_, _) =>
        {
            if (running)
            {
                cancellation?.Cancel();
                return;
            }

            Close();
        };
        FormClosing += (_, args) =>
        {
            if (running)
            {
                args.Cancel = true;
                cancellation?.Cancel();
            }
        };
        Controls.AddRange(new Control[] { title, drives, details, bar, status, start, close });
        LoadDrives();
        ThemePalette.Apply(this, ThemePalette.For(dark));
    }

    private void LoadDrives()
    {
        drives.Items.Clear();
        foreach (DriveInfo drive in DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType is DriveType.Fixed or DriveType.Removable))
        {
            drives.Items.Add(drive);
        }

        drives.DisplayMember = "Name";
        if (drives.Items.Count > 0)
        {
            drives.SelectedIndex = 0;
        }
        else
        {
            details.Text = "No ready drives were found.";
            start.Enabled = false;
        }
    }

    private void ShowDrive()
    {
        if (drives.SelectedItem is not DriveInfo drive)
        {
            return;
        }

        var current = new DriveInfo(drive.Name);
        long planned = FreeSpaceWiper.PlannedBytes(current.AvailableFreeSpace, FreeSpaceWiper.ReservedMarginBytes);
        details.Text = current.Name + "  " + current.DriveFormat + "  free " + ByteFormat.Format(current.AvailableFreeSpace)
            + "\r\nThis wipe would write about " + ByteFormat.Format(planned) + " using " + options.Patterns.Count + " pass(es).";
        start.Enabled = planned > 0 && !running;
    }

    private async Task StartWipeAsync()
    {
        if (drives.SelectedItem is not DriveInfo drive)
        {
            return;
        }

        string first = "Wipe free space on " + drive.Name + "? A large temporary file will be written and then shredded with "
            + options.Patterns.Count + " pass(es).";
        if (MessageBox.Show(this, first, "Free space wipe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        string second = "This keeps 256 MB free and can take a long time. Continue?";
        if (MessageBox.Show(this, second, "Confirm wipe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        running = true;
        start.Enabled = false;
        drives.Enabled = false;
        cancellation = new CancellationTokenSource();
        var progress = new Progress<FreeSpaceProgress>(update =>
        {
            status.Text = update.Phase;
            if (update.BytesTotal > 0)
            {
                bar.SetTarget((int)Math.Clamp(update.BytesCompleted * 10000d / update.BytesTotal, 0, 10000));
            }
        });

        try
        {
            FreeSpaceWipeResult result = await FreeSpaceWiper.WipeAsync(drive, options, progress, cancellation.Token);
            status.Text = result.Message;
            MessageBox.Show(this, result.Message, "Free space wipe", MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            status.Text = ex.Message;
            MessageBox.Show(this, ex.Message, "Free space wipe", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            running = false;
            drives.Enabled = true;
            cancellation.Dispose();
            cancellation = null;
            ShowDrive();
        }
    }
}
