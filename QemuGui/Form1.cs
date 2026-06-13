using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QemuLauncher
{
    public partial class Form1 : Form
    {
        private const string SettingsFilePath = "qemu-launcher-settings.json";
        private const string DefaultQemuPath = @"C:\Program Files\qemu\qemu-system-x86_64.exe";
        private const int DiskpartTimeoutMs = 10000;

        private static readonly HashSet<string> ResizableVgaCards = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "virtio-vga", "qxl", "vmvga", "virtio-gpu", "vmware"
        };

        private List<PhysicalDriveInfo> _allDrives = new List<PhysicalDriveInfo>();
        private readonly CancellationTokenSource _cancellationTokenSource;

        // Pending-значения для комбобоксов, которые заполняются после запуска QEMU
        private string _pendingCpu;
        private string _pendingChipset;
        private string _pendingVideo;
        private string _pendingAudioBackend;
        private string _pendingNetworkModel;
        private string _pendingPhysicalDriveDisplay;

        // ----------------------------------------------------------------
        // Конструкторы
        // ----------------------------------------------------------------
        public Form1() : this(new CancellationTokenSource()) { }

        public Form1(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();

            InitializeComponent();

            // Защита от выполнения runtime-кода в design-time
            if (!DesignMode)
            {
                LoadSettingsOnStart();
                this.FormClosing += Form1_FormClosing;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            BtnSave_Click(sender, EventArgs.Empty);
        }

        // ----------------------------------------------------------------
        // Обработчики событий дизайнера
        // ----------------------------------------------------------------
        private void BtnReloadQemu_Click(object sender, EventArgs e) => ReloadQemuConfig();

        private void ChkAudio_CheckedChanged(object sender, EventArgs e)
        {
            cmbAudioBackend.Enabled = chkAudio.Checked;
            // Обновление доступности для консистентности (хотя cmbAudioBackend уже включается)
        }

        // Общий обработчик для всех новых групповых чекбоксов
        private void ChkGroup_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            if (chk == null) return;

            // Определяем, какой группе принадлежит чекбокс, и включаем/отключаем соответствующие контролы
            if (chk == chkEnableCPU)
            {
                cmbCPU.Enabled = chk.Checked;
                cmbCores.Enabled = chk.Checked;
            }
            else if (chk == chkEnableRAM)
            {
                cmbRAM.Enabled = chk.Checked;
            }
            else if (chk == chkEnableChipset)
            {
                cmbChipset.Enabled = chk.Checked;
            }
            else if (chk == chkEnableAccel)
            {
                cmbAccelerator.Enabled = chk.Checked;
            }
            else if (chk == chkEnableVideo)
            {
                cmbVideo.Enabled = chk.Checked;
                numWidth.Enabled = chk.Checked;
                numHeight.Enabled = chk.Checked;
            }
            else if (chk == chkEnableNetwork)
            {
                cmbNetwork.Enabled = chk.Checked;
                cmbNetworkModel.Enabled = chk.Checked;
                txtMAC.Enabled = chk.Checked;
                btnGenerateMAC.Enabled = chk.Checked;
            }
            else if (chk == chkEnableSharedFolder)
            {
                txtSharedFolder.Enabled = chk.Checked;
                btnBrowseSharedFolder.Enabled = chk.Checked;
            }
            else if (chk == chkEnableDisk)
            {
                cmbDiskType.Enabled = chk.Checked;
                // При отключении диска скрываем панели, при включении показываем в зависимости от выбранного типа
                if (chk.Checked)
                {
                    CmbDiskType_SelectedIndexChanged(cmbDiskType, EventArgs.Empty);
                }
                else
                {
                    pnlPhysicalDrive.Visible = false;
                    pnlHddImage.Visible = false;
                }
            }
            else if (chk == chkEnableBoot)
            {
                cmbBootOrder.Enabled = chk.Checked;
            }
            else if (chk == chkEnableInput)
            {
                cmbKeyboard.Enabled = chk.Checked;
                cmbMouse.Enabled = chk.Checked;
                cmbUsbController.Enabled = chk.Checked;
            }
            else if (chk == chkEnableISO)
            {
                txtISO.Enabled = chk.Checked;
                btnBrowseISO.Enabled = chk.Checked;
            }
        }
        // ----------------------------------------------------------------
        // QEMU configuration
        // ----------------------------------------------------------------
        private void ReloadQemuConfig()
        {
            if (string.IsNullOrWhiteSpace(txtQemuPath.Text) || !File.Exists(txtQemuPath.Text))
            {
                lblStatus.Text = "QEMU executable not found. Using fallback lists.";
                SetFallbackOptions();
                ApplyComboBoxSelections();
                return;
            }

            var config = QemuConfig.LoadOrCreate(txtQemuPath.Text);
            if (config == null)
            {
                lblStatus.Text = "Failed to load QEMU configuration.";
                SetFallbackOptions();
                ApplyComboBoxSelections();
                return;
            }

            if (config.CPUs != null && config.CPUs.Count > 0)
            {
                cmbCPU.Items.Clear();
                cmbCPU.Items.AddRange(config.CPUs.Cast<object>().ToArray());
            }

            if (config.Machines != null && config.Machines.Count > 0)
            {
                cmbChipset.Items.Clear();
                cmbChipset.Items.AddRange(config.Machines.Cast<object>().ToArray());
            }

            if (config.VideoCards != null && config.VideoCards.Count > 0)
            {
                cmbVideo.Items.Clear();
                cmbVideo.Items.AddRange(config.VideoCards.Cast<object>().ToArray());
            }
            else
            {
                cmbVideo.Items.Clear();
                cmbVideo.Items.AddRange(new object[] { "std", "cirrus", "vmware", "qxl", "virtio" });
            }

            if (config.AudioDevices != null && config.AudioDevices.Count > 0)
            {
                cmbAudioBackend.Items.Clear();
                cmbAudioBackend.Items.AddRange(config.AudioDevices.Cast<object>().ToArray());
                chkAudio.Text = $"Add Audio ({string.Join(",", config.AudioDevices.Take(3))})";
            }
            else
            {
                cmbAudioBackend.Items.Clear();
                cmbAudioBackend.Items.AddRange(new object[] { "dsound", "none" });
                chkAudio.Text = "Add Audio";
            }

            if (config.NetworkDevices != null && config.NetworkDevices.Count > 0)
            {
                cmbNetworkModel.Items.Clear();
                cmbNetworkModel.Items.AddRange(config.NetworkDevices.Cast<object>().ToArray());
            }

            ApplyComboBoxSelections();
            lblStatus.Text = "QEMU configuration loaded.";
        }

        private void ApplyComboBoxSelections()
        {
            SetComboBoxSelection(cmbCPU, _pendingCpu, "qemu64");
            SetComboBoxSelection(cmbChipset, _pendingChipset, "q35");
            SetComboBoxSelection(cmbVideo, _pendingVideo, "std");
            SetComboBoxSelection(cmbAudioBackend, _pendingAudioBackend, "dsound");
            SetComboBoxSelection(cmbNetworkModel, _pendingNetworkModel, "e1000");
        }

        private static void SetComboBoxSelection(ComboBox cmb, string value, string defaultValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int idx = cmb.FindStringExact(value);
                if (idx >= 0) { cmb.SelectedIndex = idx; return; }
            }
            int defIdx = cmb.FindStringExact(defaultValue);
            if (defIdx >= 0)
                cmb.SelectedIndex = defIdx;
            else if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }

        private static void SelectComboByValue(ComboBox cmb, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            int idx = cmb.FindStringExact(value);
            if (idx >= 0) cmb.SelectedIndex = idx;
        }

        private void SetFallbackOptions()
        {
            cmbCPU.Items.Clear();
            cmbCPU.Items.AddRange(new object[] { "qemu64", "host", "core2duo", "penryn" });

            cmbChipset.Items.Clear();
            cmbChipset.Items.AddRange(new object[] { "pc", "q35" });

            cmbVideo.Items.Clear();
            cmbVideo.Items.AddRange(new object[] { "std", "cirrus", "vmware", "qxl", "virtio" });

            cmbAudioBackend.Items.Clear();
            cmbAudioBackend.Items.AddRange(new object[] { "dsound", "none" });

            cmbNetworkModel.Items.Clear();
            cmbNetworkModel.Items.AddRange(new object[] { "e1000", "rtl8139", "virtio-net-pci" });

            chkAudio.Text = "Add Audio";
        }

        // ----------------------------------------------------------------
        // Helpers
        // ----------------------------------------------------------------
        private static bool IsRunAsAdmin()
        {
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var wp = new System.Security.Principal.WindowsPrincipal(wi);
            return wp.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private void RestartAsAdmin()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restart as administrator: {ex.Message}", "Error");
                return;
            }
            Application.Exit();
        }

        private static string AnalyzeQemuError(string errorText)
        {
            if (string.IsNullOrWhiteSpace(errorText))
                return "QEMU exited without visible error message.\nCheck parameters and permissions.";

            string lower = errorText.ToLowerInvariant();

            if (lower.Contains("whpx") && lower.Contains("not available"))
                return "Hyper-V (WHPX) is not available.\nMake sure:\n- Virtualization is enabled in BIOS\n- Hyper-V is enabled (Windows Features)\n- You run launcher as administrator.";
            if (lower.Contains("hax") && lower.Contains("not available"))
                return "HAXM is not available.\nMake sure Intel HAXM driver is installed and active.";
            if (lower.Contains("kvm") && lower.Contains("not available"))
                return "KVM is not available. You are probably not on Linux, or kvm module is not loaded.";
            if (lower.Contains("could not open") && lower.Contains("physicaldrive"))
                return "Could not open PhysicalDrive.\nDisk may be Online or in use by system. Try 'Set Offline' before starting.";
            if (lower.Contains("could not open") && (lower.Contains(".img") || lower.Contains(".qcow2")))
                return "Could not open disk image file.\nCheck if file exists and is accessible.";
            if (lower.Contains("accelerator") && lower.Contains("invalid"))
                return "Selected accelerator is not supported by your system. Try switching to TCG.";
            if (lower.Contains("access denied"))
                return "Access denied. Run launcher as administrator.";

            return $"QEMU reported an error:\n\n{errorText}";
        }

        private static bool IsValidMac(string mac) =>
            !string.IsNullOrWhiteSpace(mac) &&
            Regex.IsMatch(mac.Trim(), @"^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$");

        // ----------------------------------------------------------------
        // Event handlers — Browse / Disk
        // ----------------------------------------------------------------
        private void BtnBrowseQemu_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "qemu-system-x86_64.exe|qemu-system-x86_64.exe|All files (*.*)|*.*";
                ofd.Title = "Select QEMU executable";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtQemuPath.Text = ofd.FileName;
                    ReloadQemuConfig();
                }
            }
        }

        private void BtnBrowseISO_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "ISO files (*.iso)|*.iso|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtISO.Text = ofd.FileName;
            }
        }

        private void CmbDiskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!chkEnableDisk.Checked) return; // если диск отключён, ничего не делаем
            bool isPhysical = cmbDiskType.SelectedItem?.ToString() == "PhysicalDrive";
            pnlPhysicalDrive.Visible = isPhysical;
            pnlHddImage.Visible = !isPhysical;
            if (isPhysical) RefreshPhysicalDrives();
        }

        private void BtnRefreshDrives_Click(object sender, EventArgs e) => RefreshPhysicalDrives();

        private void BtnSetOffline_Click(object sender, EventArgs e)
        {
            if (cmbPhysicalDrive.SelectedItem == null) return;
            var match = Regex.Match(cmbPhysicalDrive.SelectedItem.ToString(), @"Disk (\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int diskNum))
            {
                if (SetDriveOffline(diskNum))
                {
                    MessageBox.Show($"Disk {diskNum} is now Offline.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshPhysicalDrives();
                }
                else
                {
                    MessageBox.Show("Failed to set disk Offline. Run as Administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnBrowseHDD_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Disk images (*.img;*.raw;*.qcow2;*.vhd;*.vmdk)|*.img;*.raw;*.qcow2;*.vhd;*.vmdk|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtHDD.Text = ofd.FileName;
            }
        }

        private void BtnCreateHDD_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Raw image (*.img)|*.img|Qcow2 image (*.qcow2)|*.qcow2";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                string path = sfd.FileName;
                string format = Path.GetExtension(path).ToLowerInvariant() == ".qcow2" ? "qcow2" : "raw";
                string size = ShowSizeDialog();
                if (string.IsNullOrEmpty(size)) return;

                string qemuDir = Path.GetDirectoryName(txtQemuPath.Text);
                string qemuImg = string.IsNullOrEmpty(qemuDir) ? "qemu-img.exe" : Path.Combine(qemuDir, "qemu-img.exe");

                if (!File.Exists(qemuImg))
                {
                    if (format != "raw") { MessageBox.Show("qemu-img.exe not found. Cannot create qcow2 image.", "Error"); return; }
                    if (TryCreateRawImage(path, size)) txtHDD.Text = path;
                    return;
                }

                var psi = new ProcessStartInfo
                {
                    FileName = qemuImg,
                    Arguments = $"create -f {format} \"{path}\" {size}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };

                using (var proc = Process.Start(psi))
                {
                    if (proc == null) { MessageBox.Show("Failed to start qemu-img.", "Error"); return; }

                    string errorOutput = proc.StandardError.ReadToEnd();
                    bool finished = proc.WaitForExit(30000);

                    if (!finished)
                    {
                        try { proc.Kill(); } catch (InvalidOperationException) { }
                        MessageBox.Show("qemu-img timed out.", "Error");
                        return;
                    }

                    if (proc.ExitCode == 0)
                        txtHDD.Text = path;
                    else
                        MessageBox.Show($"qemu-img create failed.\n{errorOutput}", "Error");
                }
            }
        }

        private string ShowSizeDialog()
        {
            using (var dlg = new Form
            {
                Text = "Image Size",
                Size = new System.Drawing.Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                dlg.SuspendLayout();
                var lbl = new Label { Text = "Size (e.g. 10G, 512M):", Location = new System.Drawing.Point(10, 20), Size = new System.Drawing.Size(200, 20) };
                var txt = new TextBox { Text = "10G", Location = new System.Drawing.Point(10, 50), Size = new System.Drawing.Size(200, 20) };
                var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(100, 80), Size = new System.Drawing.Size(75, 23) };
                dlg.Controls.AddRange(new Control[] { lbl, txt, btnOk });
                dlg.AcceptButton = btnOk;
                dlg.ResumeLayout(false);
                dlg.PerformLayout();
                return dlg.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : null;
            }
        }

        private void BtnGenerateMAC_Click(object sender, EventArgs e)
        {
            var bytes = new byte[3];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(bytes);
            txtMAC.Text = $"52:54:00:{bytes[0]:X2}:{bytes[1]:X2}:{bytes[2]:X2}";
        }
        private static bool TryCreateRawImage(string path, string size)
        {
            try
            {
                long sizeBytes = ParseSize(size);
                if (sizeBytes <= 0) return false;
                using (var fs = new FileStream(path, FileMode.CreateNew))
                    fs.SetLength(sizeBytes);
                return true;
            }
            catch { return false; }
        }

        private static long ParseSize(string size)
        {
            var m = Regex.Match(size.Trim(), @"^(\d+)([KMGkmg])?$");
            if (!m.Success) return 0;
            long num = long.Parse(m.Groups[1].Value);
            switch (m.Groups[2].Value.ToUpperInvariant())
            {
                case "K": return num * 1024L;
                case "M": return num * 1024L * 1024L;
                case "G": return num * 1024L * 1024L * 1024L;
                default: return num;
            }
        }

        // ----------------------------------------------------------------
        // Physical drives
        // ----------------------------------------------------------------
        private void RefreshPhysicalDrives()
        {
            _allDrives = GetPhysicalDrives();
            cmbPhysicalDrive.Items.Clear();

            if (_allDrives.Count == 0)
            {
                lblStatus.Text = "No physical drives detected or access denied.";
                return;
            }

            foreach (var d in _allDrives)
                cmbPhysicalDrive.Items.Add($"Disk {d.Number} ({d.SizeGB} GB) - {d.Status}");

            cmbPhysicalDrive.SelectedIndex = 0;
        }

        private void RestorePhysicalDriveSelection()
        {
            if (string.IsNullOrEmpty(_pendingPhysicalDriveDisplay)) return;
            int idx = cmbPhysicalDrive.FindStringExact(_pendingPhysicalDriveDisplay);
            if (idx >= 0) cmbPhysicalDrive.SelectedIndex = idx;
        }

        private List<PhysicalDriveInfo> GetPhysicalDrives()
        {
            var drives = new List<PhysicalDriveInfo>();
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Index, Size FROM Win32_DiskDrive"))
                using (var results = searcher.Get())
                {
                    foreach (ManagementBaseObject obj in results)
                        using (var disk = (ManagementObject)obj)
                        {
                            int number = Convert.ToInt32(disk["Index"]);
                            ulong size = disk["Size"] != null ? (ulong)disk["Size"] : 0UL;
                            double sizeGB = Math.Round(size / 1073741824.0, 1);
                            drives.Add(new PhysicalDriveInfo(number, sizeGB, "Unknown"));
                        }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Failed to query physical drives: " + ex.Message;
                return drives;
            }

            try
            {
                string output = RunDiskpartScript("list disk\nexit");
                if (!string.IsNullOrEmpty(output))
                    ApplyDiskpartStatuses(drives, output);
            }
            catch { }

            return drives;
        }

        private bool SetDriveOffline(int diskNumber)
        {
            try
            {
                string output = RunDiskpartScript($"select disk {diskNumber}\r\noffline disk\r\nexit");
                if (output == null) return false;
                string lower = output.ToLowerInvariant();
                return !lower.Contains("error") &&
                       !lower.Contains("ошибка") &&
                       !lower.Contains("diskpart has encountered an error");
            }
            catch { return false; }
        }

        private bool GetDriveStatusOnline(int diskNumber)
        {
            try
            {
                string output = RunDiskpartScript("list disk\nexit");
                if (string.IsNullOrEmpty(output)) return false;

                foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = Regex.Match(line, $@"(?:Disk|Диск)\s+{diskNumber}\s+(\S+)",
                                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    if (m.Success)
                        return NormalizeDiskStatus(m.Groups[1].Value.Trim())
                               .Equals("Online", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
            catch { return false; }
        }

        private static string RunDiskpartScript(string scriptContent)
        {
            string tempFile = null;
            try
            {
                tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, scriptContent, Encoding.UTF8);

                var psi = new ProcessStartInfo("diskpart", $"/s \"{tempFile}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.GetEncoding(866),
                    StandardErrorEncoding = Encoding.GetEncoding(866),
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var proc = Process.Start(psi))
                {
                    if (proc == null) return null;

                    var stdoutTask = proc.StandardOutput.ReadToEndAsync();
                    var stderrTask = proc.StandardError.ReadToEndAsync();

                    bool finished = proc.WaitForExit(DiskpartTimeoutMs);
                    if (!finished)
                    {
                        try { proc.Kill(); } catch (InvalidOperationException) { }
                        return null;
                    }

                    Task.WaitAll(stdoutTask, stderrTask);
                    return stdoutTask.Result + stderrTask.Result;
                }
            }
            finally
            {
                if (tempFile != null)
                    try { File.Delete(tempFile); } catch { }
            }
        }

        private static void ApplyDiskpartStatuses(List<PhysicalDriveInfo> drives, string output)
        {
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var m = Regex.Match(line, @"(?:Disk|Диск)\s+(\d+)\s+(\S+)",
                                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (!m.Success || !int.TryParse(m.Groups[1].Value, out int diskNum)) continue;

                string status = NormalizeDiskStatus(m.Groups[2].Value.Trim());
                var drive = drives.Find(d => d.Number == diskNum);
                if (drive != null) drive.Status = status;
            }
        }

        private static string NormalizeDiskStatus(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Unknown";
            string s = raw.Trim().ToLowerInvariant();
            if (s == "online" || s == "on-line" || s == "ok" || s == "в сети" || s == "онлайн" || s == "он-лайн") return "Online";
            if (s == "offline" || s == "off-line" || s == "офлайн" || s == "не в сети" || s == "оф-лайн") return "Offline";
            return char.ToUpperInvariant(raw[0]) + raw.Substring(1);
        }

        // ----------------------------------------------------------------
        // Command confirmation dialog
        // ----------------------------------------------------------------
        private static DialogResult ShowCommandDialog(IWin32Window owner, string fullCommand)
        {
            using (var dlg = new Form
            {
                Text = "QEMU command line",
                Size = new System.Drawing.Size(900, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                dlg.SuspendLayout();

                var txt = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Consolas", 9F),
                    Text = fullCommand,
                    WordWrap = false
                };

                var btnCopy = new Button { Text = "Copy to Clipboard", Width = 130, Height = 30 };
                var btnRun = new Button { Text = "Run", Width = 80, Height = 30, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Width = 80, Height = 30, DialogResult = DialogResult.Cancel };

                btnCopy.Click += (s, ev) =>
                {
                    try { Clipboard.SetText(fullCommand); MessageBox.Show("Copied.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    catch (Exception ex) { MessageBox.Show($"Copy failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };

                var panel = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(5) };
                panel.Controls.AddRange(new Control[] { btnCancel, btnRun, btnCopy });

                dlg.Controls.Add(panel);
                dlg.Controls.Add(txt);
                dlg.AcceptButton = btnRun;
                dlg.CancelButton = btnCancel;
                dlg.ResumeLayout(false);
                dlg.PerformLayout();

                return dlg.ShowDialog(owner);
            }
        }

        // ----------------------------------------------------------------
        // Start QEMU
        // ----------------------------------------------------------------
        private async void BtnStart_Click(object sender, EventArgs e)
        {
            string qemuPath = txtQemuPath.Text.Trim();
            if (!File.Exists(qemuPath))
            {
                MessageBox.Show($"QEMU not found at:\n{qemuPath}", "Error");
                return;
            }

            bool isAdmin = IsRunAsAdmin();

            // ---------- RAM (значение по умолчанию, даже если чекбокс выключен) ----------
            int memMb = 1024;
            if (chkEnableRAM.Checked)
            {
                if (!int.TryParse(cmbRAM.Text.Trim(), out memMb) || memMb <= 0)
                {
                    memMb = 1024;
                    cmbRAM.Text = memMb.ToString();
                }
            }

            // ---------- Акселератор ----------
            string accel = "tcg";
            bool accelNeedsAdmin = false;   // объявляем здесь, доступно во всём методе

            if (chkEnableAccel.Checked)
            {
                switch (cmbAccelerator.SelectedIndex)
                {
                    case 1: accel = "whpx"; break;
                    case 2: accel = "hax"; break;
                    case 3: accel = "kvm"; break;
                    default: accel = "tcg"; break;
                }

                accelNeedsAdmin = (accel == "whpx" || accel == "hax");   // просто присваиваем

                if (accelNeedsAdmin && !isAdmin)
                {
                    var res = MessageBox.Show($"Accelerator '{accel}' requires administrator rights.\nRestart as administrator?",
                        "Insufficient rights", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes) { RestartAsAdmin(); return; }
                    if (res == DialogResult.Cancel) return;
                    cmbAccelerator.SelectedIndex = 0;
                    accel = "tcg";
                    accelNeedsAdmin = false;
                }
            }

            // ---------- Чипсет ----------
            string machine = "pc";
            if (chkEnableChipset.Checked)
                machine = cmbChipset.SelectedItem?.ToString() ?? "pc";
            if (memMb > 4096) machine += ",phys-bits=36";

            // ---------- Видео ----------
            string videoCard = "std";
            if (chkEnableVideo.Checked)
                videoCard = cmbVideo.SelectedItem?.ToString() ?? "std";

            // ---------- Сеть ----------
            string networkModel = "e1000";
            string netType = "None";
            if (chkEnableNetwork.Checked)
            {
                networkModel = cmbNetworkModel.SelectedItem?.ToString() ?? "e1000";
                netType = cmbNetwork.SelectedItem?.ToString() ?? "None";
                if (!IsValidMac(txtMAC.Text))
                {
                    MessageBox.Show("Invalid MAC address format. Expected: XX:XX:XX:XX:XX:XX", "Error");
                    return;
                }
            }
            // ---------- Диск ----------
            string diskType = "";
            if (chkEnableDisk.Checked)
                diskType = cmbDiskType.SelectedItem?.ToString() ?? "HDD Image";

            // ---------- Загрузка ----------
            string bootOrder = "d";
            if (chkEnableBoot.Checked)
            {
                switch (cmbBootOrder.SelectedIndex)
                {
                    case 1: bootOrder = "c"; break;
                    case 2: bootOrder = "d"; break;
                    case 3: bootOrder = "c"; break;
                    case 4: bootOrder = "n"; break;
                    default: bootOrder = "d"; break;
                }
            }

            // ---------- Устройства ввода ----------
            string keyboardType = "PS/2";
            string mouseType = "USB Tablet";
            string usbControllerChoice = "Auto";
            if (chkEnableInput.Checked)
            {
                keyboardType = cmbKeyboard.SelectedItem?.ToString() ?? "PS/2";
                mouseType = cmbMouse.SelectedItem?.ToString() ?? "USB Tablet";
                usbControllerChoice = cmbUsbController.SelectedItem?.ToString() ?? "Auto";
            }

            // ========== Сбор аргументов ==========
            var args = new List<string>();

            // Машина (чипсет)
            if (chkEnableChipset.Checked)
                args.Add($"-machine {machine}");

            // CPU и ядра
            if (chkEnableCPU.Checked)
            {
                string cpuModel = cmbCPU.SelectedItem?.ToString() ?? "qemu64";
                string cores = cmbCores.SelectedItem?.ToString() ?? "2";
                args.Add($"-cpu {cpuModel}");
                args.Add($"-smp {cores}");
            }

            // RAM
            if (chkEnableRAM.Checked)
                args.Add($"-m {memMb}");

            // Акселератор
            if (chkEnableAccel.Checked)
                args.Add(accel == "whpx" ? $"-accel {accel},kernel-irqchip=off" : $"-accel {accel}");

            // Диск
            if (chkEnableDisk.Checked)
            {
                if (diskType == "PhysicalDrive")
                {
                    if (!isAdmin)
                    {
                        var res = MessageBox.Show("PhysicalDrive access requires administrator rights.\nRestart as administrator?",
                            "Insufficient rights", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (res == DialogResult.Yes) { RestartAsAdmin(); return; }
                        if (res == DialogResult.Cancel) return;
                    }

                    string selected = cmbPhysicalDrive.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(selected))
                    {
                        MessageBox.Show("No physical drive selected.", "Error");
                        return;
                    }

                    var mDisk = Regex.Match(selected, @"Disk\s+(\d+)");
                    if (!mDisk.Success || !int.TryParse(mDisk.Groups[1].Value, out int diskNum))
                    {
                        MessageBox.Show("Failed to parse disk number.", "Error");
                        return;
                    }

                    if (GetDriveStatusOnline(diskNum))
                    {
                        var res = MessageBox.Show($"Disk {diskNum} is ONLINE. Set Offline now?",
                            "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (res == DialogResult.Yes)
                        {
                            if (!SetDriveOffline(diskNum))
                            {
                                MessageBox.Show("Failed to set disk offline.", "Error");
                                return;
                            }
                            RefreshPhysicalDrives();
                        }
                        else if (res == DialogResult.Cancel) return;
                    }

                    args.Add($"-drive file=\\\\.\\PhysicalDrive{diskNum},format=raw,if=ide,index=0,media=disk");
                }
                else // HDD Image
                {
                    if (!string.IsNullOrWhiteSpace(txtHDD.Text) && File.Exists(txtHDD.Text))
                    {
                        string fmt = Path.GetExtension(txtHDD.Text).ToLowerInvariant() == ".qcow2" ? "qcow2" : "raw";
                        string controller = cmbHddController.SelectedItem?.ToString() ?? "IDE";
                        switch (controller)
                        {
                            case "AHCI":
                                args.Add("-device ahci,id=ahci");
                                args.Add($"-drive file=\"{txtHDD.Text}\",format={fmt},if=none,id=drive0");
                                args.Add("-device ide-hd,drive=drive0,bus=ahci.0");
                                break;
                            case "SCSI":
                                args.Add("-device lsi53c895a,id=scsi");
                                args.Add($"-drive file=\"{txtHDD.Text}\",format={fmt},if=none,id=drive0");
                                args.Add("-device scsi-hd,drive=drive0,bus=scsi.0");
                                break;
                            default:
                                args.Add($"-drive file=\"{txtHDD.Text}\",format={fmt},if=ide,index=0,media=disk");
                                break;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("HDD image not selected or not found. Continue without a disk?",
                            "Continue without disk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return;
                    }
                }
            }

            // ISO
            bool hasIso = chkEnableISO.Checked && !string.IsNullOrWhiteSpace(txtISO.Text) && File.Exists(txtISO.Text);
            if (hasIso)
                args.Add($"-drive file=\"{txtISO.Text}\",format=raw,if=ide,index=1,media=cdrom");

            // Boot order
            if (chkEnableBoot.Checked)
            {
                string bootFlag;
                switch (cmbBootOrder.SelectedIndex)
                {
                    case 1: bootFlag = "-boot order=c"; break;
                    case 2: bootFlag = "-boot once=d,menu=on"; break;
                    case 3: bootFlag = "-boot once=c,menu=on"; break;
                    case 4: bootFlag = "-boot order=n"; break;
                    default: bootFlag = "-boot order=d"; break;
                }
                args.Add(bootFlag);
            }

            // USB / Keyboard / Mouse
            if (chkEnableInput.Checked)
            {
                bool needUsb = keyboardType == "USB" || mouseType == "USB Mouse" || mouseType == "USB Tablet";
                if (needUsb)
                {
                    string usbDev;
                    if (usbControllerChoice == "Auto")
                        usbDev = machine.IndexOf("q35", StringComparison.OrdinalIgnoreCase) >= 0 ? "qemu-xhci" : "piix3-usb-uhci";
                    else if (usbControllerChoice.StartsWith("XHCI", StringComparison.OrdinalIgnoreCase))
                        usbDev = "qemu-xhci";
                    else if (usbControllerChoice.StartsWith("EHCI", StringComparison.OrdinalIgnoreCase))
                        usbDev = "usb-ehci";
                    else
                        usbDev = "piix3-usb-uhci";
                    args.Add($"-device {usbDev},id=usb");
                }

                if (keyboardType == "PS/2" || mouseType == "PS/2") args.Add("-device i8042");
                if (keyboardType == "USB") args.Add("-device usb-kbd" + (needUsb ? ",bus=usb.0" : ""));
                if (mouseType == "USB Mouse") args.Add("-device usb-mouse" + (needUsb ? ",bus=usb.0" : ""));
                if (mouseType == "USB Tablet") args.Add("-device usb-tablet" + (needUsb ? ",bus=usb.0" : ""));
            }

            // Видео
            if (chkEnableVideo.Checked)
            {
                args.Add($"-vga {videoCard}");
                if (numWidth.Value > 0 && numHeight.Value > 0)
                {
                    if (ResizableVgaCards.Contains(videoCard))
                    {
                        args.Add($"-global VGA.width={(int)numWidth.Value}");
                        args.Add($"-global VGA.height={(int)numHeight.Value}");
                    }
                    else
                        lblStatus.Text = "Window size ignored: not supported for this video card.";
                }
            }

            // Аудио
            if (chkAudio.Checked && chkAudio.Enabled)
            {
                if (cmbAudioBackend.SelectedItem != null)
                {
                    args.Add($"-audiodev {cmbAudioBackend.SelectedItem},id=audio0");
                    args.Add("-device AC97,audiodev=audio0");
                }
            }

            // Сеть
            if (chkEnableNetwork.Checked && netType != "None")
            {
                if (netType == "User (Slirp)")
                {
                    string userParams = "user,id=net0";
                    if (chkEnableSharedFolder.Checked && !string.IsNullOrWhiteSpace(txtSharedFolder.Text))
                    {
                        string smbPath = txtSharedFolder.Text.TrimEnd('\\');
                        userParams += $",smb={smbPath}";
                    }
                    args.Add($"-netdev {userParams}");
                    args.Add($"-device {networkModel},netdev=net0,mac={txtMAC.Text.Trim()}");
                }
                else if (netType == "Tap (Bridge)")
                {
                    args.Add("-netdev tap,id=net0,ifname=tap0,script=no,downscript=no");
                    args.Add($"-device {networkModel},netdev=net0,mac={txtMAC.Text.Trim()}");
                }
            }

            // Дополнительные аргументы
            if (!string.IsNullOrWhiteSpace(txtExtraArgs.Text))
                args.Add(txtExtraArgs.Text.Trim());

            // Проверка: если ISO нет, а boot order = CD-ROM
            if (!hasIso && chkEnableBoot.Checked && bootOrder == "d")
            {
                if (MessageBox.Show("No ISO selected. Continue booting from HDD?",
                    "Boot device missing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    args.RemoveAll(a => a.StartsWith("-boot "));
                    args.Add("-boot order=c");
                }
                else return;
            }

            string arguments = string.Join(" ", args);
            string fullCommand = $"\"{qemuPath}\" {arguments}";

            if (ShowCommandDialog(this, fullCommand) != DialogResult.OK) return;

            bool canRedirect = isAdmin || !(accelNeedsAdmin || (chkEnableDisk.Checked && diskType == "PhysicalDrive"));

            var psi2 = canRedirect
                ? new ProcessStartInfo
                {
                    FileName = qemuPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(qemuPath) ?? string.Empty
                }
                : new ProcessStartInfo
                {
                    FileName = qemuPath,
                    Arguments = arguments,
                    UseShellExecute = true,
                    Verb = "runas"
                };

            try
            {
                var proc = Process.Start(psi2);
                if (proc == null)
                {
                    MessageBox.Show("Process could not be started.", "Launch aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var token = _cancellationTokenSource.Token;
                await Task.Run(() =>
                {
                    try
                    {
                        var stderrTask = psi2.RedirectStandardError
                            ? proc.StandardError.ReadToEndAsync()
                            : Task.FromResult<string>(null);

                        bool exitedQuickly = proc.WaitForExit(3000);
                        if (!exitedQuickly || !IsHandleCreated || IsDisposed || token.IsCancellationRequested)
                            return;

                        stderrTask.Wait();
                        string errorOutput = stderrTask.Result;

                        string message = !string.IsNullOrWhiteSpace(errorOutput)
                            ? AnalyzeQemuError(errorOutput)
                            : "QEMU process exited quickly (within 3 seconds).\n\n" +
                              "Common causes:\n" +
                              "• Invalid command-line parameters\n" +
                              "• Missing administrator rights\n" +
                              "• Selected accelerator is not available\n" +
                              "• Disk image is missing or locked";

                        Invoke((MethodInvoker)(() =>
                        {
                            if (!IsDisposed)
                                MessageBox.Show(this, message, "QEMU launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    catch (OperationCanceledException) { }
                    catch { }
                }, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start QEMU: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ----------------------------------------------------------------
        // Settings
        // ----------------------------------------------------------------
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var s = new AppSettings
            {
                QemuPath = txtQemuPath.Text,
                ISO = txtISO.Text,
                DiskType = cmbDiskType.SelectedItem?.ToString() ?? "HDD Image",
                PhysicalDriveDisplay = cmbPhysicalDrive.SelectedItem?.ToString() ?? "",
                HDDImage = txtHDD.Text,
                CPU = cmbCPU.SelectedItem?.ToString() ?? "qemu64",
                Cores = cmbCores.SelectedItem?.ToString() ?? "2",
                RAM = cmbRAM.Text,
                Chipset = cmbChipset.SelectedItem?.ToString() ?? "pc",
                Audio = chkAudio.Checked,
                Video = cmbVideo.SelectedItem?.ToString() ?? "std",
                Network = cmbNetwork.SelectedItem?.ToString() ?? "None",
                NetworkModel = cmbNetworkModel.SelectedItem?.ToString() ?? "e1000",
                MAC = txtMAC.Text,
                WindowWidth = numWidth.Value,
                WindowHeight = numHeight.Value,
                AudioBackend = cmbAudioBackend.SelectedItem?.ToString() ?? "dsound",
                AcceleratorIndex = cmbAccelerator.SelectedIndex,
                ExtraArgs = txtExtraArgs.Text,
                BootOrderIndex = cmbBootOrder.SelectedIndex,
                HddControllerIndex = cmbHddController.SelectedIndex,
                KeyboardType = cmbKeyboard.SelectedItem?.ToString() ?? "PS/2",
                MouseType = cmbMouse.SelectedItem?.ToString() ?? "USB Tablet",
                UsbController = cmbUsbController.SelectedItem?.ToString() ?? "Auto",

                // Сохраняем состояния чекбоксов групп
                EnableCPU = chkEnableCPU.Checked,
                EnableRAM = chkEnableRAM.Checked,
                EnableChipset = chkEnableChipset.Checked,
                EnableAccel = chkEnableAccel.Checked,
                EnableVideo = chkEnableVideo.Checked,
                EnableNetwork = chkEnableNetwork.Checked,
                EnableDisk = chkEnableDisk.Checked,
                EnableBoot = chkEnableBoot.Checked,
                EnableInput = chkEnableInput.Checked,
                EnableISO = chkEnableISO.Checked,
                EnableSharedFolder = chkEnableSharedFolder.Checked,
                SharedFolderPath = txtSharedFolder.Text
            };
            File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(s, Formatting.Indented), Encoding.UTF8);
            lblStatus.Text = "Settings saved.";
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadSettingsFromFile();
            RefreshPhysicalDrives();
            RestorePhysicalDriveSelection();
            CmbDiskType_SelectedIndexChanged(null, EventArgs.Empty);
        }
        private void BtnBrowseSharedFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder to share with guest";
                if (fbd.ShowDialog() == DialogResult.OK)
                    txtSharedFolder.Text = fbd.SelectedPath;
            }
        }
        private void LoadSettingsFromFile()
        {
            if (!File.Exists(SettingsFilePath)) return;
            AppSettings s;
            try
            {
                s = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(SettingsFilePath, Encoding.UTF8));
            }
            catch (Exception ex) { lblStatus.Text = "Failed to load settings: " + ex.Message; return; }
            if (s == null) return;

            txtQemuPath.Text = s.QemuPath;
            txtISO.Text = s.ISO;
            SelectComboByValue(cmbDiskType, s.DiskType);

            _pendingCpu = s.CPU;
            _pendingChipset = s.Chipset;
            _pendingVideo = s.Video;
            _pendingAudioBackend = s.AudioBackend;
            _pendingNetworkModel = s.NetworkModel;
            _pendingPhysicalDriveDisplay = s.PhysicalDriveDisplay;

            txtHDD.Text = s.HDDImage;
            SelectComboByValue(cmbCores, s.Cores);
            cmbRAM.Text = s.RAM;
            chkAudio.Checked = s.Audio;
            cmbAudioBackend.Enabled = s.Audio;
            txtMAC.Text = s.MAC;
            SafeSetNumericValue(numWidth, s.WindowWidth);
            SafeSetNumericValue(numHeight, s.WindowHeight);
            txtExtraArgs.Text = s.ExtraArgs;

            if (s.AcceleratorIndex >= 0 && s.AcceleratorIndex < cmbAccelerator.Items.Count) cmbAccelerator.SelectedIndex = s.AcceleratorIndex;
            if (s.BootOrderIndex >= 0 && s.BootOrderIndex < cmbBootOrder.Items.Count) cmbBootOrder.SelectedIndex = s.BootOrderIndex;
            if (s.HddControllerIndex >= 0 && s.HddControllerIndex < cmbHddController.Items.Count) cmbHddController.SelectedIndex = s.HddControllerIndex;

            SelectComboByValue(cmbNetwork, s.Network);
            SelectComboByValue(cmbKeyboard, s.KeyboardType);
            SelectComboByValue(cmbMouse, s.MouseType);
            SelectComboByValue(cmbUsbController, s.UsbController);

            // Восстанавливаем состояния чекбоксов групп
            chkEnableCPU.Checked = s.EnableCPU;
            chkEnableRAM.Checked = s.EnableRAM;
            chkEnableChipset.Checked = s.EnableChipset;
            chkEnableAccel.Checked = s.EnableAccel;
            chkEnableVideo.Checked = s.EnableVideo;
            chkEnableNetwork.Checked = s.EnableNetwork;
            chkEnableDisk.Checked = s.EnableDisk;
            chkEnableBoot.Checked = s.EnableBoot;
            chkEnableInput.Checked = s.EnableInput;
            chkEnableISO.Checked = s.EnableISO;
            chkEnableSharedFolder.Checked = s.EnableSharedFolder;
            txtSharedFolder.Text = s.SharedFolderPath ?? "";

            // Вызываем обновление видимости/доступности
            foreach (var chk in new CheckBox[] { chkEnableCPU, chkEnableRAM, chkEnableChipset, chkEnableAccel,
                                                   chkEnableVideo, chkEnableNetwork, chkEnableDisk,
                                                   chkEnableBoot, chkEnableInput, chkEnableISO, chkEnableSharedFolder })
            {
                ChkGroup_CheckedChanged(chk, EventArgs.Empty);
            }

            lblStatus.Text = "Settings loaded.";
        }

        private void LoadSettingsOnStart()
        {
            if (File.Exists(SettingsFilePath))
                LoadSettingsFromFile();
            else
                txtQemuPath.Text = DefaultQemuPath;

            ReloadQemuConfig();
            CmbDiskType_SelectedIndexChanged(null, EventArgs.Empty);
            RestorePhysicalDriveSelection();
            chkEnableSharedFolder.Checked = false;
            txtSharedFolder.Text = "";
            ChkGroup_CheckedChanged(chkEnableSharedFolder, EventArgs.Empty);
        }

        private static void SafeSetNumericValue(NumericUpDown ctrl, decimal value) =>
            ctrl.Value = Math.Max(ctrl.Minimum, Math.Min(ctrl.Maximum, value));

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}