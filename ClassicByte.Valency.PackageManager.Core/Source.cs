namespace ClassicByte.Valency.PackageManager.Core;


/// <summary>
/// 表示一个包源。
/// </summary>
[Serializable]
public class Source
{

	/// <summary>
	/// 源的名字
	/// </summary>
	public string Name { get; set; } = "";

	// 结构：包ID -> (版本 -> URL)
	/// <summary>
	/// 内容
	/// </summary>
	public Dictionary<string, Dictionary<Version, string>> Content { get; set; } = [];

	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 获取指定包ID下所有版本及其对应的URL。
	/// </summary>
	/// <param name="id">包ID</param>
	/// <returns>版本与URL的字典。如果没有该ID，返回空。</returns>
	public Dictionary<Version, string>? GetVersionsWithUrls(string id)
	{
		if (Content.TryGetValue(id, out var versions))
		{
			return versions;
		}
		return null;
	}

	/// <summary>
	/// 获取指定包ID和版本的URL。
	/// </summary>
	/// <param name="id">包id</param>
	/// <param name="version">包的版本（默认为null）</param>
	/// <returns>没指定版本就返回指定id的最新版url，或者返回指定的版本的url。如果id不存在，返回空。</returns>
	public string? GetUrlByVersion(string id, Version? version = null)
	{
		if (Content.TryGetValue(id, out var versions) && versions.Count > 0)
		{
			if (version is null)
			{
				// 获取最新版本（最大值）
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
