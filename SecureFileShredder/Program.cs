using Microsoft.Win32;

namespace SecureFileShredder
{
    internal static class Program
    { 

        [STAThread]
        static void Main(string[] args)
        { 
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 
            if (args.Length > 0)
            { 
                Application.Run(new Mainmenu(args));
            }
            else
            {
                Application.Run(new Mainmenu());
            }
        }
    }
}