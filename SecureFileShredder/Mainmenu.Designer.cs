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
            pictureBox1 = new PictureBox();
            btnInfo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnInfo).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Copperplate Gothic Bold", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(47, 13);
            label1.Name = "label1";
            label1.Size = new Size(358, 31);
            label1.TabIndex = 0;
            label1.Text = "Secure File Shredder";
            label1.MouseDown += Form1_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Image = Properties.Resources.icons8_close_50;
            btnClose.Location = new Point(579, 11);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 30);
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
            listBoxFiles.Location = new Point(11, 60);
            listBoxFiles.Margin = new Padding(3, 4, 3, 4);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(596, 284);
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
            btnStartDeleting.Font = new Font("Bahnschrift SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartDeleting.ForeColor = Color.White;
            btnStartDeleting.Location = new Point(401, 376);
            btnStartDeleting.Margin = new Padding(3, 4, 3, 4);
            btnStartDeleting.Name = "btnStartDeleting";
            btnStartDeleting.Size = new Size(206, 45);
            btnStartDeleting.TabIndex = 3;
            btnStartDeleting.Text = "Start Shredding to bits";
            btnStartDeleting.UseVisualStyleBackColor = false;
            btnStartDeleting.Click += btnStartDeleting_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(354, 348);
            label2.Name = "label2";
            label2.Size = new Size(253, 20);
            label2.TabIndex = 4;
            label2.Text = "Please drop files and start shredding.\r\n";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(14, 435);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(590, 13);
            progressBar.TabIndex = 5;
            // 
            // cmbPasses
            // 
            cmbPasses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPasses.FormattingEnabled = true;
            cmbPasses.Location = new Point(129, 359);
            cmbPasses.Margin = new Padding(3, 4, 3, 4);
            cmbPasses.Name = "cmbPasses";
            cmbPasses.Size = new Size(175, 28);
            cmbPasses.TabIndex = 6;
            // 
            // label3
            // 
            label3.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(2, 360);
            label3.Name = "label3";
            label3.Size = new Size(123, 23);
            label3.TabIndex = 7;
            label3.Text = "Total Passes";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(14, 396);
            label4.Name = "label4";
            label4.Size = new Size(111, 25);
            label4.TabIndex = 9;
            label4.Text = "Buffer Size";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbBufferSize
            // 
            cmbBufferSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBufferSize.FormattingEnabled = true;
            cmbBufferSize.Location = new Point(129, 395);
            cmbBufferSize.Margin = new Padding(3, 4, 3, 4);
            cmbBufferSize.Name = "cmbBufferSize";
            cmbBufferSize.Size = new Size(175, 28);
            cmbBufferSize.TabIndex = 8;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.LogoPng;
            pictureBox1.Location = new Point(8, 2);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(42, 48);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += Form1_MouseDown;
            // 
            // btnInfo
            // 
            btnInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInfo.Image = Properties.Resources.icons8_information_100;
            btnInfo.Location = new Point(543, 11);
            btnInfo.Margin = new Padding(3, 4, 3, 4);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new Size(30, 30);
            btnInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            btnInfo.TabIndex = 11;
            btnInfo.TabStop = false;
            // 
            // Mainmenu
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(619, 464);
            Controls.Add(btnInfo);
            Controls.Add(pictureBox1);
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
            Margin = new Padding(3, 4, 3, 4);
            Name = "Mainmenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mainmenu";
            DragDrop += listBox1_DragDrop;
            DragEnter += Form1_DragEnter;
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnInfo).EndInit();
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
        private PictureBox pictureBox1;
        private PictureBox btnInfo;
    }
}