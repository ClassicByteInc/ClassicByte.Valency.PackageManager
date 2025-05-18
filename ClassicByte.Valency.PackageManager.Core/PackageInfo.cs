using System;

namespace ClassicByte.Valency.PackageManager.Core;

/// <summary>
/// ��ʾһ�����Ļ�����Ϣ������ID���汾�����������ߺ����֤�ȡ�
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
}
