namespace SecureFileShredder
{
    partial class SettingsForm
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
            profiles = new ComboBox();
            btnSave = new Button();
            profileName = new TextBox();
            btnDelete = new Button();
            lblPasses = new Label();
            passes = new ComboBox();
            lblBuffer = new Label();
            buffers = new ComboBox();
            rename = new CheckBox();
            timestamps = new CheckBox();
            verify = new CheckBox();
            streams = new CheckBox();
            dark = new CheckBox();
            btnApply = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.Location = new Point(18, 21);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(137, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Shredding profiles";
            // 
            // profiles
            // 
            profiles.DropDownStyle = ComboBoxStyle.DropDownList;
            profiles.Location = new Point(18, 59);
            profiles.Margin = new Padding(3, 4, 3, 4);
            profiles.Name = "profiles";
            profiles.Size = new Size(319, 28);
            profiles.TabIndex = 1;
            profiles.SelectedIndexChanged += profiles_SelectedIndexChanged;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(347, 56);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(160, 35);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save profile";
            btnSave.Click += btnSave_Click;
            // 
            // profileName
            // 
            profileName.Location = new Point(18, 104);
            profileName.Margin = new Padding(3, 4, 3, 4);
            profileName.Name = "profileName";
            profileName.PlaceholderText = "Profile name";
            profileName.Size = new Size(319, 27);
            profileName.TabIndex = 3;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(347, 101);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(160, 34);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete profile";
            btnDelete.Click += btnDelete_Click;
            // 
            // lblPasses
            // 
            lblPasses.AutoSize = true;
            lblPasses.Location = new Point(18, 147);
            lblPasses.Name = "lblPasses";
            lblPasses.Size = new Size(50, 20);
            lblPasses.TabIndex = 5;
            lblPasses.Text = "Passes";
            // 
            // passes
            // 
            passes.DropDownStyle = ComboBoxStyle.DropDownList;
            passes.Location = new Point(18, 173);
            passes.Margin = new Padding(3, 4, 3, 4);
            passes.Name = "passes";
            passes.Size = new Size(489, 28);
            passes.TabIndex = 6;
            // 
            // lblBuffer
            // 
            lblBuffer.AutoSize = true;
            lblBuffer.Location = new Point(18, 221);
            lblBuffer.Name = "lblBuffer";
            lblBuffer.Size = new Size(49, 20);
            lblBuffer.TabIndex = 7;
            lblBuffer.Text = "Buffer";
            // 
            // buffers
            // 
            buffers.DropDownStyle = ComboBoxStyle.DropDownList;
            buffers.Location = new Point(18, 248);
            buffers.Margin = new Padding(3, 4, 3, 4);
            buffers.Name = "buffers";
            buffers.Size = new Size(489, 28);
            buffers.TabIndex = 8;
            // 
            // rename
            // 
            rename.AutoSize = true;
            rename.Checked = true;
            rename.CheckState = CheckState.Checked;
            rename.Location = new Point(18, 307);
            rename.Margin = new Padding(3, 4, 3, 4);
            rename.Name = "rename";
            rename.Size = new Size(179, 24);
            rename.TabIndex = 9;
            rename.Text = "Rename before delete";
            // 
            // timestamps
            // 
            timestamps.AutoSize = true;
            timestamps.Checked = true;
            timestamps.CheckState = CheckState.Checked;
            timestamps.Location = new Point(18, 341);
            timestamps.Margin = new Padding(3, 4, 3, 4);
            timestamps.Name = "timestamps";
            timestamps.Size = new Size(187, 24);
            timestamps.TabIndex = 10;
            timestamps.Text = "Randomize timestamps";
            // 
            // verify
            // 
            verify.AutoSize = true;
            verify.Location = new Point(18, 376);
            verify.Margin = new Padding(3, 4, 3, 4);
            verify.Name = "verify";
            verify.Size = new Size(128, 24);
            verify.TabIndex = 11;
            verify.Text = "Verify last pass";
            // 
            // streams
            // 
            streams.AutoSize = true;
            streams.Checked = true;
            streams.CheckState = CheckState.Checked;
            streams.Location = new Point(18, 411);
            streams.Margin = new Padding(3, 4, 3, 4);
            streams.Name = "streams";
            streams.Size = new Size(218, 24);
            streams.TabIndex = 12;
            streams.Text = "Wipe alternate data streams";
            // 
            // dark
            // 
            dark.AutoSize = true;
            dark.Location = new Point(18, 445);
            dark.Margin = new Padding(3, 4, 3, 4);
            dark.Name = "dark";
            dark.Size = new Size(108, 24);
            dark.TabIndex = 13;
            dark.Text = "Dark theme";
            // 
            // btnApply
            // 
            btnApply.Location = new Point(240, 509);
            btnApply.Margin = new Padding(3, 4, 3, 4);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(149, 33);
            btnApply.TabIndex = 14;
            btnApply.Text = "Apply and close";
            btnApply.Click += btnApply_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(398, 509);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 33);
            btnCancel.TabIndex = 15;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(526, 562);
            Controls.Add(btnCancel);
            Controls.Add(btnApply);
            Controls.Add(dark);
            Controls.Add(streams);
            Controls.Add(verify);
            Controls.Add(timestamps);
            Controls.Add(rename);
            Controls.Add(buffers);
            Controls.Add(lblBuffer);
            Controls.Add(passes);
            Controls.Add(lblPasses);
            Controls.Add(btnDelete);
            Controls.Add(profileName);
            Controls.Add(btnSave);
            Controls.Add(profiles);
            Controls.Add(lblTitle);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private ComboBox profiles;
        private Button btnSave;
        private TextBox profileName;
        private Button btnDelete;
        private Label lblPasses;
        private ComboBox passes;
        private Label lblBuffer;
        private ComboBox buffers;
        private CheckBox rename;
        private CheckBox timestamps;
        private CheckBox verify;
        private CheckBox streams;
        private CheckBox dark;
        private Button btnApply;
        private Button btnCancel;
    }
}
