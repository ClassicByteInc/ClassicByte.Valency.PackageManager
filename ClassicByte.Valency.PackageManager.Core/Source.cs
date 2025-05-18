using System;

namespace ClassicByte.Valency.PackageManager.Core;


/// <summary>
/// ��ʾһ����Դ��
/// </summary>
[Serializable]
public class Source
{

	/// <summary>
	/// Դ������
	/// </summary>
	public string Name { get; set; } = "";

	// �ṹ����ID -> (�汾 -> URL)
	/// <summary>
	/// ����
	/// </summary>
	public Dictionary<string, Dictionary<Version, string>> Content { get; set; } = [];

	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// ��ȡָ����ID�����а汾�����Ӧ��URL��
	/// </summary>
	/// <param name="id">��ID</param>
	/// <returns>�汾��URL���ֵ䡣���û�и�ID�����ؿա�</returns>
	public Dictionary<Version, string>? GetVersionsWithUrls(string id)
	{
		if (Content.TryGetValue(id, out var versions))
		{
			return versions;
		}
		return null;
	}

	/// <summary>
	/// ��ȡָ����ID�Ͱ汾��URL��
	/// </summary>
	/// <param name="id">��id</param>
	/// <param name="version">���İ汾��Ĭ��Ϊnull��</param>
	/// <returns>ûָ���汾�ͷ���ָ��id�����°�url�����߷���ָ���İ汾��url�����id�����ڣ����ؿա�</returns>
	public string? GetUrlByVersion(string id, Version? version = null)
	{
		if (Content.TryGetValue(id, out var versions) && versions.Count > 0)
		{
			if (version is null)
			{
				// ��ȡ���°汾�����ֵ��
				var latest = versions.Keys.Max();
				return versions[latest];
			}
			else if (versions.TryGetValue(version, out var url))
			{
				return url;
			}
		}
		return null;
	}
}
