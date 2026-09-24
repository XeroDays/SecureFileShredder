using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public partial class SettingsForm : Form
{
    private readonly AppSettings settings;

    public SettingsForm(AppSettings settings, int passIndex, int bufferIndex)
    {
        this.settings = settings;
        InitializeComponent();
        AppSettingsStore.EnsureProfiles(settings);

        foreach (ShredPreset preset in PresetCatalog.PassPresets)
        {
            passes.Items.Add(preset);
        }

        foreach (BufferOption option in PresetCatalog.Buffers)
        {
            buffers.Items.Add(option);
        }

        foreach (ShredProfile profile in settings.Profiles)
        {
            profiles.Items.Add(profile.Name);
        }

        passes.SelectedIndex = Clamp(passIndex, passes.Items.Count);
        buffers.SelectedIndex = Clamp(bufferIndex, buffers.Items.Count);
        dark.Checked = settings.DarkTheme;
        rename.Checked = settings.RenameBeforeDelete;
        timestamps.Checked = settings.RandomizeTimestamps;
        verify.Checked = settings.VerifyOverwrite;
        streams.Checked = settings.WipeAlternateStreams;
        int selected = settings.Profiles.FindIndex(profile => string.Equals(profile.Name, settings.SelectedProfile, StringComparison.OrdinalIgnoreCase));
        profiles.SelectedIndex = selected >= 0 ? selected : 0;
        LoadSelectedProfile();
        ThemePalette.Apply(this, ThemePalette.For(settings.DarkTheme));
    }

    private void profiles_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadSelectedProfile();
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        SaveProfile();
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        DeleteProfile();
    }

    private void btnApply_Click(object? sender, EventArgs e)
    {
        WriteFlags();
        settings.PassIndex = passes.SelectedIndex;
        settings.BufferIndex = buffers.SelectedIndex;
        settings.SelectedProfile = profiles.SelectedItem?.ToString() ?? settings.SelectedProfile;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void LoadSelectedProfile()
    {
        ShredProfile? profile = CurrentProfile();
        if (profile == null)
        {
            return;
        }

        profileName.Text = profile.Name;
        passes.SelectedIndex = Clamp(profile.PassIndex, passes.Items.Count);
        buffers.SelectedIndex = Clamp(profile.BufferIndex, buffers.Items.Count);
        rename.Checked = profile.RenameBeforeDelete;
        timestamps.Checked = profile.RandomizeTimestamps;
        verify.Checked = profile.VerifyOverwrite;
        streams.Checked = profile.WipeAlternateStreams;
    }

    private void SaveProfile()
    {
        string name = profileName.Text.Trim();
        if (name.Length == 0)
        {
            MessageBox.Show(this, "Enter a profile name.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        ShredProfile? existing = settings.Profiles.FirstOrDefault(profile => string.Equals(profile.Name, name, StringComparison.OrdinalIgnoreCase));
        if (existing == null)
        {
            existing = new ShredProfile { Name = name };
            settings.Profiles.Add(existing);
            profiles.Items.Add(name);
        }

        existing.PassIndex = passes.SelectedIndex;
        existing.BufferIndex = buffers.SelectedIndex;
        existing.RenameBeforeDelete = rename.Checked;
        existing.RandomizeTimestamps = timestamps.Checked;
        existing.VerifyOverwrite = verify.Checked;
        existing.WipeAlternateStreams = streams.Checked;
        profiles.SelectedItem = existing.Name;
        settings.SelectedProfile = existing.Name;
        AppSettingsStore.Save(settings);
    }

    private void DeleteProfile()
    {
        ShredProfile? profile = CurrentProfile();
        if (profile == null)
        {
            return;
        }

        if (profile.BuiltIn)
        {
            MessageBox.Show(this, "Built-in profiles stay in the list.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        settings.Profiles.Remove(profile);
        profiles.Items.Remove(profile.Name);
        AppSettingsStore.Save(settings);
        if (profiles.Items.Count > 0)
        {
            profiles.SelectedIndex = 0;
        }
    }

    private void WriteFlags()
    {
        settings.DarkTheme = dark.Checked;
        settings.RenameBeforeDelete = rename.Checked;
        settings.RandomizeTimestamps = timestamps.Checked;
        settings.VerifyOverwrite = verify.Checked;
        settings.WipeAlternateStreams = streams.Checked;
        ShredProfile? profile = CurrentProfile();
        if (profile != null)
        {
            profile.PassIndex = passes.SelectedIndex;
            profile.BufferIndex = buffers.SelectedIndex;
            profile.RenameBeforeDelete = rename.Checked;
            profile.RandomizeTimestamps = timestamps.Checked;
            profile.VerifyOverwrite = verify.Checked;
            profile.WipeAlternateStreams = streams.Checked;
        }
    }

    private ShredProfile? CurrentProfile()
    {
        string? name = profiles.SelectedItem?.ToString();
        return settings.Profiles.FirstOrDefault(profile => string.Equals(profile.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    private static int Clamp(int index, int count) => count == 0 ? -1 : Math.Clamp(index, 0, count - 1);
}
