#nullable disable
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Diagnostics;
using Version = ClassicByte.Valency.PackageManager.Core.Version;

namespace ClassicByte.Valency.PackageManager;

public class MainCore
{
	private static Version AppVersion => "1.0.0.0";
	private static TextWriter Out { get; set; } = Console.Out;

	public static readonly Option<string> IdOption = new(["--id", "-i"], "当前操作所指定的包的ID");
	public static readonly Option<string> NameOption = new(["--name", "-n"], "当前操作所指定的包的名字或者有关的字符串");
	public static readonly Option<string> VersionOption = new(["--version", "-v"], "当前操作所指定的包的版本");
	public static readonly Option<string> SourceOption = new(["--source", "-s"], () => "", "当前的操作所指定的包的来源");
	public static readonly Option<bool> VerboseFlagOption = new(["--verbose"], "当前操作是否详细输出的值");
	public static readonly Option<string> PackageFileUriOption = new(["--package", "-p"], "当前操作所指定的包文件的uri");
	public static readonly Option<string> HashOption = new(["--hash"], "当前某个操作需要的hash值");
	public static readonly Option<bool> HelpFlagOption = new(new[] { "--help" }, "输出当前某个操作的帮助信息");
	public static readonly Option<string> DirectoryExOption = new(new[] { "--directory", "-d" }, "当前某个操作要用的目录");
	public static readonly Option<bool> ShowLogoFlagOption = new(new[] { "--showlogo" }, () => true, "是否显示Logo");

	public static List<string> Args { get; set; }

	public MainCore(List<string> args)
	{
		Args = args;
	}

	public int Run()
	{
		InitializeApp();

		if (Args.Count == 0)
		{
			Help();
			return 1;
		}

		// 构建命令树
		var rootCommand = new RootCommand("Valency Package Manager");

		// install
		var installCmd = new Command("install", "安装给定的程序包")
		{
			IdOption, NameOption, VersionOption, SourceOption, VerboseFlagOption, PackageFileUriOption
		};
		installCmd.SetHandler((string id, string name, string version, string source, bool verbose, string package) =>
		{
			Install(id, name, version, source, verbose, package);
		}, IdOption, NameOption, VersionOption, SourceOption, VerboseFlagOption, PackageFileUriOption);

		// uninstall
		var uninstallCmd = new Command("uninstall", "卸载指定的程序包")
		{
			IdOption, NameOption, VersionOption, SourceOption, VerboseFlagOption
		};
		uninstallCmd.SetHandler((string id, string name, string version, string source, bool verbose) =>
		{
			Uninstall(id, name, version, source, verbose);
		}, IdOption, NameOption, VersionOption, SourceOption, VerboseFlagOption);

		// list
		var listCmd = new Command("list", "显示本地已经安装过的包");
		listCmd.SetHandler(ListPackage);

		// show
		var showCmd = new Command("show", "显示包的相关信息")
		{
			IdOption, NameOption, SourceOption, VersionOption
		};
		showCmd.SetHandler((string id, string name, string source, string version) =>
		{
			ShowPackage(id, name, source, version);
		}, IdOption, NameOption, SourceOption, VersionOption);

		// search
		var searchCmd = new Command("search", "在当前源中尝试查找包")
		{
			NameOption, SourceOption
		};
		searchCmd.SetHandler((string name, string source) =>
		{
			Search(name, source);
		}, NameOption, SourceOption);

		// update
		var updateCmd = new Command("update", "显示可更新的包");
		updateCmd.SetHandler(Update);

		// build
		var buildCmd = new Command("build", "构造一个 Valency 应用程序包");
		buildCmd.SetHandler(Build);

		// download
		var downloadCmd = new Command("download", "下载指定包")
		{
			PackageFileUriOption, DirectoryExOption
		};
		downloadCmd.SetHandler((string package, string directory) =>
		{
			Download(package, directory);
		}, PackageFileUriOption, DirectoryExOption);

		// verifyHash
		var verifyHashCmd = new Command("verifyHash", "验证文件Hash")
		{
			HashOption, PackageFileUriOption
		};
		verifyHashCmd.SetHandler((string hash, string package) =>
		{
			VerifyHash(hash, package);
		}, HashOption, PackageFileUriOption);

		// help
		var helpCmd = new Command("help", "显示帮助信息");
		helpCmd.SetHandler(Help);

		// 注册所有命令
		rootCommand.AddCommand(installCmd);
		rootCommand.AddCommand(uninstallCmd);
		rootCommand.AddCommand(listCmd);
		rootCommand.AddCommand(showCmd);
		rootCommand.AddCommand(searchCmd);
		rootCommand.AddCommand(updateCmd);
		rootCommand.AddCommand(buildCmd);
		rootCommand.AddCommand(downloadCmd);
		rootCommand.AddCommand(verifyHashCmd);
		rootCommand.AddCommand(helpCmd);

		// 解析并执行
		var parser = new CommandLineBuilder(rootCommand)
			.UseDefaults()
			.Build();

		return parser.Invoke([.. Args]);
	}

