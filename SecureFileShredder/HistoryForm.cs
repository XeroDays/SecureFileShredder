using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder;

public sealed class HistoryForm : Form
{
    private readonly ListView list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        GridLines = true,
        Dock = DockStyle.Fill
    };

    public HistoryForm(bool dark)
    {
        Text = "Shred history";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(860, 460);
        Font = new Font("Segoe UI", 9f);
        list.Columns.Add("When", 150);
        list.Columns.Add("Result", 80);
        list.Columns.Add("Passes", 60);
        list.Columns.Add("Size", 80);
        list.Columns.Add("Path", 320);
        list.Columns.Add("Detail", 160);
        var clear = new Button { Text = "Clear log", Dock = DockStyle.Right, Width = 100 };
        var close = new Button { Text = "Close", Dock = DockStyle.Right, Width = 100 };
        var bar = new Panel { Dock = DockStyle.Bottom, Height = 48 };
        bar.Controls.Add(clear);
        bar.Controls.Add(close);
        Controls.Add(list);
        Controls.Add(bar);
        clear.Click += (_, _) =>
        {
            if (MessageBox.Show(this, "Clear the shred history on this PC?", "History", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            ShredHistoryStore.Clear();
            list.Items.Clear();
        };
        close.Click += (_, _) => Close();
        Reload();
        ThemePalette.Apply(this, ThemePalette.For(dark));
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
