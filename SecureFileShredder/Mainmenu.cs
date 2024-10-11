using Microsoft.Win32;
using System.ComponentModel; 
using System.Security.Cryptography; 

namespace SecureFileShredder
{
    public partial class Mainmenu : Form
    {

        static int PASSES;

        List<string> listofPaths = new List<string>();
        List<string> listOfDirectories = new List<string>();

      
        public Mainmenu(string[]? args=null)
        { 
            InitializeComponent();
            InitializeBackgroundWorker();
            progressBar.Visible = false; 

            if (args != null && args.Length > 0)
            {
                updateListWithFiles (args);
            }
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

            foreach (string file in filesToShred)
            {

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                ShredFile(file, backgroundWorker, ref progress); 
                listBoxFiles.Invoke(new Action(() => listBoxFiles.Items.Remove(file))); 
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            if (e.Cancelled)
            {
                MessageBox.Show("File shredding operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show("An error occurred during file shredding: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            { 
                progressBar.Value = 0;
                progressBar.Maximum = listofPaths.Count;
                foreach (string file in listofPaths)
                {
                    File.Delete(file);
                    progressBar.Value++;
                }  
                  
                foreach (string directory in listOfDirectories)
                {
                    Directory.Delete(directory, true);
                }

                MessageBox.Show("Files have been shredded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            trackBar1.Enabled = true;
            btnStartDeleting.Visible = true;
            progressBar.Visible = false;
            listBoxFiles.Items.Clear();
            listofPaths.Clear();

        }

        private void cancelRunner()
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }
         
        private void trackBar1_Scroll(object sender, System.EventArgs e)
        { 
            lblPasses.Text = "PASSES  -  " + trackBar1.Value;
        }

        private void btnStartDeleting_Click(object sender, EventArgs e)
        { 
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete these files?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
              
            PASSES = Convert.ToInt32(trackBar1.Value);
            if (listofPaths.Count == 0)
            {
                MessageBox.Show("Please add files to shred.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 
            trackBar1.Enabled = false;
            btnStartDeleting.Visible = false;
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
            OverwriteFile(filePath, PASSES, worker, ref progress);
        }


        static void OverwriteFile(string filePath, int passes, BackgroundWorker worker, ref int progress)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4096];  
                long fileLength = new FileInfo(filePath).Length; 

                for (int i = 0; i < passes; i++)
                {
                    worker.ReportProgress(++progress);
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                    {
                        long position = 0;
                        while (position < fileLength)
                        {
                            rng.GetBytes(data);
                            fs.Write(data, 0, (int)Math.Min(data.Length, fileLength - position));
                            position += data.Length;
                        }
                    }
                }
            }
        }



        #region Designer code
        private void btnClose_Click(object sender, EventArgs e)
        {
            cancelRunner();
            Application.Exit();
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


        #endregion

    }
}
