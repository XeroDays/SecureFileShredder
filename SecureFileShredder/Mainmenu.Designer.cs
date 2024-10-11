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
            cmbPasses = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            cmbBufferSize = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(6, 6);
            label1.Name = "label1";
            label1.Size = new Size(207, 30);
            label1.TabIndex = 0;
            label1.Text = "Secure File Shredder";
            label1.MouseDown += Form1_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Image = Properties.Resources.icons8_close_48;
            btnClose.Location = new Point(464, 7);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 29);
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
            listBoxFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.ItemHeight = 15;
            listBoxFiles.Location = new Point(10, 40);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(484, 214);
            listBoxFiles.TabIndex = 2;
            listBoxFiles.DragDrop += listBox1_DragDrop;
            listBoxFiles.DragEnter += Form1_DragEnter;
            listBoxFiles.MouseDown += Form1_MouseDown;
            // 
            // btnStartDeleting
            // 
            btnStartDeleting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStartDeleting.BackColor = Color.Red;
            btnStartDeleting.FlatAppearance.BorderColor = Color.Maroon;
            btnStartDeleting.FlatStyle = FlatStyle.Flat;
            btnStartDeleting.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartDeleting.ForeColor = Color.White;
            btnStartDeleting.Location = new Point(325, 279);
            btnStartDeleting.Name = "btnStartDeleting";
            btnStartDeleting.Size = new Size(169, 34);
            btnStartDeleting.TabIndex = 3;
            btnStartDeleting.Text = "Start Shredding to bits";
            btnStartDeleting.UseVisualStyleBackColor = false;
            btnStartDeleting.Click += btnStartDeleting_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(294, 257);
            label2.Name = "label2";
            label2.Size = new Size(200, 15);
            label2.TabIndex = 4;
            label2.Text = "Please drop files and start shredding.\r\n";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 326);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(479, 10);
            progressBar.TabIndex = 5;
            // 
            // cmbPasses
            // 
            cmbPasses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPasses.FormattingEnabled = true;
            cmbPasses.Location = new Point(95, 262);
            cmbPasses.Name = "cmbPasses";
            cmbPasses.Size = new Size(154, 23);
            cmbPasses.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Candara", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(11, 265);
            label3.Name = "label3";
            label3.Size = new Size(82, 18);
            label3.TabIndex = 7;
            label3.Text = "Total Passes";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Candara", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(11, 291);
            label4.Name = "label4";
            label4.Size = new Size(76, 18);
            label4.TabIndex = 9;
            label4.Text = "Buffer Size";
            // 
            // cmbBufferSize
            // 
            cmbBufferSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBufferSize.FormattingEnabled = true;
            cmbBufferSize.Location = new Point(95, 289);
            cmbBufferSize.Name = "cmbBufferSize";
            cmbBufferSize.Size = new Size(154, 23);
            cmbBufferSize.TabIndex = 8;
            // 
            // Mainmenu
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(504, 348);
            Controls.Add(label4);
            Controls.Add(cmbBufferSize);
            Controls.Add(label3);
            Controls.Add(cmbPasses);
            Controls.Add(btnStartDeleting);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(listBoxFiles);
            Controls.Add(btnClose);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Mainmenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mainmenu";
            DragDrop += listBox1_DragDrop;
            DragEnter += Form1_DragEnter;
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
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
        private ComboBox cmbPasses;
        private Label label3;
        private Label label4;
        private ComboBox cmbBufferSize;
    }
}