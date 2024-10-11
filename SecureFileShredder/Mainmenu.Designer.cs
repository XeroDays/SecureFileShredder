namespace SecureFileShredder
{
    partial class Mainmenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainmenu));
            label1 = new Label();
            btnClose = new PictureBox();
            fileSystemWatcher1 = new FileSystemWatcher();
            listBoxFiles = new ListBox();
            btnStartDeleting = new Button();
            label2 = new Label();
            progressBar = new ProgressBar();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            lblPasses = new Label();
            trackBar1 = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(7, 8);
            label1.Name = "label1";
            label1.Size = new Size(266, 37);
            label1.TabIndex = 0;
            label1.Text = "Secure File Shredder";
            label1.MouseDown += Form1_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Image = Properties.Resources.icons8_close_48;
            btnClose.Location = new Point(453, 9);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(38, 39);
            btnClose.SizeMode = PictureBoxSizeMode.StretchImage;
            btnClose.TabIndex = 1;
            btnClose.TabStop = false;
            btnClose.Click += btnClose_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // listBoxFiles
            // 
            listBoxFiles.AllowDrop = true;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.Location = new Point(14, 55);
            listBoxFiles.Margin = new Padding(3, 4, 3, 4);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(473, 284);
            listBoxFiles.TabIndex = 2;
            listBoxFiles.DragDrop += listBox1_DragDrop;
            listBoxFiles.DragEnter += Form1_DragEnter;
            listBoxFiles.MouseDown += Form1_MouseDown;
            // 
            // btnStartDeleting
            // 
            btnStartDeleting.Location = new Point(302, 411);
            btnStartDeleting.Margin = new Padding(3, 4, 3, 4);
            btnStartDeleting.Name = "btnStartDeleting";
            btnStartDeleting.Size = new Size(185, 36);
            btnStartDeleting.TabIndex = 3;
            btnStartDeleting.Text = "Start Secure Clean-Up";
            btnStartDeleting.UseVisualStyleBackColor = true;
            btnStartDeleting.Click += btnStartDeleting_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Green;
            label2.Location = new Point(14, 407);
            label2.Name = "label2";
            label2.Size = new Size(253, 20);
            label2.TabIndex = 4;
            label2.Text = "Please drop files and start shredding.\r\n";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(14, 435);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(473, 13);
            progressBar.TabIndex = 5;
            // 
            // lblPasses
            // 
            lblPasses.AutoSize = true;
            lblPasses.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPasses.ForeColor = Color.Red;
            lblPasses.Location = new Point(16, 345);
            lblPasses.Name = "lblPasses";
            lblPasses.Size = new Size(91, 20);
            lblPasses.TabIndex = 7;
            lblPasses.Text = "PASSES  -  3";
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(14, 365);
            trackBar1.Margin = new Padding(3, 4, 3, 4);
            trackBar1.Maximum = 30;
            trackBar1.Minimum = 1;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(473, 56);
            trackBar1.TabIndex = 8;
            trackBar1.Value = 3;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // Mainmenu
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(501, 463);
            Controls.Add(lblPasses);
            Controls.Add(btnStartDeleting);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(trackBar1);
            Controls.Add(listBoxFiles);
            Controls.Add(btnClose);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Mainmenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mainmenu";
            DragDrop += listBox1_DragDrop;
            DragEnter += Form1_DragEnter;
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox btnClose;
        private FileSystemWatcher fileSystemWatcher1;
        private Label label2;
        private Button btnStartDeleting;
        private ListBox listBoxFiles;
        private ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private Label lblPasses;
        private TrackBar trackBar1;
    }
}