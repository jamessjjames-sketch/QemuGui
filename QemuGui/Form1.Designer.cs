using System.Windows.Forms;

namespace QemuLauncher
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtQemuPath = new System.Windows.Forms.TextBox();
            this.btnBrowseQemu = new System.Windows.Forms.Button();
            this.btnReloadQemu = new System.Windows.Forms.Button();
            this.txtISO = new System.Windows.Forms.TextBox();
            this.btnBrowseISO = new System.Windows.Forms.Button();
            this.cmbDiskType = new System.Windows.Forms.ComboBox();
            this.pnlPhysicalDrive = new System.Windows.Forms.Panel();
            this.lblPhys = new System.Windows.Forms.Label();
            this.cmbPhysicalDrive = new System.Windows.Forms.ComboBox();
            this.btnRefreshDrives = new System.Windows.Forms.Button();
            this.btnSetOffline = new System.Windows.Forms.Button();
            this.cmbHddController = new System.Windows.Forms.ComboBox();
            this.lblHddController = new System.Windows.Forms.Label();
            this.pnlHddImage = new System.Windows.Forms.Panel();
            this.lblHdd = new System.Windows.Forms.Label();
            this.txtHDD = new System.Windows.Forms.TextBox();
            this.btnBrowseHDD = new System.Windows.Forms.Button();
            this.btnCreateHDD = new System.Windows.Forms.Button();
            this.cmbBootOrder = new System.Windows.Forms.ComboBox();
            this.cmbCPU = new System.Windows.Forms.ComboBox();
            this.cmbCores = new System.Windows.Forms.ComboBox();
            this.cmbRAM = new System.Windows.Forms.ComboBox();
            this.cmbAccelerator = new System.Windows.Forms.ComboBox();
            this.cmbChipset = new System.Windows.Forms.ComboBox();
            this.chkAudio = new System.Windows.Forms.CheckBox();
            this.cmbAudioBackend = new System.Windows.Forms.ComboBox();
            this.cmbVideo = new System.Windows.Forms.ComboBox();
            this.cmbNetwork = new System.Windows.Forms.ComboBox();
            this.txtMAC = new System.Windows.Forms.TextBox();
            this.btnGenerateMAC = new System.Windows.Forms.Button();
            this.cmbNetworkModel = new System.Windows.Forms.ComboBox();
            this.cmbKeyboard = new System.Windows.Forms.ComboBox();
            this.cmbMouse = new System.Windows.Forms.ComboBox();
            this.cmbUsbController = new System.Windows.Forms.ComboBox();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.txtExtraArgs = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblQemuPath = new System.Windows.Forms.Label();
            this.lblISO = new System.Windows.Forms.Label();
            this.lblDiskType = new System.Windows.Forms.Label();
            this.lblBootOrder = new System.Windows.Forms.Label();
            this.lblCPU = new System.Windows.Forms.Label();
            this.lblCores = new System.Windows.Forms.Label();
            this.lblRAM = new System.Windows.Forms.Label();
            this.lblAccel = new System.Windows.Forms.Label();
            this.lblChipset = new System.Windows.Forms.Label();
            this.lblVideo = new System.Windows.Forms.Label();
            this.lblNetwork = new System.Windows.Forms.Label();
            this.lblMAC = new System.Windows.Forms.Label();
            this.lblNetworkModel = new System.Windows.Forms.Label();
            this.lblKeyboard = new System.Windows.Forms.Label();
            this.lblMouse = new System.Windows.Forms.Label();
            this.lblUsbController = new System.Windows.Forms.Label();
            this.lblRes = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblAuto = new System.Windows.Forms.Label();
            this.lblExtraArgs = new System.Windows.Forms.Label();
            this.chkEnableCPU = new System.Windows.Forms.CheckBox();
            this.chkEnableRAM = new System.Windows.Forms.CheckBox();
            this.chkEnableChipset = new System.Windows.Forms.CheckBox();
            this.chkEnableAccel = new System.Windows.Forms.CheckBox();
            this.chkEnableVideo = new System.Windows.Forms.CheckBox();
            this.chkEnableNetwork = new System.Windows.Forms.CheckBox();
            this.chkEnableDisk = new System.Windows.Forms.CheckBox();
            this.chkEnableBoot = new System.Windows.Forms.CheckBox();
            this.chkEnableInput = new System.Windows.Forms.CheckBox();
            this.chkEnableISO = new System.Windows.Forms.CheckBox();
            this.chkEnableSharedFolder = new System.Windows.Forms.CheckBox();
            this.txtSharedFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseSharedFolder = new System.Windows.Forms.Button();
            this.lblSharedFolder = new System.Windows.Forms.Label();
            this.pnlPhysicalDrive.SuspendLayout();
            this.pnlHddImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // txtQemuPath
            // 
            this.txtQemuPath.Location = new System.Drawing.Point(118, 16);
            this.txtQemuPath.Name = "txtQemuPath";
            this.txtQemuPath.Size = new System.Drawing.Size(243, 20);
            this.txtQemuPath.TabIndex = 1;
            // 
            // btnBrowseQemu
            // 
            this.btnBrowseQemu.Location = new System.Drawing.Point(444, 17);
            this.btnBrowseQemu.Name = "btnBrowseQemu";
            this.btnBrowseQemu.Size = new System.Drawing.Size(58, 24);
            this.btnBrowseQemu.TabIndex = 2;
            this.btnBrowseQemu.Text = "Browse...";
            this.btnBrowseQemu.Click += new System.EventHandler(this.BtnBrowseQemu_Click);
            // 
            // btnReloadQemu
            // 
            this.btnReloadQemu.Location = new System.Drawing.Point(378, 16);
            this.btnReloadQemu.Name = "btnReloadQemu";
            this.btnReloadQemu.Size = new System.Drawing.Size(58, 24);
            this.btnReloadQemu.TabIndex = 3;
            this.btnReloadQemu.Text = "Reload";
            this.btnReloadQemu.Click += new System.EventHandler(this.BtnReloadQemu_Click);
            // 
            // txtISO
            // 
            this.txtISO.Location = new System.Drawing.Point(118, 48);
            this.txtISO.Name = "txtISO";
            this.txtISO.Size = new System.Drawing.Size(318, 20);
            this.txtISO.TabIndex = 5;
            // 
            // btnBrowseISO
            // 
            this.btnBrowseISO.Location = new System.Drawing.Point(444, 48);
            this.btnBrowseISO.Name = "btnBrowseISO";
            this.btnBrowseISO.Size = new System.Drawing.Size(58, 24);
            this.btnBrowseISO.TabIndex = 6;
            this.btnBrowseISO.Text = "Browse...";
            this.btnBrowseISO.Click += new System.EventHandler(this.BtnBrowseISO_Click);
            // 
            // cmbDiskType
            // 
            this.cmbDiskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiskType.Items.AddRange(new object[] {
            "PhysicalDrive",
            "HDD Image"});
            this.cmbDiskType.Location = new System.Drawing.Point(118, 80);
            this.cmbDiskType.Name = "cmbDiskType";
            this.cmbDiskType.Size = new System.Drawing.Size(243, 21);
            this.cmbDiskType.TabIndex = 8;
            this.cmbDiskType.SelectedIndexChanged += new System.EventHandler(this.CmbDiskType_SelectedIndexChanged);
            // 
            // pnlPhysicalDrive
            // 
            this.pnlPhysicalDrive.Controls.Add(this.lblPhys);
            this.pnlPhysicalDrive.Controls.Add(this.cmbPhysicalDrive);
            this.pnlPhysicalDrive.Controls.Add(this.btnRefreshDrives);
            this.pnlPhysicalDrive.Controls.Add(this.btnSetOffline);
            this.pnlPhysicalDrive.Controls.Add(this.cmbHddController);
            this.pnlPhysicalDrive.Controls.Add(this.lblHddController);
            this.pnlPhysicalDrive.Location = new System.Drawing.Point(12, 110);
            this.pnlPhysicalDrive.Name = "pnlPhysicalDrive";
            this.pnlPhysicalDrive.Size = new System.Drawing.Size(510, 62);
            this.pnlPhysicalDrive.TabIndex = 9;
            this.pnlPhysicalDrive.Visible = false;
            // 
            // lblPhys
            // 
            this.lblPhys.Location = new System.Drawing.Point(0, 5);
            this.lblPhys.Name = "lblPhys";
            this.lblPhys.Size = new System.Drawing.Size(100, 23);
            this.lblPhys.TabIndex = 0;
            this.lblPhys.Text = "PhysicalDrive:";
            // 
            // cmbPhysicalDrive
            // 
            this.cmbPhysicalDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhysicalDrive.Location = new System.Drawing.Point(105, 3);
            this.cmbPhysicalDrive.Name = "cmbPhysicalDrive";
            this.cmbPhysicalDrive.Size = new System.Drawing.Size(244, 21);
            this.cmbPhysicalDrive.TabIndex = 1;
            // 
            // btnRefreshDrives
            // 
            this.btnRefreshDrives.Location = new System.Drawing.Point(366, 5);
            this.btnRefreshDrives.Name = "btnRefreshDrives";
            this.btnRefreshDrives.Size = new System.Drawing.Size(58, 24);
            this.btnRefreshDrives.TabIndex = 2;
            this.btnRefreshDrives.Text = "Refresh";
            this.btnRefreshDrives.Click += new System.EventHandler(this.BtnRefreshDrives_Click);
            // 
            // btnSetOffline
            // 
            this.btnSetOffline.Location = new System.Drawing.Point(432, 5);
            this.btnSetOffline.Name = "btnSetOffline";
            this.btnSetOffline.Size = new System.Drawing.Size(58, 24);
            this.btnSetOffline.TabIndex = 3;
            this.btnSetOffline.Text = "Set Offline";
            this.btnSetOffline.Click += new System.EventHandler(this.BtnSetOffline_Click);
            // 
            // cmbHddController
            // 
            this.cmbHddController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHddController.Items.AddRange(new object[] {
            "IDE",
            "AHCI",
            "SCSI"});
            this.cmbHddController.Location = new System.Drawing.Point(106, 30);
            this.cmbHddController.Name = "cmbHddController";
            this.cmbHddController.Size = new System.Drawing.Size(100, 21);
            this.cmbHddController.TabIndex = 12;
            // 
            // lblHddController
            // 
            this.lblHddController.Location = new System.Drawing.Point(0, 33);
            this.lblHddController.Name = "lblHddController";
            this.lblHddController.Size = new System.Drawing.Size(100, 23);
            this.lblHddController.TabIndex = 11;
            this.lblHddController.Text = "HDD Controller:";
            // 
            // pnlHddImage
            // 
            this.pnlHddImage.Controls.Add(this.lblHdd);
            this.pnlHddImage.Controls.Add(this.txtHDD);
            this.pnlHddImage.Controls.Add(this.btnBrowseHDD);
            this.pnlHddImage.Controls.Add(this.btnCreateHDD);
            this.pnlHddImage.Location = new System.Drawing.Point(12, 110);
            this.pnlHddImage.Name = "pnlHddImage";
            this.pnlHddImage.Size = new System.Drawing.Size(510, 62);
            this.pnlHddImage.TabIndex = 10;
            // 
            // lblHdd
            // 
            this.lblHdd.Location = new System.Drawing.Point(0, 5);
            this.lblHdd.Name = "lblHdd";
            this.lblHdd.Size = new System.Drawing.Size(100, 23);
            this.lblHdd.TabIndex = 0;
            this.lblHdd.Text = "HDD Image:";
            // 
            // txtHDD
            // 
            this.txtHDD.Location = new System.Drawing.Point(105, 3);
            this.txtHDD.Name = "txtHDD";
            this.txtHDD.Size = new System.Drawing.Size(244, 20);
            this.txtHDD.TabIndex = 1;
            // 
            // btnBrowseHDD
            // 
            this.btnBrowseHDD.Location = new System.Drawing.Point(356, 1);
            this.btnBrowseHDD.Name = "btnBrowseHDD";
            this.btnBrowseHDD.Size = new System.Drawing.Size(75, 24);
            this.btnBrowseHDD.TabIndex = 2;
            this.btnBrowseHDD.Text = "Browse...";
            this.btnBrowseHDD.Click += new System.EventHandler(this.BtnBrowseHDD_Click);
            // 
            // btnCreateHDD
            // 
            this.btnCreateHDD.Location = new System.Drawing.Point(105, 32);
            this.btnCreateHDD.Name = "btnCreateHDD";
            this.btnCreateHDD.Size = new System.Drawing.Size(75, 24);
            this.btnCreateHDD.TabIndex = 3;
            this.btnCreateHDD.Text = "Create...";
            this.btnCreateHDD.Click += new System.EventHandler(this.BtnCreateHDD_Click);
            // 
            // cmbBootOrder
            // 
            this.cmbBootOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBootOrder.Items.AddRange(new object[] {
            "CD-ROM first",
            "HDD first",
            "Only CD-ROM",
            "Only HDD",
            "Network (PXE) first"});
            this.cmbBootOrder.Location = new System.Drawing.Point(118, 181);
            this.cmbBootOrder.Name = "cmbBootOrder";
            this.cmbBootOrder.Size = new System.Drawing.Size(150, 21);
            this.cmbBootOrder.TabIndex = 14;
            // 
            // cmbCPU
            // 
            this.cmbCPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCPU.Location = new System.Drawing.Point(117, 215);
            this.cmbCPU.Name = "cmbCPU";
            this.cmbCPU.Size = new System.Drawing.Size(151, 21);
            this.cmbCPU.TabIndex = 16;
            // 
            // cmbCores
            // 
            this.cmbCores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCores.Items.AddRange(new object[] {
            "1",
            "2",
            "4"});
            this.cmbCores.Location = new System.Drawing.Point(382, 215);
            this.cmbCores.Name = "cmbCores";
            this.cmbCores.Size = new System.Drawing.Size(31, 21);
            this.cmbCores.TabIndex = 18;
            // 
            // cmbRAM
            // 
            this.cmbRAM.Items.AddRange(new object[] {
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "6144",
            "8192",
            "16384"});
            this.cmbRAM.Location = new System.Drawing.Point(118, 249);
            this.cmbRAM.Name = "cmbRAM";
            this.cmbRAM.Size = new System.Drawing.Size(150, 21);
            this.cmbRAM.TabIndex = 20;
            // 
            // cmbAccelerator
            // 
            this.cmbAccelerator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccelerator.Items.AddRange(new object[] {
            "TCG (software)",
            "Hyper-V (whpx)",
            "HAXM (hax)",
            "KVM (Linux)"});
            this.cmbAccelerator.Location = new System.Drawing.Point(382, 250);
            this.cmbAccelerator.Name = "cmbAccelerator";
            this.cmbAccelerator.Size = new System.Drawing.Size(97, 21);
            this.cmbAccelerator.TabIndex = 22;
            // 
            // cmbChipset
            // 
            this.cmbChipset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChipset.Location = new System.Drawing.Point(117, 283);
            this.cmbChipset.Name = "cmbChipset";
            this.cmbChipset.Size = new System.Drawing.Size(151, 21);
            this.cmbChipset.TabIndex = 24;
            // 
            // chkAudio
            // 
            this.chkAudio.Location = new System.Drawing.Point(289, 284);
            this.chkAudio.Name = "chkAudio";
            this.chkAudio.Size = new System.Drawing.Size(81, 24);
            this.chkAudio.TabIndex = 25;
            this.chkAudio.Text = "Add Audio";
            this.chkAudio.CheckedChanged += new System.EventHandler(this.ChkAudio_CheckedChanged);
            // 
            // cmbAudioBackend
            // 
            this.cmbAudioBackend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioBackend.Enabled = false;
            this.cmbAudioBackend.Location = new System.Drawing.Point(382, 284);
            this.cmbAudioBackend.Name = "cmbAudioBackend";
            this.cmbAudioBackend.Size = new System.Drawing.Size(97, 21);
            this.cmbAudioBackend.TabIndex = 26;
            // 
            // cmbVideo
            // 
            this.cmbVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideo.Location = new System.Drawing.Point(117, 317);
            this.cmbVideo.Name = "cmbVideo";
            this.cmbVideo.Size = new System.Drawing.Size(151, 21);
            this.cmbVideo.TabIndex = 28;
            // 
            // cmbNetwork
            // 
            this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetwork.Items.AddRange(new object[] {
            "User (Slirp)",
            "Tap (Bridge)",
            "None"});
            this.cmbNetwork.Location = new System.Drawing.Point(117, 351);
            this.cmbNetwork.Name = "cmbNetwork";
            this.cmbNetwork.Size = new System.Drawing.Size(151, 21);
            this.cmbNetwork.TabIndex = 30;
            // 
            // txtMAC
            // 
            this.txtMAC.Location = new System.Drawing.Point(382, 387);
            this.txtMAC.Name = "txtMAC";
            this.txtMAC.Size = new System.Drawing.Size(99, 20);
            this.txtMAC.TabIndex = 32;
            this.txtMAC.Text = "52:54:00:12:34:56";
            // 
            // btnGenerateMAC
            // 
            this.btnGenerateMAC.Location = new System.Drawing.Point(487, 387);
            this.btnGenerateMAC.Name = "btnGenerateMAC";
            this.btnGenerateMAC.Size = new System.Drawing.Size(15, 21);
            this.btnGenerateMAC.TabIndex = 33;
            this.btnGenerateMAC.Text = "G";
            this.btnGenerateMAC.Click += new System.EventHandler(this.BtnGenerateMAC_Click);
            // 
            // cmbNetworkModel
            // 
            this.cmbNetworkModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetworkModel.Location = new System.Drawing.Point(117, 386);
            this.cmbNetworkModel.Name = "cmbNetworkModel";
            this.cmbNetworkModel.Size = new System.Drawing.Size(151, 21);
            this.cmbNetworkModel.TabIndex = 35;
            // 
            // cmbKeyboard
            // 
            this.cmbKeyboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyboard.Items.AddRange(new object[] {
            "PS/2",
            "USB"});
            this.cmbKeyboard.Location = new System.Drawing.Point(117, 494);
            this.cmbKeyboard.Name = "cmbKeyboard";
            this.cmbKeyboard.Size = new System.Drawing.Size(75, 21);
            this.cmbKeyboard.TabIndex = 37;
            // 
            // cmbMouse
            // 
            this.cmbMouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMouse.Items.AddRange(new object[] {
            "PS/2",
            "USB Mouse",
            "USB Tablet"});
            this.cmbMouse.Location = new System.Drawing.Point(245, 494);
            this.cmbMouse.Name = "cmbMouse";
            this.cmbMouse.Size = new System.Drawing.Size(78, 21);
            this.cmbMouse.TabIndex = 39;
            // 
            // cmbUsbController
            // 
            this.cmbUsbController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsbController.Items.AddRange(new object[] {
            "Auto",
            "XHCI (USB 3.0)",
            "EHCI (USB 2.0)",
            "UHCI (USB 1.1)"});
            this.cmbUsbController.Location = new System.Drawing.Point(117, 456);
            this.cmbUsbController.Name = "cmbUsbController";
            this.cmbUsbController.Size = new System.Drawing.Size(151, 21);
            this.cmbUsbController.TabIndex = 41;
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(419, 317);
            this.numWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(60, 20);
            this.numWidth.TabIndex = 43;
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(419, 343);
            this.numHeight.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(60, 20);
            this.numHeight.TabIndex = 45;
            // 
            // txtExtraArgs
            // 
            this.txtExtraArgs.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtExtraArgs.Location = new System.Drawing.Point(117, 528);
            this.txtExtraArgs.Name = "txtExtraArgs";
            this.txtExtraArgs.Size = new System.Drawing.Size(410, 22);
            this.txtExtraArgs.TabIndex = 48;
            this._toolTip.SetToolTip(this.txtExtraArgs, "Additional QEMU parameters (e.g. -display sdl -device usb-host...)");
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(117, 565);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 32);
            this.btnStart.TabIndex = 50;
            this.btnStart.Text = "Start QEMU";
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(237, 565);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.TabIndex = 51;
            this.btnSave.Text = "Save Settings";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(347, 565);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 32);
            this.btnLoad.TabIndex = 52;
            this.btnLoad.Text = "Load Settings";
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(11, 611);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(616, 32);
            this.lblStatus.TabIndex = 53;
            this.lblStatus.Text = "Ready. PhysicalDrive requires OFFLINE state and admin rights.";
            // 
            // lblQemuPath
            // 
            this.lblQemuPath.Location = new System.Drawing.Point(12, 18);
            this.lblQemuPath.Name = "lblQemuPath";
            this.lblQemuPath.Size = new System.Drawing.Size(100, 23);
            this.lblQemuPath.TabIndex = 0;
            this.lblQemuPath.Text = "QEMU path:";
            // 
            // lblISO
            // 
            this.lblISO.Location = new System.Drawing.Point(12, 50);
            this.lblISO.Name = "lblISO";
            this.lblISO.Size = new System.Drawing.Size(100, 23);
            this.lblISO.TabIndex = 4;
            this.lblISO.Text = "ISO Image:";
            // 
            // lblDiskType
            // 
            this.lblDiskType.Location = new System.Drawing.Point(12, 82);
            this.lblDiskType.Name = "lblDiskType";
            this.lblDiskType.Size = new System.Drawing.Size(100, 23);
            this.lblDiskType.TabIndex = 7;
            this.lblDiskType.Text = "Disk Type:";
            // 
            // lblBootOrder
            // 
            this.lblBootOrder.Location = new System.Drawing.Point(12, 184);
            this.lblBootOrder.Name = "lblBootOrder";
            this.lblBootOrder.Size = new System.Drawing.Size(80, 23);
            this.lblBootOrder.TabIndex = 13;
            this.lblBootOrder.Text = "Boot order:";
            // 
            // lblCPU
            // 
            this.lblCPU.Location = new System.Drawing.Point(12, 218);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(100, 23);
            this.lblCPU.TabIndex = 15;
            this.lblCPU.Text = "CPU model:";
            // 
            // lblCores
            // 
            this.lblCores.Location = new System.Drawing.Point(286, 218);
            this.lblCores.Name = "lblCores";
            this.lblCores.Size = new System.Drawing.Size(42, 23);
            this.lblCores.TabIndex = 17;
            this.lblCores.Text = "Cores:";
            // 
            // lblRAM
            // 
            this.lblRAM.Location = new System.Drawing.Point(12, 252);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(100, 23);
            this.lblRAM.TabIndex = 19;
            this.lblRAM.Text = "RAM (MB):";
            // 
            // lblAccel
            // 
            this.lblAccel.Location = new System.Drawing.Point(282, 247);
            this.lblAccel.Name = "lblAccel";
            this.lblAccel.Size = new System.Drawing.Size(75, 23);
            this.lblAccel.TabIndex = 21;
            this.lblAccel.Text = "Acceleration:";
            this.lblAccel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChipset
            // 
            this.lblChipset.Location = new System.Drawing.Point(12, 286);
            this.lblChipset.Name = "lblChipset";
            this.lblChipset.Size = new System.Drawing.Size(100, 23);
            this.lblChipset.TabIndex = 23;
            this.lblChipset.Text = "Chipset:";
            // 
            // lblVideo
            // 
            this.lblVideo.Location = new System.Drawing.Point(12, 320);
            this.lblVideo.Name = "lblVideo";
            this.lblVideo.Size = new System.Drawing.Size(104, 23);
            this.lblVideo.TabIndex = 27;
            this.lblVideo.Text = "Video card:";
            // 
            // lblNetwork
            // 
            this.lblNetwork.Location = new System.Drawing.Point(12, 354);
            this.lblNetwork.Name = "lblNetwork";
            this.lblNetwork.Size = new System.Drawing.Size(100, 23);
            this.lblNetwork.TabIndex = 29;
            this.lblNetwork.Text = "Network:";
            // 
            // lblMAC
            // 
            this.lblMAC.Location = new System.Drawing.Point(286, 389);
            this.lblMAC.Name = "lblMAC";
            this.lblMAC.Size = new System.Drawing.Size(79, 23);
            this.lblMAC.TabIndex = 31;
            this.lblMAC.Text = "MAC Address:";
            // 
            // lblNetworkModel
            // 
            this.lblNetworkModel.Location = new System.Drawing.Point(12, 389);
            this.lblNetworkModel.Name = "lblNetworkModel";
            this.lblNetworkModel.Size = new System.Drawing.Size(100, 23);
            this.lblNetworkModel.TabIndex = 34;
            this.lblNetworkModel.Text = "NIC model:";
            // 
            // lblKeyboard
            // 
            this.lblKeyboard.Location = new System.Drawing.Point(9, 497);
            this.lblKeyboard.Name = "lblKeyboard";
            this.lblKeyboard.Size = new System.Drawing.Size(100, 23);
            this.lblKeyboard.TabIndex = 36;
            this.lblKeyboard.Text = "Keyboard:";
            // 
            // lblMouse
            // 
            this.lblMouse.Location = new System.Drawing.Point(198, 497);
            this.lblMouse.Name = "lblMouse";
            this.lblMouse.Size = new System.Drawing.Size(48, 23);
            this.lblMouse.TabIndex = 38;
            this.lblMouse.Text = "Mouse:";
            // 
            // lblUsbController
            // 
            this.lblUsbController.Location = new System.Drawing.Point(12, 459);
            this.lblUsbController.Name = "lblUsbController";
            this.lblUsbController.Size = new System.Drawing.Size(100, 23);
            this.lblUsbController.TabIndex = 40;
            this.lblUsbController.Text = "USB Ctrl:";
            // 
            // lblRes
            // 
            this.lblRes.Location = new System.Drawing.Point(282, 320);
            this.lblRes.Name = "lblRes";
            this.lblRes.Size = new System.Drawing.Size(131, 21);
            this.lblRes.TabIndex = 42;
            this.lblRes.Text = "Max window size (W x H):";
            // 
            // lblX
            // 
            this.lblX.Location = new System.Drawing.Point(485, 331);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(14, 23);
            this.lblX.TabIndex = 44;
            this.lblX.Text = "x";
            // 
            // lblAuto
            // 
            this.lblAuto.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblAuto.ForeColor = System.Drawing.Color.Gray;
            this.lblAuto.Location = new System.Drawing.Point(329, 340);
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new System.Drawing.Size(76, 23);
            this.lblAuto.TabIndex = 46;
            this.lblAuto.Text = "(0 = auto)";
            // 
            // lblExtraArgs
            // 
            this.lblExtraArgs.Location = new System.Drawing.Point(11, 531);
            this.lblExtraArgs.Name = "lblExtraArgs";
            this.lblExtraArgs.Size = new System.Drawing.Size(100, 23);
            this.lblExtraArgs.TabIndex = 47;
            this.lblExtraArgs.Text = "Extra args:";
            // 
            // chkEnableCPU
            // 
            this.chkEnableCPU.AutoSize = true;
            this.chkEnableCPU.Location = new System.Drawing.Point(92, 217);
            this.chkEnableCPU.Name = "chkEnableCPU";
            this.chkEnableCPU.Size = new System.Drawing.Size(15, 14);
            this.chkEnableCPU.TabIndex = 70;
            this.chkEnableCPU.UseVisualStyleBackColor = true;
            this.chkEnableCPU.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableRAM
            // 
            this.chkEnableRAM.AutoSize = true;
            this.chkEnableRAM.Location = new System.Drawing.Point(92, 249);
            this.chkEnableRAM.Name = "chkEnableRAM";
            this.chkEnableRAM.Size = new System.Drawing.Size(15, 14);
            this.chkEnableRAM.TabIndex = 71;
            this.chkEnableRAM.UseVisualStyleBackColor = true;
            this.chkEnableRAM.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableChipset
            // 
            this.chkEnableChipset.AutoSize = true;
            this.chkEnableChipset.Location = new System.Drawing.Point(92, 285);
            this.chkEnableChipset.Name = "chkEnableChipset";
            this.chkEnableChipset.Size = new System.Drawing.Size(15, 14);
            this.chkEnableChipset.TabIndex = 72;
            this.chkEnableChipset.UseVisualStyleBackColor = true;
            this.chkEnableChipset.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableAccel
            // 
            this.chkEnableAccel.AutoSize = true;
            this.chkEnableAccel.Location = new System.Drawing.Point(355, 252);
            this.chkEnableAccel.Name = "chkEnableAccel";
            this.chkEnableAccel.Size = new System.Drawing.Size(15, 14);
            this.chkEnableAccel.TabIndex = 73;
            this.chkEnableAccel.UseVisualStyleBackColor = true;
            this.chkEnableAccel.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableVideo
            // 
            this.chkEnableVideo.AutoSize = true;
            this.chkEnableVideo.Location = new System.Drawing.Point(92, 319);
            this.chkEnableVideo.Name = "chkEnableVideo";
            this.chkEnableVideo.Size = new System.Drawing.Size(15, 14);
            this.chkEnableVideo.TabIndex = 74;
            this.chkEnableVideo.UseVisualStyleBackColor = true;
            this.chkEnableVideo.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableNetwork
            // 
            this.chkEnableNetwork.AutoSize = true;
            this.chkEnableNetwork.Location = new System.Drawing.Point(92, 353);
            this.chkEnableNetwork.Name = "chkEnableNetwork";
            this.chkEnableNetwork.Size = new System.Drawing.Size(15, 14);
            this.chkEnableNetwork.TabIndex = 75;
            this.chkEnableNetwork.UseVisualStyleBackColor = true;
            this.chkEnableNetwork.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableDisk
            // 
            this.chkEnableDisk.AutoSize = true;
            this.chkEnableDisk.Location = new System.Drawing.Point(92, 81);
            this.chkEnableDisk.Name = "chkEnableDisk";
            this.chkEnableDisk.Size = new System.Drawing.Size(15, 14);
            this.chkEnableDisk.TabIndex = 76;
            this.chkEnableDisk.UseVisualStyleBackColor = true;
            this.chkEnableDisk.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableBoot
            // 
            this.chkEnableBoot.AutoSize = true;
            this.chkEnableBoot.Location = new System.Drawing.Point(92, 183);
            this.chkEnableBoot.Name = "chkEnableBoot";
            this.chkEnableBoot.Size = new System.Drawing.Size(15, 14);
            this.chkEnableBoot.TabIndex = 77;
            this.chkEnableBoot.UseVisualStyleBackColor = true;
            this.chkEnableBoot.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableInput
            // 
            this.chkEnableInput.AutoSize = true;
            this.chkEnableInput.Location = new System.Drawing.Point(92, 459);
            this.chkEnableInput.Name = "chkEnableInput";
            this.chkEnableInput.Size = new System.Drawing.Size(15, 14);
            this.chkEnableInput.TabIndex = 78;
            this.chkEnableInput.UseVisualStyleBackColor = true;
            this.chkEnableInput.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableISO
            // 
            this.chkEnableISO.AutoSize = true;
            this.chkEnableISO.Location = new System.Drawing.Point(92, 49);
            this.chkEnableISO.Name = "chkEnableISO";
            this.chkEnableISO.Size = new System.Drawing.Size(15, 14);
            this.chkEnableISO.TabIndex = 79;
            this.chkEnableISO.UseVisualStyleBackColor = true;
            this.chkEnableISO.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // chkEnableSharedFolder
            // 
            this.chkEnableSharedFolder.AutoSize = true;
            this.chkEnableSharedFolder.Location = new System.Drawing.Point(92, 416);
            this.chkEnableSharedFolder.Name = "chkEnableSharedFolder";
            this.chkEnableSharedFolder.Size = new System.Drawing.Size(15, 14);
            this.chkEnableSharedFolder.TabIndex = 80;
            this.chkEnableSharedFolder.UseVisualStyleBackColor = true;
            this.chkEnableSharedFolder.CheckedChanged += new System.EventHandler(this.ChkGroup_CheckedChanged);
            // 
            // txtSharedFolder
            // 
            this.txtSharedFolder.Enabled = false;
            this.txtSharedFolder.Location = new System.Drawing.Point(117, 414);
            this.txtSharedFolder.Name = "txtSharedFolder";
            this.txtSharedFolder.Size = new System.Drawing.Size(319, 20);
            this.txtSharedFolder.TabIndex = 81;
            // 
            // btnBrowseSharedFolder
            // 
            this.btnBrowseSharedFolder.Enabled = false;
            this.btnBrowseSharedFolder.Location = new System.Drawing.Point(467, 412);
            this.btnBrowseSharedFolder.Name = "btnBrowseSharedFolder";
            this.btnBrowseSharedFolder.Size = new System.Drawing.Size(35, 23);
            this.btnBrowseSharedFolder.TabIndex = 82;
            this.btnBrowseSharedFolder.Text = "...";
            this.btnBrowseSharedFolder.UseVisualStyleBackColor = true;
            this.btnBrowseSharedFolder.Click += new System.EventHandler(this.BtnBrowseSharedFolder_Click);
            // 
            // lblSharedFolder
            // 
            this.lblSharedFolder.AutoSize = true;
            this.lblSharedFolder.Location = new System.Drawing.Point(12, 417);
            this.lblSharedFolder.Name = "lblSharedFolder";
            this.lblSharedFolder.Size = new System.Drawing.Size(76, 13);
            this.lblSharedFolder.TabIndex = 83;
            this.lblSharedFolder.Text = "Shared Folder:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 627);
            this.Controls.Add(this.lblSharedFolder);
            this.Controls.Add(this.chkEnableSharedFolder);
            this.Controls.Add(this.txtSharedFolder);
            this.Controls.Add(this.btnBrowseSharedFolder);
            this.Controls.Add(this.chkEnableISO);
            this.Controls.Add(this.chkEnableInput);
            this.Controls.Add(this.chkEnableBoot);
            this.Controls.Add(this.chkEnableDisk);
            this.Controls.Add(this.chkEnableNetwork);
            this.Controls.Add(this.chkEnableVideo);
            this.Controls.Add(this.chkEnableAccel);
            this.Controls.Add(this.chkEnableChipset);
            this.Controls.Add(this.chkEnableRAM);
            this.Controls.Add(this.chkEnableCPU);
            this.Controls.Add(this.lblQemuPath);
            this.Controls.Add(this.txtQemuPath);
            this.Controls.Add(this.btnBrowseQemu);
            this.Controls.Add(this.btnReloadQemu);
            this.Controls.Add(this.lblISO);
            this.Controls.Add(this.txtISO);
            this.Controls.Add(this.btnBrowseISO);
            this.Controls.Add(this.lblDiskType);
            this.Controls.Add(this.cmbDiskType);
            this.Controls.Add(this.pnlPhysicalDrive);
            this.Controls.Add(this.pnlHddImage);
            this.Controls.Add(this.lblBootOrder);
            this.Controls.Add(this.cmbBootOrder);
            this.Controls.Add(this.lblCPU);
            this.Controls.Add(this.cmbCPU);
            this.Controls.Add(this.lblCores);
            this.Controls.Add(this.cmbCores);
            this.Controls.Add(this.lblRAM);
            this.Controls.Add(this.cmbRAM);
            this.Controls.Add(this.lblAccel);
            this.Controls.Add(this.cmbAccelerator);
            this.Controls.Add(this.lblChipset);
            this.Controls.Add(this.cmbChipset);
            this.Controls.Add(this.chkAudio);
            this.Controls.Add(this.cmbAudioBackend);
            this.Controls.Add(this.lblVideo);
            this.Controls.Add(this.cmbVideo);
            this.Controls.Add(this.lblNetwork);
            this.Controls.Add(this.cmbNetwork);
            this.Controls.Add(this.lblMAC);
            this.Controls.Add(this.txtMAC);
            this.Controls.Add(this.btnGenerateMAC);
            this.Controls.Add(this.lblNetworkModel);
            this.Controls.Add(this.cmbNetworkModel);
            this.Controls.Add(this.lblKeyboard);
            this.Controls.Add(this.cmbKeyboard);
            this.Controls.Add(this.lblMouse);
            this.Controls.Add(this.cmbMouse);
            this.Controls.Add(this.lblUsbController);
            this.Controls.Add(this.cmbUsbController);
            this.Controls.Add(this.lblRes);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.lblAuto);
            this.Controls.Add(this.lblExtraArgs);
            this.Controls.Add(this.txtExtraArgs);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QEMU Launcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlPhysicalDrive.ResumeLayout(false);
            this.pnlHddImage.ResumeLayout(false);
            this.pnlHddImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtQemuPath;
        private System.Windows.Forms.Button btnBrowseQemu;
        private System.Windows.Forms.Button btnReloadQemu;
        private System.Windows.Forms.TextBox txtISO;
        private System.Windows.Forms.Button btnBrowseISO;
        private System.Windows.Forms.ComboBox cmbDiskType;
        private System.Windows.Forms.Panel pnlPhysicalDrive;
        private System.Windows.Forms.ComboBox cmbPhysicalDrive;
        private System.Windows.Forms.Button btnRefreshDrives;
        private System.Windows.Forms.Button btnSetOffline;
        private System.Windows.Forms.Panel pnlHddImage;
        private System.Windows.Forms.TextBox txtHDD;
        private System.Windows.Forms.Button btnBrowseHDD;
        private System.Windows.Forms.Button btnCreateHDD;
        private System.Windows.Forms.ComboBox cmbHddController;
        private System.Windows.Forms.ComboBox cmbBootOrder;
        private System.Windows.Forms.ComboBox cmbCPU;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.ComboBox cmbRAM;
        private System.Windows.Forms.ComboBox cmbAccelerator;
        private System.Windows.Forms.ComboBox cmbChipset;
        private System.Windows.Forms.CheckBox chkAudio;
        private System.Windows.Forms.ComboBox cmbAudioBackend;
        private System.Windows.Forms.ComboBox cmbVideo;
        private System.Windows.Forms.ComboBox cmbNetwork;
        private System.Windows.Forms.TextBox txtMAC;
        private System.Windows.Forms.Button btnGenerateMAC;
        private System.Windows.Forms.ComboBox cmbNetworkModel;
        private System.Windows.Forms.ComboBox cmbKeyboard;
        private System.Windows.Forms.ComboBox cmbMouse;
        private System.Windows.Forms.ComboBox cmbUsbController;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.TextBox txtExtraArgs;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Label lblPhys;
        private System.Windows.Forms.Label lblHdd;
        private System.Windows.Forms.Label lblQemuPath;
        private System.Windows.Forms.Label lblISO;
        private System.Windows.Forms.Label lblDiskType;
        private System.Windows.Forms.Label lblHddController;
        private System.Windows.Forms.Label lblBootOrder;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label lblCores;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.Label lblAccel;
        private System.Windows.Forms.Label lblChipset;
        private System.Windows.Forms.Label lblVideo;
        private System.Windows.Forms.Label lblNetwork;
        private System.Windows.Forms.Label lblMAC;
        private System.Windows.Forms.Label lblNetworkModel;
        private System.Windows.Forms.Label lblKeyboard;
        private System.Windows.Forms.Label lblMouse;
        private System.Windows.Forms.Label lblUsbController;
        private System.Windows.Forms.Label lblRes;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblAuto;
        private System.Windows.Forms.Label lblExtraArgs;
        private System.Windows.Forms.CheckBox chkEnableCPU;
        private System.Windows.Forms.CheckBox chkEnableRAM;
        private System.Windows.Forms.CheckBox chkEnableChipset;
        private System.Windows.Forms.CheckBox chkEnableAccel;
        private System.Windows.Forms.CheckBox chkEnableVideo;
        private System.Windows.Forms.CheckBox chkEnableNetwork;
        private System.Windows.Forms.CheckBox chkEnableDisk;
        private System.Windows.Forms.CheckBox chkEnableBoot;
        private System.Windows.Forms.CheckBox chkEnableInput;
        private System.Windows.Forms.CheckBox chkEnableISO;
        private System.Windows.Forms.CheckBox chkEnableSharedFolder;
        private System.Windows.Forms.TextBox txtSharedFolder;
        private System.Windows.Forms.Button btnBrowseSharedFolder;
        private System.Windows.Forms.Label lblSharedFolder;
    }
}