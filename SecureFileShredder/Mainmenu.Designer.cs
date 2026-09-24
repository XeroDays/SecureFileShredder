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
            listBoxFiles = new SecureFileShredder.Controls.FileQueueList();
            btnStartDeleting = new Button();
            cmbPasses = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            cmbBufferSize = new ComboBox();
            pictureBox1 = new PictureBox();
            btnInfo = new PictureBox();
            btnMinimize = new Label();
            btnAddFiles = new Button();
            btnAddFolder = new Button();
            btnRemove = new Button();
            btnClear = new Button();
            btnSettings = new Button();
            btnHistory = new Button();
            btnFreeSpace = new Button();
            lblSummary = new Label();
            lblStatus = new Label();
            lblMetrics = new Label();
            lblResult = new Label();
            shredBar = new SecureFileShredder.Controls.ShredProgressBar();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
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
            btnClose.Location = new Point(858, 11);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 30);
            btnClose.SizeMode = PictureBoxSizeMode.StretchImage;
            btnClose.TabIndex = 1;
            btnClose.TabStop = false;
            btnClose.Click += btnClose_Click;
            // 
            // listBoxFiles
            // 
            listBoxFiles.ActiveBack = Color.FromArgb(255, 228, 225);
            listBoxFiles.AllowDrop = true;
            listBoxFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxFiles.BorderStyle = BorderStyle.FixedSingle;
            listBoxFiles.DragActive = false;
            listBoxFiles.DragBack = Color.FromArgb(255, 236, 232);
            listBoxFiles.DragBorder = Color.Maroon;
            listBoxFiles.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.IntegralHeight = false;
            listBoxFiles.ItemHeight = 22;
            listBoxFiles.Location = new Point(12, 96);
            listBoxFiles.Margin = new Padding(3, 4, 3, 4);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.NormalBack = Color.White;
            listBoxFiles.NormalText = Color.Black;
            listBoxFiles.Size = new Size(876, 348);
            listBoxFiles.TabIndex = 2;
            listBoxFiles.DragDrop += listBox1_DragDrop;
            listBoxFiles.DragEnter += Form1_DragEnter;
            // 
            // btnStartDeleting
            // 
            btnStartDeleting.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnStartDeleting.BackColor = Color.Red;
            btnStartDeleting.FlatAppearance.BorderColor = Color.Maroon;
            btnStartDeleting.FlatStyle = FlatStyle.Flat;
            btnStartDeleting.Font = new Font("Bahnschrift SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartDeleting.ForeColor = Color.White;
            btnStartDeleting.Location = new Point(658, 478);
            btnStartDeleting.Margin = new Padding(3, 4, 3, 4);
            btnStartDeleting.Name = "btnStartDeleting";
            btnStartDeleting.Size = new Size(230, 64);
            btnStartDeleting.TabIndex = 3;
            btnStartDeleting.Text = "Start Shredding to bits";
            btnStartDeleting.UseVisualStyleBackColor = false;
            btnStartDeleting.Click += btnStartDeleting_Click;
            // 
            // cmbPasses
            // 
            cmbPasses.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cmbPasses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPasses.FormattingEnabled = true;
            cmbPasses.Location = new Point(126, 478);
            cmbPasses.Margin = new Padding(3, 4, 3, 4);
            cmbPasses.Name = "cmbPasses";
            cmbPasses.Size = new Size(230, 28);
            cmbPasses.TabIndex = 6;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 478);
            label3.Name = "label3";
            label3.Size = new Size(110, 28);
            label3.TabIndex = 7;
            label3.Text = "Total Passes";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(12, 514);
            label4.Name = "label4";
            label4.Size = new Size(110, 28);
            label4.TabIndex = 9;
            label4.Text = "Buffer Size";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbBufferSize
            // 
            cmbBufferSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cmbBufferSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBufferSize.FormattingEnabled = true;
            cmbBufferSize.Location = new Point(126, 514);
            cmbBufferSize.Margin = new Padding(3, 4, 3, 4);
            cmbBufferSize.Name = "cmbBufferSize";
            cmbBufferSize.Size = new Size(230, 28);
            cmbBufferSize.TabIndex = 8;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.LogoPng;
            pictureBox1.Location = new Point(8, 2);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(42, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += Form1_MouseDown;
            // 
            // btnInfo
            // 
            btnInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInfo.Image = Properties.Resources.icons8_information_100;
            btnInfo.Location = new Point(786, 11);
            btnInfo.Margin = new Padding(3, 4, 3, 4);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new Size(30, 30);
            btnInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            btnInfo.TabIndex = 11;
            btnInfo.TabStop = false;
            btnInfo.Click += btnInfo_Click;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.Cursor = Cursors.Hand;
            btnMinimize.Font = new Font("Bahnschrift SemiBold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMinimize.ForeColor = Color.Maroon;
            btnMinimize.Location = new Point(822, 11);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(30, 30);
            btnMinimize.TabIndex = 12;
            btnMinimize.Text = "–";
            btnMinimize.TextAlign = ContentAlignment.MiddleCenter;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // btnAddFiles
            // 
            btnAddFiles.BackColor = Color.White;
            btnAddFiles.FlatAppearance.BorderColor = Color.Maroon;
            btnAddFiles.FlatStyle = FlatStyle.Flat;
            btnAddFiles.Location = new Point(12, 56);
            btnAddFiles.Name = "btnAddFiles";
            btnAddFiles.Size = new Size(100, 30);
            btnAddFiles.TabIndex = 13;
            btnAddFiles.Text = "Add files";
            btnAddFiles.UseVisualStyleBackColor = false;
            // 
            // btnAddFolder
            // 
            btnAddFolder.BackColor = Color.White;
            btnAddFolder.FlatAppearance.BorderColor = Color.Maroon;
            btnAddFolder.FlatStyle = FlatStyle.Flat;
            btnAddFolder.Location = new Point(118, 56);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Size = new Size(104, 30);
            btnAddFolder.TabIndex = 14;
            btnAddFolder.Text = "Add folder";
            btnAddFolder.UseVisualStyleBackColor = false;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = Color.White;
            btnRemove.FlatAppearance.BorderColor = Color.Maroon;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Location = new Point(228, 56);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(84, 30);
            btnRemove.TabIndex = 15;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.White;
            btnClear.FlatAppearance.BorderColor = Color.Maroon;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Location = new Point(318, 56);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(72, 30);
            btnClear.TabIndex = 16;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.BackColor = Color.White;
            btnSettings.FlatAppearance.BorderColor = Color.Maroon;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(584, 56);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(88, 30);
            btnSettings.TabIndex = 17;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnHistory
            // 
            btnHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHistory.BackColor = Color.White;
            btnHistory.FlatAppearance.BorderColor = Color.Maroon;
            btnHistory.FlatStyle = FlatStyle.Flat;
            btnHistory.Location = new Point(678, 56);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(84, 30);
            btnHistory.TabIndex = 18;
            btnHistory.Text = "History";
            btnHistory.UseVisualStyleBackColor = false;
            // 
            // btnFreeSpace
            // 
            btnFreeSpace.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFreeSpace.BackColor = Color.White;
            btnFreeSpace.FlatAppearance.BorderColor = Color.Maroon;
            btnFreeSpace.FlatStyle = FlatStyle.Flat;
            btnFreeSpace.Location = new Point(768, 56);
            btnFreeSpace.Name = "btnFreeSpace";
            btnFreeSpace.Size = new Size(120, 30);
            btnFreeSpace.TabIndex = 19;
            btnFreeSpace.Text = "Free space";
            btnFreeSpace.UseVisualStyleBackColor = false;
            // 
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSummary.ForeColor = Color.DimGray;
            lblSummary.Location = new Point(12, 452);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(220, 22);
            lblSummary.TabIndex = 20;
            lblSummary.Text = "0 files · 0 B";
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.ForeColor = Color.DimGray;
            lblStatus.Location = new Point(488, 452);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(400, 22);
            lblStatus.TabIndex = 21;
            lblStatus.Text = "Drop files here or use Add files.";
            lblStatus.TextAlign = ContentAlignment.TopRight;
            // 
            // lblMetrics
            // 
            lblMetrics.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblMetrics.ForeColor = Color.DimGray;
            lblMetrics.Location = new Point(12, 572);
            lblMetrics.Name = "lblMetrics";
            lblMetrics.Size = new Size(876, 20);
            lblMetrics.TabIndex = 22;
            // 
            // lblResult
            // 
            lblResult.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResult.ForeColor = Color.DimGray;
            lblResult.Location = new Point(12, 594);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(876, 40);
            lblResult.TabIndex = 23;
            // 
            // shredBar
            // 
            shredBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            shredBar.BarColor = Color.Maroon;
            shredBar.Location = new Point(12, 554);
            shredBar.Maximum = 10000;
            shredBar.Name = "shredBar";
            shredBar.Size = new Size(876, 14);
            shredBar.TabIndex = 24;
            shredBar.TrackColor = Color.Silver;
            shredBar.Visible = false;
            // 
            // Mainmenu
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(900, 640);
            Controls.Add(shredBar);
            Controls.Add(lblResult);
            Controls.Add(lblMetrics);
            Controls.Add(lblStatus);
            Controls.Add(lblSummary);
            Controls.Add(btnFreeSpace);
            Controls.Add(btnHistory);
            Controls.Add(btnSettings);
            Controls.Add(btnClear);
            Controls.Add(btnRemove);
            Controls.Add(btnAddFolder);
            Controls.Add(btnAddFiles);
            Controls.Add(btnMinimize);
            Controls.Add(btnInfo);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(cmbBufferSize);
            Controls.Add(label3);
            Controls.Add(cmbPasses);
            Controls.Add(btnStartDeleting);
            Controls.Add(listBoxFiles);
            Controls.Add(btnClose);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(880, 600);
            Name = "Mainmenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mainmenu";
            DragDrop += listBox1_DragDrop;
            DragEnter += Form1_DragEnter;
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox btnClose;
        private Button btnStartDeleting;
        private Controls.FileQueueList listBoxFiles;
        private ComboBox cmbPasses;
        private Label label3;
        private Label label4;
        private ComboBox cmbBufferSize;
        private PictureBox pictureBox1;
        private PictureBox btnInfo;
        private Label btnMinimize;
        private Button btnAddFiles;
        private Button btnAddFolder;
        private Button btnRemove;
        private Button btnClear;
        private Button btnSettings;
        private Button btnHistory;
        private Button btnFreeSpace;
        private Label lblSummary;
        private Label lblStatus;
        private Label lblMetrics;
        private Label lblResult;
        private Controls.ShredProgressBar shredBar;
    }
}
