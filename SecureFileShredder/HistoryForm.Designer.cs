namespace SecureFileShredder
{
    partial class HistoryForm
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
            list = new ListView();
            bar = new Panel();
            btnClear = new Button();
            btnClose = new Button();
            bar.SuspendLayout();
            SuspendLayout();
            // 
            // list
            // 
            list.Dock = DockStyle.Fill;
            list.FullRowSelect = true;
            list.GridLines = true;
            list.Name = "list";
            list.TabIndex = 0;
            list.View = View.Details;
            list.Columns.Add("When", 150);
            list.Columns.Add("Result", 80);
            list.Columns.Add("Passes", 60);
            list.Columns.Add("Size", 80);
            list.Columns.Add("Path", 320);
            list.Columns.Add("Detail", 160);
            // 
            // bar
            // 
            bar.Controls.Add(btnClear);
            bar.Controls.Add(btnClose);
            bar.Dock = DockStyle.Bottom;
            bar.Location = new Point(0, 412);
            bar.Name = "bar";
            bar.Size = new Size(860, 48);
            bar.TabIndex = 1;
            // 
            // btnClear
            // 
            btnClear.Dock = DockStyle.Right;
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 48);
            btnClear.TabIndex = 0;
            btnClear.Text = "Clear log";
            btnClear.Click += btnClear_Click;
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Right;
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 48);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // HistoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 460);
            Controls.Add(list);
            Controls.Add(bar);
            Font = new Font("Segoe UI", 9F);
            Name = "HistoryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Shred history";
            bar.ResumeLayout(false);
            ResumeLayout(false);
        }

        private ListView list;
        private Panel bar;
        private Button btnClear;
        private Button btnClose;
    }
}
