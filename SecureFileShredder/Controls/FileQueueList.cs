using System.Drawing.Drawing2D;
using SecureFileShredder.Models;

namespace SecureFileShredder.Controls;

public sealed class FileQueueList : ListBox
{
    private readonly Dictionary<string, int> fadeAlpha = new(StringComparer.OrdinalIgnoreCase);
    private float dashOffset;

    public FileQueueList()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 22;
        IntegralHeight = false;
        BorderStyle = BorderStyle.FixedSingle;
        HorizontalScrollbar = false;
    }

    public HashSet<string> ActivePaths { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Color NormalBack { get; set; } = Color.White;
    public Color NormalText { get; set; } = Color.Black;
    public Color ActiveBack { get; set; } = Color.FromArgb(255, 228, 225);
    public Color DragBack { get; set; } = Color.FromArgb(255, 236, 232);
    public Color DragBorder { get; set; } = Color.Maroon;
    public bool DragActive { get; set; }
    public bool HasFading => fadeAlpha.Count > 0;

    public void BeginFade(string path)
    {
        fadeAlpha[path] = 255;
        ActivePaths.Remove(path);
        Invalidate();
    }

    public List<string> TickFade()
    {
        var finished = new List<string>();
        if (fadeAlpha.Count == 0)
        {
            return finished;
        }

        foreach (string key in fadeAlpha.Keys.ToList())
        {
            int alpha = fadeAlpha[key] - 28;
            if (alpha <= 0)
            {
                finished.Add(key);
            }
            else
            {
                fadeAlpha[key] = alpha;
            }
        }

        foreach (string path in finished)
        {
            fadeAlpha.Remove(path);
        }

        Invalidate();
        return finished;
    }

    public void ResetStates()
    {
        fadeAlpha.Clear();
        ActivePaths.Clear();
        DragActive = false;
        Invalidate();
    }

    public void AdvanceDash()
    {
        dashOffset = (dashOffset + 1) % 12;
        Invalidate();
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= Items.Count)
        {
            return;
        }

        string text = Items[e.Index]?.ToString() ?? "";
        bool directory = Items[e.Index] is QueueEntry { IsDirectory: true };
        bool active = ActivePaths.Contains(text);
        bool fading = fadeAlpha.TryGetValue(text, out int alpha);
        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        Color back = selected ? SystemColors.Highlight : DragActive ? DragBack : active ? ActiveBack : NormalBack;
        Color fore = selected ? SystemColors.HighlightText : NormalText;
        if (fading && !selected)
        {
            float remain = alpha / 255f;
            fore = ThemeBlend(NormalText, back, 1f - remain);
        }

        using var brush = new SolidBrush(back);
        e.Graphics.FillRectangle(brush, e.Bounds);
        FontStyle style = directory ? FontStyle.Bold : FontStyle.Regular;
        if (fading)
        {
            style |= FontStyle.Strikeout;
        }

        using var font = new Font(e.Font ?? Font, style);
        TextRenderer.DrawText(e.Graphics, directory ? "[Folder] " + text : text, font, e.Bounds, fore,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg != 0x000F || !DragActive || !IsHandleCreated)
        {
            return;
        }

        using Graphics graphics = Graphics.FromHwnd(Handle);
        using var pen = new Pen(DragBorder, 2)
        {
            DashStyle = DashStyle.Dash,
            DashOffset = dashOffset
        };
        var rect = ClientRectangle;
        rect.Inflate(-2, -2);
        graphics.DrawRectangle(pen, rect);
    }

    private static Color ThemeBlend(Color from, Color to, float amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        return Color.FromArgb(
            (int)(from.R + (to.R - from.R) * amount),
            (int)(from.G + (to.G - from.G) * amount),
            (int)(from.B + (to.B - from.B) * amount));
    }
}
