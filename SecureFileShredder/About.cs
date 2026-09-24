using System.Diagnostics;
using SecureFileShredder.Theming;

namespace SecureFileShredder
{
    public partial class About : Form
    {

        public About()
        {
            InitializeComponent();
            lblVersion.Text = $"Version {Application.ProductVersion}";
        }

        public void ApplyTheme(ThemePalette theme)
        {
            BackColor = theme.FormBack;
            label1.ForeColor = theme.Accent;
            label2.ForeColor = theme.Text;
            lblVersion.ForeColor = theme.Muted;
            label4.ForeColor = theme.Muted;
            label5.ForeColor = theme.Text;
            btnLink.ForeColor = theme.Link;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void btnLink_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.softasium.com") { UseShellExecute = true });
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
