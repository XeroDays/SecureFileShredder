using System.Runtime.InteropServices;
using SecureFileShredder.Controls;
using SecureFileShredder.Models;
using SecureFileShredder.Services;
using SecureFileShredder.Theming;

namespace SecureFileShredder
{
    public partial class Mainmenu : Form
    {
        private const int WmNcHitTest = 0x84;
        private const int HtClient = 1;
        private const int HtCaption = 2;
        private const int HtLeft = 10;
        private const int HtRight = 11;
        private const int HtTop = 12;
        private const int HtTopLeft = 13;
        private const int HtTopRight = 14;
        private const int HtBottom = 15;
        private const int HtBottomLeft = 16;
        private const int HtBottomRight = 17;
        private const int WmNclButtonDown = 0xA1;

        private readonly ShredQueue queue = new();
        private readonly ToolTip toolTip = new();
        private AppSettings settings = new();
        private ThemePalette theme = ThemePalette.LightTheme();
        private CancellationTokenSource? shredCts;
        private bool shredding;
        private bool suppressSettingsSave;
        private NotifyIcon? notifyIcon;
        private Icon? trayProgressIcon;
        private Icon? trayBaseIcon;
        private bool isMinimizedToTray;
        private int lastTrayPercent = -1;
        private System.Windows.Forms.Timer fadeTimer = null!;
        private System.Windows.Forms.Timer pulseTimer = null!;
        private System.Windows.Forms.Timer rowFadeTimer = null!;
        private System.Windows.Forms.Timer dashTimer = null!;
        private double pulsePhase;
        private DateTime speedStamp = DateTime.UtcNow;
        private long speedBytes;
        private double bytesPerSecond;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public Mainmenu(string[]? args = null)
        {
            InitializeComponent();
            settings = AppSettingsStore.Load();
            WireUi();
            KeyPreview = true;
            KeyDown += Mainmenu_KeyDown;
            FormClosing += Mainmenu_FormClosing;
            SetupCombos();
            ApplySavedWindow();
            ApplyTheme();
            InitializeNotifyIcon();
            InitializeTimers();
            if (args is { Length: > 0 })
            {
                AddPaths(args);
            }

            UpdateSummary();
            Opacity = 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            fadeTimer.Start();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            SaveWindowBounds();
            AppSettingsStore.Save(settings);
        }

        protected override void WndProc(ref Message m)
        {
            const int wmCopyData = 0x004A;
            if (m.Msg == wmCopyData)
            {
                COPYDATASTRUCT data = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT))!;
                string? filePaths = Marshal.PtrToStringUni(data.lpData);
                if (!string.IsNullOrEmpty(filePaths))
                {
                    AddPaths(filePaths.Split('|'));
                }
            }
            else if (m.Msg == WmNcHitTest)
            {
                base.WndProc(ref m);
                if ((int)m.Result == HtClient)
                {
                    Point cursor = PointToClient(Cursor.Position);
                    int grip = Scale(8);
                    bool left = cursor.X <= grip;
                    bool right = cursor.X >= ClientSize.Width - grip;
                    bool top = cursor.Y <= grip;
                    bool bottom = cursor.Y >= ClientSize.Height - grip;
                    if (left && top) m.Result = (IntPtr)HtTopLeft;
                    else if (right && top) m.Result = (IntPtr)HtTopRight;
                    else if (left && bottom) m.Result = (IntPtr)HtBottomLeft;
                    else if (right && bottom) m.Result = (IntPtr)HtBottomRight;
                    else if (left) m.Result = (IntPtr)HtLeft;
                    else if (right) m.Result = (IntPtr)HtRight;
                    else if (top) m.Result = (IntPtr)HtTop;
                    else if (bottom) m.Result = (IntPtr)HtBottom;
                }

                return;
            }

