using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public partial class HistoryForm : Form
{
    public HistoryForm(bool dark)
    {
        InitializeComponent();
        Reload();
        ThemePalette.Apply(this, ThemePalette.For(dark));
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Clear the shred history on this PC?", "History", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        ShredHistoryStore.Clear();
        list.Items.Clear();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void Reload()
    {
        list.Items.Clear();
        foreach (ShredHistoryEntry entry in ShredHistoryStore.ReadAll())
        {
            var item = new ListViewItem(entry.TimeUtc.ToLocalTime().ToString("g"));
            item.SubItems.Add(entry.Result);
            item.SubItems.Add(entry.Passes.ToString());
            item.SubItems.Add(ByteFormat.Format(entry.Bytes));
            item.SubItems.Add(entry.Path);
            item.SubItems.Add(entry.Detail ?? "");
            list.Items.Add(item);
        }
    }
}
