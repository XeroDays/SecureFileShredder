using SecureFileShredder.Models;

namespace SecureFileShredder.Theming;

public sealed class ThemePalette
{
    public required bool Dark { get; init; }
    public required Color FormBack { get; init; }
    public required Color Text { get; init; }
    public required Color Muted { get; init; }
    public required Color Accent { get; init; }
    public required Color ButtonBack { get; init; }
    public required Color ButtonPulse { get; init; }
    public required Color ButtonFore { get; init; }
    public required Color ListBack { get; init; }
    public required Color ListText { get; init; }
    public required Color ActiveRow { get; init; }
    public required Color DragBack { get; init; }
    public required Color DragBorder { get; init; }
    public required Color Track { get; init; }
    public required Color Bar { get; init; }
    public required Color InputBack { get; init; }
    public required Color Link { get; init; }

    public static ThemePalette For(bool dark) => dark ? DarkTheme() : LightTheme();

    public static ThemePalette LightTheme() => new()
    {
        Dark = false,
        FormBack = Color.FromArgb(224, 224, 224),
        Text = Color.Black,
        Muted = Color.DimGray,
        Accent = Color.Maroon,
        ButtonBack = Color.Red,
        ButtonPulse = Color.FromArgb(255, 120, 120),
        ButtonFore = Color.White,
        ListBack = Color.White,
        ListText = Color.Black,
        ActiveRow = Color.FromArgb(255, 228, 225),
        DragBack = Color.FromArgb(255, 236, 232),
        DragBorder = Color.Maroon,
        Track = Color.Silver,
        Bar = Color.Maroon,
        InputBack = Color.White,
        Link = Color.Blue
    };

    public static ThemePalette DarkTheme() => new()
    {
        Dark = true,
        FormBack = Color.FromArgb(30, 30, 30),
        Text = Color.FromArgb(240, 240, 240),
        Muted = Color.FromArgb(170, 170, 170),
        Accent = Color.FromArgb(224, 112, 112),
        ButtonBack = Color.FromArgb(139, 30, 30),
        ButtonPulse = Color.FromArgb(190, 70, 70),
        ButtonFore = Color.White,
        ListBack = Color.FromArgb(42, 42, 42),
        ListText = Color.FromArgb(240, 240, 240),
        ActiveRow = Color.FromArgb(92, 42, 42),
        DragBack = Color.FromArgb(62, 36, 36),
        DragBorder = Color.FromArgb(224, 112, 112),
        Track = Color.FromArgb(60, 60, 60),
        Bar = Color.FromArgb(176, 64, 64),
        InputBack = Color.FromArgb(48, 48, 48),
        Link = Color.FromArgb(140, 180, 255)
    };

    public static Color Blend(Color from, Color to, double amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        return Color.FromArgb(
            (int)(from.R + (to.R - from.R) * amount),
            (int)(from.G + (to.G - from.G) * amount),
            (int)(from.B + (to.B - from.B) * amount));
    }

    public static void Apply(Form form, ThemePalette theme)
    {
        form.BackColor = theme.FormBack;
        form.ForeColor = theme.Text;
        ApplyControls(form.Controls, theme);
    }

    private static void ApplyControls(Control.ControlCollection controls, ThemePalette theme)
    {
        foreach (Control control in controls)
        {
            switch (control)
            {
                case Controls.FileQueueList list:
                    list.BackColor = theme.ListBack;
                    list.NormalBack = theme.ListBack;
                    list.NormalText = theme.ListText;
                    list.ActiveBack = theme.ActiveRow;
                    list.DragBack = theme.DragBack;
                    list.DragBorder = theme.DragBorder;
                    list.ForeColor = theme.ListText;
                    break;
                case Controls.ShredProgressBar bar:
                    bar.BarColor = theme.Bar;
                    bar.TrackColor = theme.Track;
                    break;
                case Button button:
                    button.FlatStyle = FlatStyle.Flat;
                    button.UseVisualStyleBackColor = false;
                    if (button.Name == "btnStartDeleting")
                    {
                        button.BackColor = theme.ButtonBack;
                        button.ForeColor = theme.ButtonFore;
                        button.FlatAppearance.BorderColor = theme.Accent;
                    }
                    else
                    {
                        button.BackColor = theme.InputBack;
                        button.ForeColor = theme.Text;
                        button.FlatAppearance.BorderColor = theme.Accent;
                    }
                    break;
                case ComboBox combo:
                    combo.BackColor = theme.InputBack;
                    combo.ForeColor = theme.Text;
                    break;
                case TextBox textBox:
                    textBox.BackColor = theme.InputBack;
                    textBox.ForeColor = theme.Text;
                    break;
                case CheckBox checkBox:
                    checkBox.ForeColor = theme.Text;
                    checkBox.BackColor = theme.FormBack;
                    break;
                case ListView listView:
                    listView.BackColor = theme.ListBack;
                    listView.ForeColor = theme.ListText;
                    break;
                case LinkLabel link:
                    link.LinkColor = theme.Link;
                    break;
                case Label label:
                    label.BackColor = Color.Transparent;
                    label.ForeColor = label.Name is "label1" or "btnMinimize" or "label2" && label.Font.Bold
                        ? theme.Accent
                        : label.Name is "lblSummary" or "lblMetrics" or "lblResult" or "lblVersion" or "label4"
                            ? theme.Muted
                            : theme.Text;
                    break;
                case PictureBox:
                    break;
                default:
                    control.BackColor = theme.FormBack;
                    control.ForeColor = theme.Text;
                    break;
            }

            if (control.HasChildren)
            {
                ApplyControls(control.Controls, theme);
            }
        }
    }
}
