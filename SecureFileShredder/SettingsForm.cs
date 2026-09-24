using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public sealed class SettingsForm : Form
{
    private readonly AppSettings settings;
    private readonly ComboBox profiles = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox profileName = new();
    private readonly ComboBox passes = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox buffers = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox rename = new() { Text = "Rename before delete", Checked = true };
    private readonly CheckBox timestamps = new() { Text = "Randomize timestamps", Checked = true };
    private readonly CheckBox verify = new() { Text = "Verify last pass" };
    private readonly CheckBox streams = new() { Text = "Wipe alternate data streams", Checked = true };
    private readonly CheckBox dark = new() { Text = "Dark theme" };

    public SettingsForm(AppSettings settings, int passIndex, int bufferIndex)
    {
        this.settings = settings;
        AppSettingsStore.EnsureProfiles(settings);
        Text = "Settings";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(460, 430);
        Font = new Font("Segoe UI", 9f);

        var title = new Label { Text = "Shredding profiles", Font = new Font(Font, FontStyle.Bold), Location = new Point(16, 16), AutoSize = true };
        profiles.Location = new Point(16, 44);
        profiles.Width = 280;
        var save = new Button { Text = "Save profile", Location = new Point(304, 42), Width = 140 };
        profileName.Location = new Point(16, 78);
        profileName.Width = 280;
        profileName.PlaceholderText = "Profile name";
        var delete = new Button { Text = "Delete profile", Location = new Point(304, 76), Width = 140 };
        passes.Location = new Point(16, 130);
        passes.Width = 428;
        buffers.Location = new Point(16, 186);
        buffers.Width = 428;
        rename.Location = new Point(16, 230);
        rename.AutoSize = true;
        timestamps.Location = new Point(16, 256);
        timestamps.AutoSize = true;
        verify.Location = new Point(16, 282);
        verify.AutoSize = true;
        streams.Location = new Point(16, 308);
        streams.AutoSize = true;
        dark.Location = new Point(16, 334);
        dark.AutoSize = true;
        var apply = new Button { Text = "Apply and close", Location = new Point(210, 382), Width = 130 };
        var cancel = new Button { Text = "Cancel", Location = new Point(348, 382), Width = 96 };

        var passLabel = new Label { Text = "Passes", Location = new Point(16, 110), AutoSize = true };
        var bufferLabel = new Label { Text = "Buffer", Location = new Point(16, 166), AutoSize = true };

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

        profiles.SelectedIndexChanged += (_, _) => LoadSelectedProfile();
        save.Click += (_, _) => SaveProfile();
        delete.Click += (_, _) => DeleteProfile();
        apply.Click += (_, _) =>
        {
            WriteFlags();
            settings.PassIndex = passes.SelectedIndex;
            settings.BufferIndex = buffers.SelectedIndex;
            settings.SelectedProfile = profiles.SelectedItem?.ToString() ?? settings.SelectedProfile;
            DialogResult = DialogResult.OK;
            Close();
        };
        cancel.Click += (_, _) => Close();

        Controls.AddRange(new Control[]
        {
            title, profiles, save, profileName, delete, passLabel, passes, bufferLabel, buffers,
            rename, timestamps, verify, streams, dark, apply, cancel
        });
        ThemePalette.Apply(this, ThemePalette.For(settings.DarkTheme));
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