            base.WndProc(ref m);
        }

        private int Scale(int pixels) => (int)(pixels * DeviceDpi / 96f);

        private void WireUi()
        {
            btnAddFiles.Click += (_, _) => BrowseFiles();
            btnAddFolder.Click += (_, _) => BrowseFolder();
            btnRemove.Click += (_, _) => RemoveSelected();
            btnClear.Click += (_, _) => ClearQueue();
            btnSettings.Click += (_, _) => OpenSettings();
            btnHistory.Click += (_, _) => OpenHistory();
            btnFreeSpace.Click += (_, _) => OpenFreeSpace();
            DragLeave += (_, _) => DragLeaveClient();
            listBoxFiles.DragLeave += (_, _) => DragLeaveClient();
        }

        private void SetupCombos()
        {
            suppressSettingsSave = true;
            cmbPasses.Items.Clear();
            cmbBufferSize.Items.Clear();
            foreach (ShredPreset preset in PresetCatalog.PassPresets)
            {
                cmbPasses.Items.Add(preset);
            }

            foreach (BufferOption option in PresetCatalog.Buffers)
            {
                cmbBufferSize.Items.Add(option);
            }

            cmbPasses.SelectedIndex = Clamp(settings.PassIndex, cmbPasses.Items.Count);
            cmbBufferSize.SelectedIndex = Clamp(settings.BufferIndex, cmbBufferSize.Items.Count);
            suppressSettingsSave = false;
            cmbPasses.SelectedIndexChanged += (_, _) => OnComboChanged();
            cmbBufferSize.SelectedIndexChanged += (_, _) => OnComboChanged();
            UpdateTips();
        }

        private void OnComboChanged()
        {
            UpdateTips();
            if (suppressSettingsSave)
            {
                return;
            }

            settings.PassIndex = cmbPasses.SelectedIndex;
            settings.BufferIndex = cmbBufferSize.SelectedIndex;
            AppSettingsStore.Save(settings);
        }

        private void UpdateTips()
        {
            if (cmbPasses.SelectedItem is ShredPreset preset)
            {
                toolTip.SetToolTip(cmbPasses, preset.Tooltip);
            }

            if (cmbBufferSize.SelectedItem is BufferOption buffer)
            {
                toolTip.SetToolTip(cmbBufferSize, PresetCatalog.BufferTooltip(buffer));
            }
        }

        private void ApplySavedWindow()
        {
            if (settings.WindowWidth < 400 || settings.WindowHeight < 300)
            {
                return;
            }

            var bounds = new Rectangle(settings.WindowX, settings.WindowY, settings.WindowWidth, settings.WindowHeight);
            bool visible = Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(bounds));
            if (!visible)
            {
                return;
            }

            StartPosition = FormStartPosition.Manual;
            Bounds = bounds;
        }

        private void SaveWindowBounds()
        {
            settings.WindowX = Left;
            settings.WindowY = Top;
            settings.WindowWidth = Width;
            settings.WindowHeight = Height;
        }

        private void ApplyTheme()
        {
            theme = ThemePalette.For(settings.DarkTheme);
            ThemePalette.Apply(this, theme);
            btnMinimize.ForeColor = theme.Accent;
            label1.ForeColor = theme.Accent;
        }

        private void InitializeTimers()
        {
            fadeTimer = new System.Windows.Forms.Timer { Interval = 16 };
            fadeTimer.Tick += (_, _) =>
            {
                Opacity = Math.Min(1, Opacity + 0.08);
                if (Opacity >= 1)
                {
                    fadeTimer.Stop();
                }
            };
            pulseTimer = new System.Windows.Forms.Timer { Interval = 50 };
            pulseTimer.Tick += (_, _) =>
            {
                if (shredding || queue.Files.Count == 0)
                {
                    btnStartDeleting.BackColor = theme.ButtonBack;
                    return;
                }

                pulsePhase += 0.18;
                double wave = (Math.Sin(pulsePhase) + 1) / 2;
                btnStartDeleting.BackColor = ThemePalette.Blend(theme.ButtonBack, theme.ButtonPulse, wave);
            };
            rowFadeTimer = new System.Windows.Forms.Timer { Interval = 30 };
            rowFadeTimer.Tick += (_, _) =>
            {
                foreach (string path in listBoxFiles.TickFade())
                {
                    queue.DropVisual(path);
                    RemoveListPath(path);
                }

                if (!listBoxFiles.HasFading)
                {
                    rowFadeTimer.Stop();
                }
            };
            dashTimer = new System.Windows.Forms.Timer { Interval = 80 };
            dashTimer.Tick += (_, _) => listBoxFiles.AdvanceDash();
            pulseTimer.Start();
        }

        private void InitializeNotifyIcon()
        {
            var trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Restore", null, (_, _) => RestoreFromTray());
            trayBaseIcon = Icon ?? SystemIcons.Application;
            notifyIcon = new NotifyIcon
            {
                Icon = trayBaseIcon,
                Text = "Secure File Shredder",
                Visible = false,
                ContextMenuStrip = trayMenu
            };
            notifyIcon.DoubleClick += (_, _) => RestoreFromTray();
        }

        private void AddPaths(IEnumerable<string> paths)
        {
            int top = listBoxFiles.Items.Count == 0 ? 0 : listBoxFiles.TopIndex;
            QueueAddResult result = queue.Add(paths);
            foreach (QueueEntry entry in result.Added)
            {
                listBoxFiles.Items.Add(entry);
            }

            if (listBoxFiles.Items.Count > 0)
            {
                listBoxFiles.TopIndex = Math.Min(top, listBoxFiles.Items.Count - 1);
            }

            lblResult.Text = "";
            shredBar.Reset();
            shredBar.Visible = false;
            UpdateSummary();
            if (result.Unreadable > 0)
            {
                lblStatus.Text = "Some paths could not be read.";
            }
        }

        private void BrowseFiles()
        {
            using var dialog = new OpenFileDialog { Multiselect = true, Title = "Add files to shred" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                AddPaths(dialog.FileNames);
            }
        }

        private void BrowseFolder()
        {
            using var dialog = new FolderBrowserDialog { Description = "Add a folder to shred" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                AddPaths(new[] { dialog.SelectedPath });
            }
        }

        private void RemoveSelected()
        {
            if (shredding || listBoxFiles.SelectedItem is not QueueEntry entry)
            {
                return;
            }

            int top = listBoxFiles.TopIndex;
            foreach (string path in queue.Remove(entry))
            {
                RemoveListPath(path);
            }

            if (listBoxFiles.Items.Count > 0)
            {
                listBoxFiles.TopIndex = Math.Min(top, listBoxFiles.Items.Count - 1);
            }

            UpdateSummary();
        }

        private void ClearQueue()
        {
            if (shredding)
            {
                return;
            }

            queue.Clear();
            listBoxFiles.ResetStates();
            listBoxFiles.Items.Clear();
            lblResult.Text = "";
            UpdateSummary();
        }

        private void RemoveListPath(string path)
        {
            for (int index = listBoxFiles.Items.Count - 1; index >= 0; index--)
            {
                if (string.Equals(listBoxFiles.Items[index]?.ToString(), path, StringComparison.OrdinalIgnoreCase))
                {
                    listBoxFiles.Items.RemoveAt(index);
                }
            }
        }

        private void UpdateSummary()
        {
            int count = queue.Files.Count;
            string files = count == 1 ? "1 file" : count + " files";
            lblSummary.Text = files + " · " + ByteFormat.Format(queue.TotalBytes);
            if (!shredding && !listBoxFiles.DragActive)
            {
                lblStatus.Text = count == 0 ? "Drop files here or use Add files." : "Ready to shred.";
            }

            bool idle = !shredding;
            btnRemove.Enabled = idle;
            btnClear.Enabled = idle;
            btnAddFiles.Enabled = idle;
            btnAddFolder.Enabled = idle;
            btnFreeSpace.Enabled = idle;
            cmbPasses.Enabled = idle;
            cmbBufferSize.Enabled = idle;
            pulseTimer.Enabled = idle;
        }

        private async void btnStartDeleting_Click(object sender, EventArgs e)
        {
            if (shredding)
            {
                btnStartDeleting.Enabled = false;
                shredCts?.Cancel();
                return;
            }

            if (queue.Files.Count == 0)
            {
                MessageBox.Show(this, "No files found here. Are they already gone?", "No files to Shredd", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, "Are you sure you want to delete these files?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            List<string> snapshot = queue.SnapshotFiles();
            ShredJobOptions options = CurrentOptions();
            shredCts = new CancellationTokenSource();
            SetBusy(true);
            speedStamp = DateTime.UtcNow;
            speedBytes = 0;
            bytesPerSecond = 0;
            var session = new ShredSession();
            try
            {
                ShredBatchResult result = await session.RunAsync(snapshot, options, Publish, shredCts.Token);
                Finish(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                shredCts.Dispose();
                shredCts = null;
                SetBusy(false);
            }
        }

        private ShredJobOptions CurrentOptions()
        {
            var preset = cmbPasses.SelectedItem as ShredPreset ?? PresetCatalog.PassPresets[0];
            var buffer = cmbBufferSize.SelectedItem as BufferOption ?? PresetCatalog.Buffers[2];
            return new ShredJobOptions
            {
                Patterns = preset.Patterns,
                BufferBytes = buffer.Bytes,
                VerifyOverwrite = settings.VerifyOverwrite,
                WipeAlternateStreams = settings.WipeAlternateStreams,
                RenameBeforeDelete = settings.RenameBeforeDelete,
                RandomizeTimestamps = settings.RandomizeTimestamps,
                MaxDegreeOfParallelism = Math.Min(4, Math.Max(1, Environment.ProcessorCount))
            };
        }

        private void Publish(ShredProgressUpdate update)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            Action apply = () => ApplyUpdate(update);
            try
            {
                if (!InvokeRequired)
                {
                    apply();
                }
                else if (update.Kind == ShredNoticeKind.Progress)
                {
                    BeginInvoke(apply);
                }
                else
                {
                    Invoke(apply);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ApplyUpdate(ShredProgressUpdate update)
        {
            if (update.BytesTotal > 0)
            {
                shredBar.SetTarget((int)Math.Clamp(update.BytesCompleted * 10000d / update.BytesTotal, 0, 10000));
            }

            string name = Path.GetFileName(update.FilePath);
            if (update.Kind is ShredNoticeKind.Started or ShredNoticeKind.Progress)
            {
                listBoxFiles.ActivePaths.Add(update.FilePath);
                lblStatus.Text = name + " — pass " + Math.Max(1, update.PassNumber) + " of " + Math.Max(1, update.PassCount);
                UpdateSpeed(update);
            }
            else if (update.Kind == ShredNoticeKind.Completed)
            {
                queue.MarkShredded(update.FilePath);
                listBoxFiles.BeginFade(update.FilePath);
                rowFadeTimer.Start();
                UpdateSummary();
            }

            listBoxFiles.Invalidate();
            UpdateTray(update);
        }

        private void UpdateSpeed(ShredProgressUpdate update)
        {
            var now = DateTime.UtcNow;
            double seconds = (now - speedStamp).TotalSeconds;
            if (seconds >= 0.4)
            {
                double instant = (update.BytesCompleted - speedBytes) / seconds;
                bytesPerSecond = bytesPerSecond <= 0 ? instant : bytesPerSecond * 0.7 + instant * 0.3;
                speedStamp = now;
                speedBytes = update.BytesCompleted;
            }

            int percent = update.BytesTotal <= 0 ? 0 : (int)(update.BytesCompleted * 100 / update.BytesTotal);
            long remaining = Math.Max(0, update.BytesTotal - update.BytesCompleted);
            lblMetrics.Text = percent + "%    " + ByteFormat.Speed(bytesPerSecond) + "    " + ByteFormat.Eta(remaining, bytesPerSecond);
        }

        private void UpdateTray(ShredProgressUpdate update)
        {
            if (notifyIcon == null || !notifyIcon.Visible || update.BytesTotal <= 0)
            {
                return;
            }

            int percent = (int)Math.Clamp(update.BytesCompleted * 100 / update.BytesTotal, 0, 100);
            notifyIcon.Text = "Shredding: " + percent + "%";
            UpdateTrayProgressIcon(percent);
        }

        private void Finish(ShredBatchResult result)
        {
            RemoveEmptyRoots();
            string summary = result.Succeeded.Count + " files destroyed · " + ByteFormat.Format(result.BytesOverwritten)
                + " overwritten · " + result.Elapsed.ToString(@"mm\:ss");
            lblResult.Text = summary;
            WriteHistory(result);
            bool wasTray = isMinimizedToTray;
            if (result.Cancelled)
            {
                if (wasTray)
                {
                    RestoreFromTray();
                }

                MessageBox.Show(this, "File shredding operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result.Failed.Count > 0)
            {
                if (wasTray)
                {
                    RestoreFromTray();
                }

                string reason = result.Failed[0].Detail ?? "Unknown error";
                MessageBox.Show(this,
                    result.Succeeded.Count + " file(s) shredded successfully.\n" +
                    result.Failed.Count + " file(s) could not be shredded and remain in the list.\n\nExample: " + reason,
                    "Completed with errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (wasTray && notifyIcon != null)
            {
                notifyIcon.ShowBalloonTip(4000, "Secure File Shredder", summary, ToolTipIcon.Info);
            }

            if (!result.Cancelled && result.Failed.Count == 0 && result.Succeeded.Count > 0)
            {
                shredBar.SetTarget(10000);
            }

            UpdateSummary();
        }

        private void WriteHistory(ShredBatchResult result)
        {
            int passes = (cmbPasses.SelectedItem as ShredPreset)?.Passes ?? 1;
            var entries = result.Succeeded.Select(item => Entry(item, "Success", passes))
                .Concat(result.Failed.Select(item => Entry(item, "Failed", passes)));
            ShredHistoryStore.Append(entries);
        }

        private static ShredHistoryEntry Entry(FileOutcome outcome, string status, int passes) => new()
        {
            TimeUtc = DateTime.UtcNow,
            Path = outcome.Path,
            Result = status,
            Passes = passes,
            Bytes = outcome.Bytes,
            Detail = outcome.Detail
        };

        private void RemoveEmptyRoots()
        {
            foreach (string directory in queue.Directories.ToList())
            {
                if (queue.Files.Any(path => ShredQueue.IsUnder(path, directory)))
                {
                    continue;
                }

                try
                {
                    if (Directory.Exists(directory))
                    {
                        Directory.Delete(directory, true);
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                queue.ForgetDirectory(directory);
                RemoveListPath(directory);
            }
        }

        private void SetBusy(bool busy)
        {
            shredding = busy;
            btnStartDeleting.Text = busy ? "Stop Shredding" : "Start Shredding to bits";
            btnStartDeleting.Enabled = true;
            btnClose.Visible = !busy;
            shredBar.Visible = busy || shredBar.Visible;
            if (busy)
            {
                lblResult.Text = "";
                lblMetrics.Text = "0%";
                shredBar.Reset();
                shredBar.Visible = true;
            }
            else
            {
                listBoxFiles.ActivePaths.Clear();
                lblMetrics.Text = "";
            }

            UpdateSummary();
        }

        private void OpenSettings()
        {
            using var form = new SettingsForm(settings, cmbPasses.SelectedIndex, cmbBufferSize.SelectedIndex);
            if (form.ShowDialog(this) != DialogResult.OK)
            {
                settings = AppSettingsStore.Load();
                return;
            }

            suppressSettingsSave = true;
            cmbPasses.SelectedIndex = Clamp(settings.PassIndex, cmbPasses.Items.Count);
            cmbBufferSize.SelectedIndex = Clamp(settings.BufferIndex, cmbBufferSize.Items.Count);
            suppressSettingsSave = false;
            ApplyTheme();
            AppSettingsStore.Save(settings);
            UpdateTips();
        }

        private void OpenHistory()
        {
            using var form = new HistoryForm(settings.DarkTheme);
            form.ShowDialog(this);
        }

        private void OpenFreeSpace()
        {
            using var form = new FreeSpaceForm(CurrentOptions(), settings.DarkTheme);
            form.ShowDialog(this);
        }

        private void Mainmenu_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listBoxFiles.Focused)
            {
                RemoveSelected();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && !shredding && !cmbPasses.DroppedDown && !cmbBufferSize.DroppedDown)
            {
                btnStartDeleting.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape && shredding)
            {
                shredCts?.Cancel();
                e.Handled = true;
            }
        }

        private void Mainmenu_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (shredding)
            {
                e.Cancel = true;
                return;
            }

            SaveWindowBounds();
            AppSettingsStore.Save(settings);
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                if (trayBaseIcon != null)
                {
                    notifyIcon.Icon = trayBaseIcon;
                }

                DisposeTrayProgressIcon();
                notifyIcon.Dispose();
                notifyIcon = null;
            }

            fadeTimer.Dispose();
            pulseTimer.Dispose();
            rowFadeTimer.Dispose();
            dashTimer.Dispose();
            toolTip.Dispose();
        }

        private void UpdateTrayProgressIcon(int percent)
        {
            if (notifyIcon == null)
            {
                return;
            }

            if (percent <= 0)
            {
                if (trayBaseIcon != null)
                {
                    notifyIcon.Icon = trayBaseIcon;
                }

                DisposeTrayProgressIcon();
                lastTrayPercent = 0;
                return;
            }

            int pct = Math.Clamp(percent, 1, 100);
            if (pct == lastTrayPercent)
            {
                return;
            }

            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "TaskbarIcon", $"pct_{pct:D3}.ico");
            if (!File.Exists(path))
            {
                return;
            }

            Icon newIcon = new(path);
            notifyIcon.Icon = newIcon;
            DisposeTrayProgressIcon();
            trayProgressIcon = newIcon;
            lastTrayPercent = pct;
        }

        private void DisposeTrayProgressIcon()
        {
            trayProgressIcon?.Dispose();
            trayProgressIcon = null;
        }

        private void MinimizeToTray()
        {
            if (notifyIcon == null)
            {
                return;
            }

            isMinimizedToTray = true;
            notifyIcon.Text = "Shredding...";
            notifyIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }

        private void RestoreFromTray()
        {
            isMinimizedToTray = false;
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
            if (notifyIcon == null)
            {
                return;
            }

            notifyIcon.Visible = false;
            notifyIcon.Text = "Secure File Shredder";
            if (trayBaseIcon != null)
            {
                notifyIcon.Icon = trayBaseIcon;
            }

            DisposeTrayProgressIcon();
            lastTrayPercent = -1;
        }

        private static int Clamp(int index, int count) => count == 0 ? -1 : Math.Clamp(index, 0, count - 1);

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (shredding)
            {
                return;
            }

            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            if (shredding)
            {
                MinimizeToTray();
                return;
            }

            WindowState = FormWindowState.Minimized;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            SetDragHighlight(false);
            if (shredding || e.Data?.GetData(DataFormats.FileDrop) is not string[] files)
            {
                return;
            }

            AddPaths(files);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (shredding || e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
            SetDragHighlight(true);
        }

        private void DragLeaveClient()
        {
            Point cursor = PointToClient(Cursor.Position);
            if (!ClientRectangle.Contains(cursor))
            {
                SetDragHighlight(false);
            }
        }

        private void SetDragHighlight(bool active)
        {
            listBoxFiles.DragActive = active;
            dashTimer.Enabled = active;
            if (!shredding)
            {
                lblStatus.Text = active ? "Release to add to the queue." : (queue.Files.Count == 0 ? "Drop files here or use Add files." : "Ready to shred.");
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclButtonDown, HtCaption, 0);
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            using var about = new About();
            about.ApplyTheme(theme);
            about.ShowDialog(this);
        }
    }
}
