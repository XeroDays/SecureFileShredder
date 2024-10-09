﻿namespace SecureFileShredder
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
            label1 = new Label();
            btnClose = new PictureBox();
            fileSystemWatcher1 = new FileSystemWatcher();
            listBoxFiles = new ListBox();
            btnStartDeleting = new Button();
            label2 = new Label();
            progressBar = new ProgressBar();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            numberPasses = new NumericUpDown();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numberPasses).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(9, 9);
            label1.Name = "label1";
            label1.Size = new Size(189, 30);
            label1.TabIndex = 0;
            label1.Text = "Secure File Deleter";
            label1.MouseDown += Form1_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Image = Properties.Resources.icons8_close_48;
            btnClose.Location = new Point(396, 12);
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
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.ItemHeight = 15;
            listBoxFiles.Location = new Point(12, 58);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(414, 169);
            listBoxFiles.TabIndex = 2;
            listBoxFiles.DragDrop += listBox1_DragDrop;
            listBoxFiles.DragEnter += Form1_DragEnter;
            listBoxFiles.MouseDown += Form1_MouseDown;
            // 
            // btnStartDeleting
            // 
            btnStartDeleting.Location = new Point(264, 229);
            btnStartDeleting.Name = "btnStartDeleting";
            btnStartDeleting.Size = new Size(162, 27);
            btnStartDeleting.TabIndex = 3;
            btnStartDeleting.Text = "Start Secure Clean-Up";
            btnStartDeleting.UseVisualStyleBackColor = true;
            btnStartDeleting.Click += btnStartDeleting_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Green;
            label2.Location = new Point(12, 229);
            label2.Name = "label2";
            label2.Size = new Size(200, 15);
            label2.TabIndex = 4;
            label2.Text = "Please drop files and start shredding.\r\n";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 247);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(414, 10);
            progressBar.TabIndex = 5;
            // 
            // numberPasses
            // 
            numberPasses.Location = new Point(348, 12);
            numberPasses.Name = "numberPasses";
            numberPasses.Size = new Size(42, 23);
            numberPasses.TabIndex = 6;
            numberPasses.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(296, 15);
            label3.Name = "label3";
            label3.Size = new Size(48, 15);
            label3.TabIndex = 7;
            label3.Text = "PASSES";
            // 
            // Mainmenu
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(438, 268);
            Controls.Add(label3);
            Controls.Add(numberPasses);
            Controls.Add(btnStartDeleting);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(listBoxFiles);
            Controls.Add(btnClose);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Mainmenu";
            Text = "Mainmenu";
            DragDrop += listBox1_DragDrop;
            DragEnter += Form1_DragEnter;
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numberPasses).EndInit();
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
        private Label label3;
        private NumericUpDown numberPasses;
    }
}