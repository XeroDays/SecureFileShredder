namespace SecureFileShredder.Controls;

public sealed class ShredProgressBar : Control
{
    private readonly System.Windows.Forms.Timer timer;
    private int maximum = 10000;
    private int target;
    private double displayed;

    public ShredProgressBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        Height = 14;
        timer = new System.Windows.Forms.Timer { Interval = 30 };
        timer.Tick += (_, _) =>
        {
            if (Math.Abs(displayed - target) < 0.6)
            {
                displayed = target;
                timer.Stop();
            }
            else
            {
                displayed += (target - displayed) * 0.28;
            }

            Invalidate();
        };
    }

    public int Maximum
    {
        get => maximum;
        set => maximum = Math.Max(1, value);
    }

    public Color BarColor { get; set; } = Color.Maroon;
    public Color TrackColor { get; set; } = Color.Silver;

    public void SetTarget(int value)
    {
        target = Math.Clamp(value, 0, maximum);
        if (!timer.Enabled)
        {
            timer.Start();
        }
    }

    public void Reset()
    {
        target = 0;
        displayed = 0;
        timer.Stop();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using var track = new SolidBrush(TrackColor);
        e.Graphics.FillRectangle(track, ClientRectangle);
        float width = maximum <= 0 ? 0 : (float)(displayed / maximum * ClientSize.Width);
        if (width <= 0)
        {
            return;
        }

        using var bar = new SolidBrush(BarColor);
        e.Graphics.FillRectangle(bar, 0, 0, width, ClientSize.Height);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            timer.Dispose();
        }

        base.Dispose(disposing);
    }
}
