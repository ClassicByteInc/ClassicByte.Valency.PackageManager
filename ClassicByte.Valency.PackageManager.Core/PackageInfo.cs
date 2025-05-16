using System;

namespace ClassicByte.Valency.PackageManager.Core;

public class PackageInfo
{
    public required String ID { get; set; }

    public required Version Version { get; set; }

    public required String Description { get; set; }

    public required String Author { get; set; }

    public required String License { get; set; }

    public required String LicenseContent { get; set; }
}
