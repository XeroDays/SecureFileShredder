using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public partial class FreeSpaceForm : Form
{
    private readonly ShredJobOptions options;
    private CancellationTokenSource? cancellation;
    private bool running;

    public FreeSpaceForm(ShredJobOptions options, bool dark)
    {
        this.options = options;
        InitializeComponent();
        LoadDrives();
        ThemePalette.Apply(this, ThemePalette.For(dark));
    }

    private void drives_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ShowDrive();
    }

    private async void btnStart_Click(object? sender, EventArgs e)
    {
        await StartWipeAsync();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        if (running)
        {
            cancellation?.Cancel();
            return;
        }

        Close();
    }

    private void FreeSpaceForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (running)
        {
            e.Cancel = true;
            cancellation?.Cancel();
        }
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
            btnStart.Enabled = false;
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
        btnStart.Enabled = planned > 0 && !running;
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
        btnStart.Enabled = false;
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
