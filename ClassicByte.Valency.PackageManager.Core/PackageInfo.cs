namespace ClassicByte.Valency.PackageManager.Core;

/// <summary>
/// 表示一个包的基本信息，包括ID、版本、描述、作者、许可证等。
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

	/// <summary>
	/// 包所支持的操作系统平台（如 Windows、Linux、MacOS 等）。
	/// </summary>
	public required Platform Platform { get; set; }
}

[Flags]
/// <summary>
/// 指定应用程序或功能所支持的平台。
/// </summary>
/// <remarks>使用此枚举来指示应用程序或功能支持的平台。该值可以表示单个平台，也可以表示所有平台。</remarks>
public enum Platform
{
	/// <summary>
	/// Windows平台。
	/// </summary>
	Windows,

	/// <summary>
	/// Linux平台
	/// </summary>
	Linux,

	/// <summary>
	/// MacOs平台
	/// </summary>
	MacOS,

	/// <summary>
	/// 所有可支持的平台。
	/// </summary>
	All
}
