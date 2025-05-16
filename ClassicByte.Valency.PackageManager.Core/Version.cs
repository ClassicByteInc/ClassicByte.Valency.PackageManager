#nullable disable
namespace ClassicByte.Valency.PackageManager.Core;

using System;
public class Version : IComparable<Version>
{
    // 定义四个属性，分别表示版本号的四个部分
    public int Major { get; }
    public int Minor { get; }
    public int Build { get; }
    public int Revision { get; }

    // 构造函数，用于初始化版本号
    public Version(int major, int minor, int build, int revision)
    {
        Major = major;
        Minor = minor;
        Build = build;
        Revision = revision;
    }

    // 实现 IComparable<Version> 接口的 CompareTo 方法
    public int CompareTo(Version other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        // 首先比较 Major 部分
        if (Major != other.Major)
        {
            return Major.CompareTo(other.Major);
        }

        // 如果 Major 相等，再比较 Minor 部分
        if (Minor != other.Minor)
        {
            return Minor.CompareTo(other.Minor);
        }

        // 如果 Minor 也相等，再比较 Build 部分
        if (Build != other.Build)
        {
            return Build.CompareTo(other.Build);
        }

        // 如果 Build 也相等，最后比较 Revision 部分
        return Revision.CompareTo(other.Revision);
    }

    // 重写 ToString 方法，方便输出版本号
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}.{Revision}";
    }

    // 重写 Equals 方法
    public override bool Equals(object obj)
    {
        if (obj is Version other)
        {
            return Major == other.Major && Minor == other.Minor && Build == other.Build && Revision == other.Revision;
        }
        return false;
    }

    // 重写 GetHashCode 方法
    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Build, Revision);
    }

    // 重载 == 运算符
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

    // 重载 != 运算符
    public static bool operator !=(Version v1, Version v2)
    {
        return !(v1 == v2);
    }

    // 重载 > 运算符
    public static bool operator >(Version v1, Version v2)
    {
        if (v1 is null || v2 is null)
        {
            throw new ArgumentNullException(v1 is null ? nameof(v1) : nameof(v2));
        }

        return v1.CompareTo(v2) > 0;
    }

    // 重载 < 运算符
    public static bool operator <(Version v1, Version v2)
    {
        if (v1 is null || v2 is null)
        {
            throw new ArgumentNullException(v1 is null ? nameof(v1) : nameof(v2));
        }

        return v1.CompareTo(v2) < 0;
    }

    // 重载 >= 运算符
    public static bool operator >=(Version v1, Version v2)
    {
        return !(v1 < v2);
    }

    // 重载 <= 运算符
    public static bool operator <=(Version v1, Version v2)
    {
        return !(v1 > v2);
    }

    public static implicit operator Version(string str)
    {
        var nums = str.Split('.');
        if (nums.Length != 4)
        {
            throw new ArgumentException("版本号格式不正确");
        }
        return new(
            int.Parse(nums[0]), int.Parse(nums[1]), int.Parse(nums[2]), int.Parse(nums[3])
        ); ;
    }
}