	public void InitializeApp()
	{
		ShowLogo();
		InitializeSource();
	}

	public void InitializeSource()
	{
		// TODO: 加载/初始化源
	}

	public void ShowLogo()
	{
		Out.WriteLine($"Valency Package Manager 程序管理器 版本：{AppVersion}");
		Out.WriteLine("版权所有 (C) ClassicByte org. 保留所有权利。\n");
		Out.WriteLine("可使用此命令行工具来安装程序、工具等 Valency 应用程序包。用户也可以使用此命令行工具来构造自己的Valency应用程序包(*.vpkg)");
	}

	// 下面是各命令的实现（这里只做演示，具体逻辑请根据实际需求完善）

	public void Install(string id, string name, string version, string source, bool verbose, string package)
	{
		Debug.WriteLine($"[安装] id={id}, name={name}, version={version}, source={source}, verbose={verbose}, package={package}");
		if (!string.IsNullOrWhiteSpace(id))
		{

		}
		else if (!string.IsNullOrWhiteSpace(name))
		{

		}
		else if (!string.IsNullOrWhiteSpace(package))
		{

		}
		else if (!string.IsNullOrWhiteSpace(Args[1]))
		{
			if (File.Exists(Args[1]))
			{
				installFormFile(Args[1]);
			}
			else if (Directory.Exists(Args[1]))
			{
				installFormDir(Args[1]);
			}
		}
		else
		{
			// TODO: 实现安装逻辑
			Out.WriteLine("用法: vpm install [选项]");
			Out.WriteLine("选项:");
			Out.WriteLine("  --id, -i         指定要安装的包的ID");
			Out.WriteLine("  --name, -n       指定要安装的包的名字或相关字符串");
			Out.WriteLine("  --version, -v    指定要安装的包的版本");
			Out.WriteLine("  --source, -s     指定包的来源（可选）");
			Out.WriteLine("  --verbose        详细输出安装过程");
			Out.WriteLine("  --package, -p    指定包文件的URI（本地或远程）");
			Out.WriteLine();
			Out.WriteLine("示例:");
			Out.WriteLine("  vpm install --id MyPackage --version 1.0.0.0");
			Out.WriteLine("  vpm install --name \"工具包\" --source default");
			Out.WriteLine("  vpm install --package ./myapp.vpkg");
		}

		void installFormFile(string fileName)
		{
			Debug.WriteLine($"开始从文件安装包：{fileName}");
			var target = new FileInfo(fileName);
			switch (target.Extension.ToLower())
			{
				case ".vpkg":
					throw new NotImplementedException("安装vpkg包的逻辑未实现。");
				case ".exe":
					Out.WriteLine($"检测到目标文件为 Windows 可执行文件。\n即将将‘{fileName}’复制到‘{UtilPath.PackagesDir}’下并创建新文件夹：‘{target.Name.Replace(target.Extension, "")}’以存储其产生的文件。");
					File.Copy(fileName, Path.Combine(UtilPath.PackagesDir.FullName, target.Name.Replace(target.Extension, "")), true);

					break;
				case ".zip":
					break;
				default:
					break;
			}
		}

		void installFormDir(string dirName)
		{

		}
	}

	public void Uninstall(string id, string name, string version, string source, bool verbose)
	{
		Out.WriteLine($"[卸载] id={id}, name={name}, version={version}, source={source}, verbose={verbose}");
		// TODO: 实现卸载逻辑
	}

	public void ListPackage()
	{
		Out.WriteLine("[列出本地包]");
		// TODO: 实现本地包列表逻辑
	}

	public void ShowPackage(string id, string name, string source, string version)
	{
		Out.WriteLine($"[显示包信息] id={id}, name={name}, source={source}, version={version}");
		// TODO: 实现包信息显示逻辑
	}

	public void Search(string name, string source)
	{
		Out.WriteLine($"[搜索] name={name}, source={source}");
		// TODO: 实现搜索逻辑
	}

	public void Update()
	{
		Out.WriteLine("[更新]");
		// TODO: 实现更新逻辑
	}

	public void Build()
	{
		Out.WriteLine("[构建包]");
		// TODO: 实现包构建逻辑
	}

	public void Download(string package, string directory)
	{
		Out.WriteLine($"[下载] package={package}, directory={directory}");
		// TODO: 实现下载逻辑
	}

	public void VerifyHash(string hash, string package)
	{
		Out.WriteLine($"[校验Hash] hash={hash}, package={package}");
		// TODO: 实现Hash校验逻辑
	}

	public void Help()
	{
		Out.WriteLine("用法：vpm  [<命令>] [<选项>]");
		Out.WriteLine(@"下列命令有效：
    install 安装给定的程序包
    show    显示包的相关信息
    source  管理包的来源
    search  在当前源中尝试查找包
    list    显示本地已经安装过的包
    update  显示可更新的包
    build   构造一个 Valency 应用程序包（有关文档请向其传递帮助参数“--help”）
    download 下载指定包
    verifyHash 验证文件Hash
    help    显示帮助信息
");
	}


}