namespace SecureFileShredder
{
    partial class About
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            btnClose = new PictureBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnLink = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.LogoPng;
            pictureBox1.Location = new Point(12, 13);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(98, 108);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += Form1_MouseDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Copperplate Gothic Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(112, 51);
            label1.Name = "label1";
            label1.Size = new Size(258, 23);
            label1.TabIndex = 11;
            label1.Text = "Secure File Shredder";
            label1.MouseDown += Form1_MouseDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Copperplate Gothic Bold", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(109, 15);
            label2.Name = "label2";
            label2.Size = new Size(134, 38);
            label2.TabIndex = 13;
            label2.Text = "About";
            label2.MouseDown += Form1_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Image = Properties.Resources.icons8_close_50;
            btnClose.Location = new Point(407, 13);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 30);
            btnClose.SizeMode = PictureBoxSizeMode.StretchImage;
            btnClose.TabIndex = 14;
            btnClose.TabStop = false;
            btnClose.Click += btnClose_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.DimGray;
            label3.Location = new Point(113, 74);
            label3.Name = "label3";
            label3.Size = new Size(88, 21);
            label3.TabIndex = 15;
            label3.Text = "Version 1.2";
            label3.MouseDown += Form1_MouseDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Consolas", 10F);
            label4.ForeColor = Color.DimGray;
            label4.Location = new Point(29, 134);
            label4.Name = "label4";
            label4.Size = new Size(396, 100);
            label4.TabIndex = 16;
            label4.Text = "Securely overwrite files with random data, \r\nthen delete them from disk.\r\n\r\nPass presets change overwrite count only.\r\nAll passes use cryptographic random bytes.";
            label4.MouseDown += Form1_MouseDown;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(29, 254);
            label5.Name = "label5";
            label5.Size = new Size(230, 21);
            label5.TabIndex = 17;
            label5.Text = "© 2026 Softasium · MIT License";
            label5.MouseDown += Form1_MouseDown;
            // 
            // btnLink
            // 
            btnLink.AutoSize = true;
            btnLink.Cursor = Cursors.Hand;
            btnLink.Font = new Font("Calibri", 10.2F, FontStyle.Underline, GraphicsUnit.Point, 0);
            btnLink.ForeColor = Color.Blue;
            btnLink.Location = new Point(29, 277);
            btnLink.Name = "btnLink";
            btnLink.Size = new Size(151, 21);
            btnLink.TabIndex = 18;
            btnLink.Text = "www.softasium.com";
            btnLink.Click += btnLink_Click;
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(449, 340);
            Controls.Add(btnLink);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(btnClose);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "About";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "About";
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private PictureBox btnClose;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label btnLink;
    }
}