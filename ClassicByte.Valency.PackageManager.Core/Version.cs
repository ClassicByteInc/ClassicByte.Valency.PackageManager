#nullable disable
namespace ClassicByte.Valency.PackageManager.Core;

using System;
/// <summary>
/// 表示一个四段式的包版本号（主版本号.次版本号.构建号.修订号）。
/// 支持比较、相等性判断和字符串隐式转换。
/// </summary>
public class Version : IComparable<Version>
{
	/// <summary>
	/// 获取主版本号。
	/// </summary>
	public int Major { get; }

	/// <summary>
	/// 获取次版本号。
	/// </summary>
	public int Minor { get; }

	/// <summary>
	/// 获取构建号。
	/// </summary>
	public int Build { get; }

	/// <summary>
	/// 获取修订号。
	/// </summary>
	public int Revision { get; }

	/// <summary>
	/// 初始化 <see cref="Version"/> 类的新实例。
	/// </summary>
	/// <param name="major">主版本号。</param>
	/// <param name="minor">次版本号。</param>
	/// <param name="build">构建号。</param>
	/// <param name="revision">修订号。</param>
	public Version(int major, int minor, int build, int revision)
	{
		Major = major;
		Minor = minor;
		Build = build;
		Revision = revision;
	}

	/// <summary>
	/// 比较当前版本与另一个版本的大小。
	/// </summary>
	/// <param name="other">要比较的另一个版本。</param>
	/// <returns>小于0表示小于，0表示等于，大于0表示大于。</returns>
	/// <exception cref="ArgumentNullException">other为null时抛出。</exception>
	public int CompareTo(Version other)
	{
		if (other == null)
		{
			throw new ArgumentNullException(nameof(other));
		}

		if (Major != other.Major)
		{
			return Major.CompareTo(other.Major);
		}
		if (Minor != other.Minor)
		{
			return Minor.CompareTo(other.Minor);
		}
		if (Build != other.Build)
		{
			return Build.CompareTo(other.Build);
		}
		return Revision.CompareTo(other.Revision);
	}

	/// <summary>
	/// 返回版本号的字符串表示形式（如"1.2.3.4"）。
	/// </summary>
	public override string ToString()
	{
		return $"{Major}.{Minor}.{Build}.{Revision}";
	}

	/// <summary>
	/// 判断当前版本是否等于另一个对象。
	/// </summary>
	/// <param name="obj">要比较的对象。</param>
	/// <returns>如果相等返回true，否则返回false。</returns>
	public override bool Equals(object obj)
	{
		if (obj is Version other)
		{
			return Major == other.Major && Minor == other.Minor && Build == other.Build && Revision == other.Revision;
		}
		return false;
	}

	/// <summary>
	/// 获取当前版本的哈希码。
	/// </summary>
	public override int GetHashCode()
	{
		return HashCode.Combine(Major, Minor, Build, Revision);
	}

	/// <summary>
	/// 判断两个版本是否相等。
	/// </summary>
	public static bool operator ==(Version v1, Version v2)
	{
		if (ReferenceEquals(v1, v2))
		{
			return true;
		}

		if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
		{
			return false;
		}

		return v1.Equals(v2);
	}

	/// <summary>
	/// 判断两个版本是否不相等。
	/// </summary>
	public static bool operator !=(Version v1, Version v2)
	{
		return !(v1 == v2);
	}

	/// <summary>
	/// 判断左侧版本是否大于右侧版本。
	/// </summary>
	public static bool operator >(Version v1, Version v2)
	{
		if (v1 is null || v2 is null)
		{
			throw new ArgumentNullException(v1 is null ? nameof(v1) : nameof(v2));
		}

		return v1.CompareTo(v2) > 0;
	}

	/// <summary>
	/// 判断左侧版本是否小于右侧版本。
	/// </summary>
	public static bool operator <(Version v1, Version v2)
	{
		if (v1 is null || v2 is null)
		{
			throw new ArgumentNullException(v1 is null ? nameof(v1) : nameof(v2));
		}

		return v1.CompareTo(v2) < 0;
	}

	/// <summary>
	/// 判断左侧版本是否大于等于右侧版本。
	/// </summary>
	public static bool operator >=(Version v1, Version v2)
	{
		return !(v1 < v2);
	}

	/// <summary>
	/// 判断左侧版本是否小于等于右侧版本。
	/// </summary>
	public static bool operator <=(Version v1, Version v2)
	{
		return !(v1 > v2);
	}

	/// <summary>
	/// 支持从字符串（如"1.2.3.4"）隐式转换为<see cref="Version"/>对象。
	/// </summary>
	/// <param name="str">版本号字符串。</param>
	/// <returns>转换后的<see cref="Version"/>对象。</returns>
	/// <exception cref="ArgumentException">格式不正确时抛出。</exception>
	public static implicit operator Version(string str)
	{
		var nums = str.Split('.');
		if (nums.Length != 4)
		{
			throw new ArgumentException("版本号格式不正确");
		}
		return new(
			int.Parse(nums[0]), int.Parse(nums[1]), int.Parse(nums[2]), int.Parse(nums[3])
		);
	}
}
