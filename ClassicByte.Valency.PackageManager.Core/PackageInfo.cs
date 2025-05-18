using System;

namespace ClassicByte.Valency.PackageManager.Core;

/// <summary>
/// 表示一个包的基本信息，包括ID、版本、描述、作者和许可证等。
/// </summary>
public class PackageInfo
{
    /// <summary>
    /// 包的唯一标识符（ID）。
    /// </summary>
    public required string ID { get; set; }

    /// <summary>
    /// 包的版本号。
    /// </summary>
    public required Version Version { get; set; }

    /// <summary>
    /// 包的简要描述信息。
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// 包的作者或维护者。
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// 包的许可证类型（如 MIT、GPL 等）。
    /// </summary>
    public required string License { get; set; }

    /// <summary>
    /// 包的许可证全文内容。
    /// </summary>
    public required string LicenseContent { get; set; }
}
