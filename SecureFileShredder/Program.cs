using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SecureFileShredder
{
    internal static class Program
    {
         
        const int WM_COPYDATA = 0x004A;
         

        [STAThread]
        static void Main(string[] args)
        {
            bool isNewInstance;
             
            using (Mutex mutex = new Mutex(true, "SecureShredder", out isNewInstance))
            {
                if (isNewInstance)
                {
                    ApplicationConfiguration.Initialize();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false); 
                    Application.Run(new Mainmenu(args.Length > 0 ? args : null)); 
                }
                else
                {
                    Thread.Sleep(200);
                    SendFilesToExistingInstance(args);
                }
            }
        }

        private static void SendFilesToExistingInstance(string[] args)
        { 
            IntPtr hWnd = FindWindow(null, "Mainmenu");  
            if (hWnd != IntPtr.Zero && args.Length > 0)
            {
               
                string filePaths = string.Join("|", args); 
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds.dwData = IntPtr.Zero;
                cds.cbData = filePaths.Length + 1;
                cds.lpData = Marshal.StringToHGlobalAnsi(filePaths); 
                SendMessage(hWnd, WM_COPYDATA, IntPtr.Zero, ref cds); 
                Marshal.FreeHGlobal(cds.lpData);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
     
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
}