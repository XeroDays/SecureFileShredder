using System.Diagnostics;

namespace SecureFileShredder
{
    public partial class About : Form
    {

        private const string AppVersion = "1.7";

        public About()
        {
            InitializeComponent();
            lblVersion.Text = $"Version {AppVersion}";
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
