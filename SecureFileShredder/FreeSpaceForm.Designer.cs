namespace SecureFileShredder
{
    partial class FreeSpaceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            drives = new ComboBox();
            details = new Label();
            bar = new Controls.ShredProgressBar();
            status = new Label();
            btnStart = new Button();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(16, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(488, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Fill free space, overwrite it, then delete the temporary file. 256 MB is left free.";
            // 
            // drives
            // 
            drives.DropDownStyle = ComboBoxStyle.DropDownList;
            drives.Location = new Point(16, 68);
            drives.Name = "drives";
            drives.Size = new Size(488, 23);
            drives.TabIndex = 1;
            drives.SelectedIndexChanged += drives_SelectedIndexChanged;
            // 
            // details
            // 
            details.Location = new Point(16, 104);
            details.Name = "details";
            details.Size = new Size(488, 40);
            details.TabIndex = 2;
            // 
            // bar
            // 
            bar.Location = new Point(16, 156);
            bar.Name = "bar";
            bar.Size = new Size(488, 16);
            bar.TabIndex = 3;
            // 
            // status
            // 
            status.Location = new Point(16, 180);
            status.Name = "status";
            status.Size = new Size(488, 36);
            status.TabIndex = 4;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(250, 230);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(140, 32);
            btnStart.TabIndex = 5;
            btnStart.Text = "Wipe free space";
            btnStart.Click += btnStart_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(400, 230);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(104, 32);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // FreeSpaceForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 280);
            Controls.Add(btnClose);
            Controls.Add(btnStart);
            Controls.Add(status);
            Controls.Add(bar);
            Controls.Add(details);
            Controls.Add(drives);
            Controls.Add(lblTitle);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FreeSpaceForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Free space wipe";
            FormClosing += FreeSpaceForm_FormClosing;
            ResumeLayout(false);
        }

        private Label lblTitle;
        private ComboBox drives;
        private Label details;
        private Controls.ShredProgressBar bar;
        private Label status;
        private Button btnStart;
        private Button btnClose;
    }
}
