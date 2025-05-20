namespace ClassicByte.Valency.PackageManager.Core;

/// <summary>
/// ��ʾһ�����Ļ�����Ϣ������ID���汾�����������ߡ����֤�ȡ�
/// </summary>
public class PackageInfo
{
	/// <summary>
	/// ����Ψһ��ʶ����ID����
	/// </summary>
	public required string ID { get; set; }

	/// <summary>
	/// ���İ汾�š�
	/// </summary>
	public required Version Version { get; set; }

	/// <summary>
	/// ���ļ�Ҫ������Ϣ��
	/// </summary>
	public required string Description { get; set; }

	/// <summary>
	/// �������߻�ά���ߡ�
	/// </summary>
	public required string Author { get; set; }

	/// <summary>
	/// �������֤���ͣ��� MIT��GPL �ȣ���
	/// </summary>
	public required string License { get; set; }

	/// <summary>
	/// �������֤ȫ�����ݡ�
	/// </summary>
	public required string LicenseContent { get; set; }

	/// <summary>
	/// ����֧�ֵĲ���ϵͳƽ̨���� Windows��Linux��MacOS �ȣ���
	/// </summary>
	public required Platform Platform { get; set; }
}

[Flags]
/// <summary>
/// ָ��Ӧ�ó��������֧�ֵ�ƽ̨��
/// </summary>
/// <remarks>ʹ�ô�ö����ָʾӦ�ó������֧�ֵ�ƽ̨����ֵ���Ա�ʾ����ƽ̨��Ҳ���Ա�ʾ����ƽ̨��</remarks>
public enum Platform
{
	/// <summary>
	/// Windowsƽ̨��
	/// </summary>
	Windows,

	/// <summary>
	/// Linuxƽ̨
	/// </summary>
	Linux,

	/// <summary>
	/// MacOsƽ̨
	/// </summary>
	MacOS,

	/// <summary>
	/// ���п�֧�ֵ�ƽ̨��
	/// </summary>
	All
}
