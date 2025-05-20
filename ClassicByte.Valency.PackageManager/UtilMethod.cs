using System.IO.Compression;
using System.Xml.Serialization;

namespace ClassicByte.Valency.PackageManager;

public static class UtilMethod
{
	public static void SerializeObjectToXml(object obj, string filePath)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj));
		}

		XmlSerializer serializer = new(obj.GetType());
		using StreamWriter writer = new(filePath);
		serializer.Serialize(writer, obj);
	}

	public static T DeserializeXmlToObject<T>(string filePath) where T : class
	{
		if (string.IsNullOrEmpty(filePath))
		{
			throw new ArgumentException("文件路径不能为空", nameof(filePath));
		}

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("指定的 XML 文件不存在", filePath);
		}

		XmlSerializer serializer = new(typeof(T));

		using StreamReader reader = new(filePath);
		if (serializer.Deserialize(reader) is not T deserializedObject)
		{
			throw new InvalidDataException("XML 文件内容无法反序列化为指定的 .NET 类型");
		}

		return deserializedObject;
	}

	public static void Unzip(string zipFile, string savePath)
	{
		if (string.IsNullOrEmpty(zipFile))
			throw new ArgumentException("压缩文件路径不能为空", nameof(zipFile));
		if (string.IsNullOrEmpty(savePath))
			throw new ArgumentException("解压目标路径不能为空", nameof(savePath));
		if (!File.Exists(zipFile))
			throw new FileNotFoundException("指定的压缩文件不存在", zipFile);

		Directory.CreateDirectory(savePath);

		using var archive = System.IO.Compression.ZipFile.OpenRead(zipFile);
		foreach (var entry in archive.Entries)
		{
			string destinationPath = Path.Combine(savePath, entry.FullName);
			if (string.IsNullOrEmpty(entry.Name))
			{
				// 目录
				Directory.CreateDirectory(destinationPath);
			}
			else
			{
				Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
				entry.ExtractToFile(destinationPath, overwrite: true);
			}
		}
	}

	public static void Zip(string sourceDir, string zipFile)
	{
		if (string.IsNullOrEmpty(sourceDir))
			throw new ArgumentException("源目录路径不能为空", nameof(sourceDir));
		if (string.IsNullOrEmpty(zipFile))
			throw new ArgumentException("压缩文件路径不能为空", nameof(zipFile));
		if (!Directory.Exists(sourceDir))
			throw new DirectoryNotFoundException("指定的源目录不存在");
		using var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create);
		foreach (var file in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
		{
			string entryName = Path.GetRelativePath(sourceDir, file);
			archive.CreateEntryFromFile(file, entryName);
		}
	}

	public static void CreateCliCommand(string exeFileName)
	{
		switch (Environment.OSVersion.Platform)
		{
			case PlatformID.Win32NT:
				// Windows
				File.WriteAllText
					(
					Path.Combine
					(
						UtilPath.ShortcutCommand.FullName,
						new FileInfo(exeFileName).FullName
						.Replace
						(
							new FileInfo(exeFileName).Extension,
							""
						)
						,".bat"
					)
					,new FileInfo(exeFileName).FullName
					, System.Text.Encoding.UTF8
					);
				break;
			case PlatformID.Unix:
				break;
			case PlatformID.MacOSX:
				break;
			default:
				break;
		}
	}
}
