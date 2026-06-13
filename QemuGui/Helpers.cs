namespace QemuLauncher
{
    public class PhysicalDriveInfo
    {
        public int Number { get; set; }
        public double SizeGB { get; set; }
        public string Status { get; set; }
        public PhysicalDriveInfo(int number, double sizeGB, string status)
        {
            Number = number;
            SizeGB = sizeGB;
            Status = status;
        }
    }

    public class AppSettings
    {
        public string QemuPath { get; set; }
        public string ISO { get; set; }
        public string DiskType { get; set; }
        public string PhysicalDriveDisplay { get; set; }
        public string HDDImage { get; set; }
        public string CPU { get; set; }
        public string Cores { get; set; }
        public string RAM { get; set; }
        public string Chipset { get; set; }
        public bool Audio { get; set; }
        public string Video { get; set; }
        public string Network { get; set; }
        public string NetworkModel { get; set; }
        public string MAC { get; set; }
        public decimal WindowWidth { get; set; }
        public decimal WindowHeight { get; set; }
        public string AudioBackend { get; set; }
        public int AcceleratorIndex { get; set; }
        public string ExtraArgs { get; set; }
        public int BootOrderIndex { get; set; }
        public int HddControllerIndex { get; set; }
        public string KeyboardType { get; set; }
        public string MouseType { get; set; }
        public string UsbController { get; set; }
        public bool EnableCPU { get; set; } = true;
        public bool EnableRAM { get; set; } = true;
        public bool EnableChipset { get; set; } = true;
        public bool EnableAccel { get; set; } = true;
        public bool EnableVideo { get; set; } = true;
        public bool EnableNetwork { get; set; } = true;
        public bool EnableDisk { get; set; } = true;
        public bool EnableBoot { get; set; } = true;
        public bool EnableInput { get; set; } = true;
        public bool EnableISO { get; set; } = true;
        public bool EnableSharedFolder { get; set; }
        public string SharedFolderPath { get; set; }
    }
}
