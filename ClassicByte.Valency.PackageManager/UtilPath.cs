namespace ClassicByte.Valency.PackageManager;

public static class UtilPath
{
	public static DirectoryInfo Workspace
	{
		get
		{
			return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".Valency", "vpm"));
		}
	}

	public static DirectoryInfo ConfigDir => Directory.CreateDirectory(Path.Combine(Workspace.FullName, "config"));

	public static DirectoryInfo PackagesDir => Directory.CreateDirectory(Path.Combine(Workspace.FullName, "packages"));

	public static FileInfo AppConfig => new FileInfo(Path.Combine(ConfigDir.FullName, "app.config"));

	public static FileInfo SourceList => new FileInfo(Path.Combine(ConfigDir.FullName, "sources.xml"));

	public static FileInfo PackageList => new FileInfo(Path.Combine(ConfigDir.FullName, "packages.xml"));

	public static DirectoryInfo ShortcutCommand => Directory.CreateDirectory(Path.Combine(Workspace.FullName, "applink"));
}
