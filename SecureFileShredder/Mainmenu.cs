using Microsoft.Win32;
using SecureFileShredder.Controllers;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SecureFileShredder
{
    public partial class Mainmenu : Form
    {

        static int PASSES;
        static int Buffer_Size;

        List<string> listofPaths = new List<string>();
        List<string> listOfDirectories = new List<string>();
        ShredBatchResult? lastShredResult;
        NotifyIcon? notifyIcon;
        Icon? trayProgressIcon;
        Icon? trayBaseIcon;
        bool isMinimizedToTray;
        int lastTrayPercent = -1;

        private sealed class ShredBatchResult
        {
            public List<string> Succeeded { get; } = new List<string>();
            public List<(string Path, string Reason)> Failed { get; } = new List<(string Path, string Reason)>();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_COPYDATA = 0x004A;
            if (m.Msg == WM_COPYDATA)
            {
                COPYDATASTRUCT data = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                string filePaths = Marshal.PtrToStringAnsi(data.lpData);

                if (!string.IsNullOrEmpty(filePaths))
                {
                    // Split and process the received file paths
                    string[] files = filePaths.Split('|');
                    updateListWithFiles(files);
                }
            }

            base.WndProc(ref m);
        }

        public Mainmenu(string[]? args = null)
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeNotifyIcon();
            progressBar.Visible = false;
            setupPassesCombo();
            setupBufferSizeCombo();
            FormClosing += Mainmenu_FormClosing;

            if (args != null && args.Length > 0)
            {
                updateListWithFiles(args);
            }
        }

        private void InitializeNotifyIcon()
        {
            var trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Restore", null, (_, _) => RestoreFromTray());

            trayBaseIcon = Icon ?? SystemIcons.Application;
            notifyIcon = new NotifyIcon
            {
                Icon = trayBaseIcon,
                Text = "Secure File Shredder",
                Visible = false,
                ContextMenuStrip = trayMenu
            };
            notifyIcon.DoubleClick += (_, _) => RestoreFromTray();
        }

        private int GetShredPercent()
        {
            if (progressBar.Maximum <= 0)
            {
                return 0;
            }

            return (int)(progressBar.Value * 100.0 / progressBar.Maximum);
        }

        private void UpdateTrayProgressIcon(int percent)
        {
            if (notifyIcon == null)
            {
                return;
            }

            if (percent <= 0)
            {
                if (trayBaseIcon != null)
                {
                    notifyIcon.Icon = trayBaseIcon;
                }
                DisposeTrayProgressIcon();
                lastTrayPercent = 0;
                return;
            }

            int pct = Math.Clamp(percent, 1, 100);
            if (pct == lastTrayPercent)
            {
                return;
            }

            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "TaskbarIcon", $"pct_{pct:D3}.ico");
            if (!File.Exists(path))
            {
                return;
            }

            Icon newIcon = new Icon(path);
            notifyIcon.Icon = newIcon;
            DisposeTrayProgressIcon();
            trayProgressIcon = newIcon;
            lastTrayPercent = pct;
        }

        private void DisposeTrayProgressIcon()
        {
            if (trayProgressIcon != null)
            {
                trayProgressIcon.Dispose();
                trayProgressIcon = null;
            }
        }

        private void MinimizeToTray()
        {
            if (notifyIcon == null)
            {
                return;
            }

            isMinimizedToTray = true;
            int pct = GetShredPercent();
            notifyIcon.Text = pct > 0 ? $"Shredding: {pct}%" : "Shredding...";
            UpdateTrayProgressIcon(pct);
            notifyIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }

        private void RestoreFromTray()
        {
            isMinimizedToTray = false;
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Text = "Secure File Shredder";
                if (trayBaseIcon != null)
                {
                    notifyIcon.Icon = trayBaseIcon;
                }
                DisposeTrayProgressIcon();
                lastTrayPercent = -1;
            }
        }

        private void Mainmenu_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                if (trayBaseIcon != null)
                {
                    notifyIcon.Icon = trayBaseIcon;
                }
                DisposeTrayProgressIcon();
                notifyIcon.Dispose();
                notifyIcon = null;
            }
        }


        private void setupBufferSizeCombo()
        {
            cmbBufferSize.Items.Add("1024 bytes ( 1 KB )");
            cmbBufferSize.Items.Add("2048 bytes ( 2 KB )");
            cmbBufferSize.Items.Add("Default ( 4 KB )");
            cmbBufferSize.Items.Add("8192 bytes ( 8 KB )");
            cmbBufferSize.Items.Add("16384 bytes ( 16 KB )");
            cmbBufferSize.Items.Add("32768 bytes ( 32 KB )");
            cmbBufferSize.Items.Add("65536 bytes ( 64 KB )");
            cmbBufferSize.Items.Add("131072 bytes ( 128 KB )");
            cmbBufferSize.Items.Add("262144 bytes ( 256 KB )");
            cmbBufferSize.Items.Add("524288 bytes ( 512 KB )");
            cmbBufferSize.SelectedIndex = 2;
        }

        private void setupPassesCombo()
        {
            cmbPasses.Items.Add("Normal ( 1 Passes )");
            cmbPasses.Items.Add("DoD ( 3 Passes )");
            cmbPasses.Items.Add("DoD 5220.M ( 7 Passes )");
            cmbPasses.Items.Add("NSA ( 12 Passes )");
            cmbPasses.Items.Add("GUTTMAN ( 35 Passes )");
            cmbPasses.Items.Add("SFIK V1 ( 55 Passes )");
            cmbPasses.SelectedIndex = 0;
        }


        private void updateListWithFiles(string[] files)
        {
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    listBoxFiles.Items.Add(file);
                    listOfDirectories.Add(file);
                }
            }

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    listofPaths.Add(file);
                    listBoxFiles.Items.Add(file);
                }
                else if (Directory.Exists(file))
                {
                    string[] filesInDirectory = Directory.GetFiles(file, "*.*", SearchOption.AllDirectories);
                    foreach (string fileInDirectory in filesInDirectory)
                    {
                        listofPaths.Add(fileInDirectory);
                        listBoxFiles.Items.Add(fileInDirectory);
                    }
                }
            }
            listofPaths = listofPaths.Distinct().ToList();

            // if listofpaths is empty then clean the listbox and directories list
            if (listofPaths.Count == 0)
            {
                listBoxFiles.Items.Clear();
                listOfDirectories.Clear();
            }
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> filesToShred = (List<string>)e.Argument;
            int progress = 0;
            var result = new ShredBatchResult();

            foreach (string file in filesToShred)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                try
                {
                    ShredFile(file, backgroundWorker, ref progress);
                    result.Succeeded.Add(file);
                    listBoxFiles.Invoke(new Action(() => listBoxFiles.Items.Remove(file)));
                }
                catch (Exception ex)
                {
                    result.Failed.Add((file, ex.Message));
                }
            }

            lastShredResult = result;
            e.Result = result;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (notifyIcon != null && notifyIcon.Visible && progressBar.Maximum > 0)
            {
                int pct = GetShredPercent();
                notifyIcon.Text = $"Shredding: {pct}%";
                UpdateTrayProgressIcon(pct);
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isMinimizedToTray)
            {
                RestoreFromTray();
            }

            btnClose.Visible = true;

            if (e.Cancelled)
            {
                FinalizeSucceededFiles(lastShredResult);
                MessageBox.Show("File shredding operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnStartDeleting.Visible = true;
                progressBar.Visible = false;
                lastShredResult = null;
                return;
            }

            if (e.Error != null)
            {
                MessageBox.Show("An error occurred during file shredding: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var result = e.Result as ShredBatchResult ?? lastShredResult ?? new ShredBatchResult();
                FinalizeSucceededFiles(result);

                if (result.Failed.Count == 0)
                {
                    listBoxFiles.Items.Clear();
                    listofPaths.Clear();
                    listOfDirectories.Clear();
                    MessageBox.Show("Files have been shredded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string firstReason = result.Failed[0].Reason;
                    MessageBox.Show(
                        $"{result.Succeeded.Count} file(s) shredded successfully.\n" +
                        $"{result.Failed.Count} file(s) could not be shredded and remain in the list.\n\n" +
                        $"Example: {firstReason}",
                        "Completed with errors",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            btnStartDeleting.Visible = true;
            progressBar.Visible = false;
            lastShredResult = null;
        }

        private void FinalizeSucceededFiles(ShredBatchResult? result)
        {
            if (result == null || result.Succeeded.Count == 0)
            {
                return;
            }

            progressBar.Value = 0;
            progressBar.Maximum = result.Succeeded.Count;

            foreach (string file in result.Succeeded)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                listofPaths.Remove(file);
                if (progressBar.Value < progressBar.Maximum)
                {
                    progressBar.Value++;
                }
            }

            var directoriesToRemove = new List<string>();
            foreach (string directory in listOfDirectories)
            {
                bool hasRemainingFiles = listofPaths.Any(path => IsPathUnderDirectory(path, directory));
                if (hasRemainingFiles)
                {
                    continue;
                }

                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }

                directoriesToRemove.Add(directory);
                listBoxFiles.Items.Remove(directory);
            }

            foreach (string directory in directoriesToRemove)
            {
                listOfDirectories.Remove(directory);
            }
        }

        private static bool IsPathUnderDirectory(string filePath, string directory)
        {
            string directoryFull = Path.GetFullPath(directory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;
            string fileFull = Path.GetFullPath(filePath);
            return fileFull.StartsWith(directoryFull, StringComparison.OrdinalIgnoreCase);
        }

        private void btnStartDeleting_Click(object sender, EventArgs e)
        {
            string passes = cmbPasses.SelectedItem.ToString().Split("( ").Last().Split("Passes").First().Trim();
            string selectedBufferSize = cmbBufferSize.SelectedItem.ToString().Split("( ").Last().Split("KB").First().Trim();

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete these files?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            PASSES = Convert.ToInt32(passes);
            Buffer_Size = Convert.ToInt32(selectedBufferSize) * 1024;

            if (listofPaths.Count == 0)
            {
                MessageBox.Show("No files found here. Are they already gone?", "No files to Shredd", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnStartDeleting.Visible = false;
            btnClose.Visible = false;
            progressBar.Visible = true;
            progressBar.Maximum = listofPaths.Count * PASSES;
            progressBar.Value = 0;
            backgroundWorker.RunWorkerAsync(listofPaths);
        }

        static void ShredFile(string filePath, BackgroundWorker worker, ref int progress)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            ShredderController shredder = new ShredderController();
            shredder.ShreddFile(filePath, PASSES, Buffer_Size, worker, ref progress);
        }


        #region Designer code
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                return;
            }

            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                MinimizeToTray();
                return;
            }

            WindowState = FormWindowState.Minimized;
        }

        //generate method to drag files into the listbox and list hte file paths in the listbox 
        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            updateListWithFiles(files);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }



        private void btnInfo_Click(object sender, EventArgs e)
        {
            Hide();
            try
            {
                new About().ShowDialog();
            }
            finally
            {
                Show();
                Activate();
            }
        }


        #endregion

    }

}
