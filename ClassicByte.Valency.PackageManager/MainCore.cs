#nullable disable
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using Version = ClassicByte.Valency.PackageManager.Core.Version;
namespace ClassicByte.Valency.PackageManager;

public class MainCore
{

    private static Version AppVersion => "1.0.0.0"; // 这里是版本号    
    private static TextWriter Out { get; set; } = Console.Out;

    public MainCore(List<string> args)
    {
        Args = args;
    }
    public static List<string> Args { get; set; }
    /**
        这是用来看给进来的开关的值的。
        静态公有是因为没有思路如何让每个操作都能够使用他们想要的参数。
        以后可能会改，但是目前来说，我只会这么写。
        */
    #region 

    ///<summary>
    /// 当前操作所指定的包的ID
    /// </summary>
    public static string ID { get; set; } = "";

    /// <summary>
    /// 当前操作所指定的包的名字或者有关的字符串
    /// </summary>
    public static string Name { get; set; } = "";

    /// <summary>
    /// 当前操作所指定的包的版本
    /// </summary>
    public static string Version { get; set; } = "";
    /// <summary>
    /// 当前的操作所指定的包的来源
    /// </summary>
    public static string Source { get; set; } = "";

    /// <summary>
    /// 当前操作是否详细输出的值
    /// </summary>
    public static bool VerboseFlag { get; set; } = false;

    /// <summary>
    /// 当前操作所指定的包文件的uri。
    /// </summary>
    public string PackageFileUri { get; set; } = "";

    /// <summary>
    /// 当前某个操作需要的hash值。
    /// </summary>
    public static string Hash { get; set; } = "";

    /// <summary>
    /// 输出当前某个操作的帮助信息。
    /// </summary>
    public bool HelpFlag { get; set; }

    /// <summary>
    /// 当前某个操作要用的目录。
    /// </summary>
    public string Dictionary { get; set; } = "";

    public bool ShowLogoFlag { get; set; } = true;

    #endregion



    public int Run()
    {
        InitializeApp();
        if (Args.Count == 0)
        {
            Help();
            return 1;
        }
        switch (Args[0].ToLower())
        {
            case "help":
                Help();
                break;
            case "install":
                Install();
                break;
            case "uninstall":
                Uninstall();
                break;
            case "update":
                Update();
                break;
            case "list":
                ListPackage();
                break;
            case "search":
                Search();
                break;
            case "hash":
                VerifyHash();
                break;
            case "download":
                Download();
                break;
            case "build":
                Build();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Out.WriteLine($"未知的命令：{Args[0]}");
                Help();
                return 1;
        }
        return 0;
    }

    /// <summary>
    /// 这是初始化app的方法，它的工作方式是：
    ///     1.首先检查配置文件，如果没有配置文件就新建一个配置文件。
    ///     2.然后在配置文件中获取（通常是下载）当前的源的源index文件。
    ///     3.然后解析index文件。
    /// </summary>
    public void InitializeApp()
    {
        ParseArgs();
        if (ShowLogoFlag)
        {
            ShowLogo();
        }
        InitializeSource();
    }

    public void InitializeSource()
    {
        
    }

    /// <summary>
    /// 用来显示app的版权信息的。
    /// </summary>
    public void ShowLogo()
    {
        Out.WriteLine($"Valency Package Manager 程序管理器 版本：{AppVersion}");
        Out.WriteLine("版权所有 (C) ClassicByte org. 保留所有权利。\n");
        Out.WriteLine("可使用此命令行工具来安装程序、工具等 Valency 应用程序包。用户也可以使用此命令行工具来构造自己的Valency应用程序包(*.vpkg)");
    }

    /// <summary>
    /// 用来解析和分配main传进来的开关的值的。
    /// </summary>
    public void ParseArgs()
    {
        ID = (GetSwitchValue("--id") == "") ? GetSwitchValue("--id") : GetSwitchValue("-i");
        Name = (GetSwitchValue("--name") == "") ? GetSwitchValue("--name") : GetSwitchValue("-n");
        Version = (GetSwitchValue("--version") == "") ? GetSwitchValue("--version") : GetSwitchValue("-v");
        Source = (GetSwitchValue("--source") == "") ? GetSwitchValue("--source") : GetSwitchValue("-s");
        PackageFileUri = (GetSwitchValue("--package") == "") ? GetSwitchValue("--package") : GetSwitchValue("-p");
        VerboseFlag = Args.Contains("--verbose");
        HelpFlag = Args.Contains("--help");
        Dictionary = (GetSwitchValue("--directory") == "") ? GetSwitchValue("--directory") : GetSwitchValue("-d");
    }

    /// <summary>
    /// 解析main传进来的值方法。
    /// </summary>
    /// <param name="switchName"></param>
    /// <returns></returns>
    private static string GetSwitchValue(string switchName)
    {
        for (int i = 0; i < Args.Count; i++)
        {
            // 检查当前参数是否是目标开关
            if (Args[i].Equals(switchName, StringComparison.OrdinalIgnoreCase))
            {
                // 如果开关后面有值，则返回该值
                if (i + 1 < Args.Count)
                {
                    return Args[i + 1];
                }
                else
                {
                    // 如果开关是最后一个参数，返回 null 或抛出异常
                    return "";
                }
            }
        }

        // 如果开关不存在，返回 null 或抛出异常
        return "";
    }


    /// <summary>
    /// 用来安装包的方法。
    /// </summary>

    public void Install()
    {
        if (HelpFlag)
        {
            Out.WriteLine("此命令用来将指定的包安装到计算机中，用户可以通过“-p”参数指定要安装的包的Uri（可以是本地或者网络位置），也可以通过“-i”指定包的id然后vpm就会在当前可用源中查找。");
            Out.WriteLine("如果没有指定任何参数，但是在“install”指令后面有用户的指定值，首先vpm会先检查此文件是否在本地，然后再在源中尝试查找匹配的内容，最后再尝试在网络位置下载包文件。\n");

            Out.Write("\n下列选项可用\n");
            Out.WriteLine("\n-p <Uri> 指定要安装的包的Uri（可以是本地或者网络位置）");
            Out.WriteLine("-i <id> 指定包的id然后vpm就会在当前可用源中查找");
            Out.WriteLine("--verbose 显示详细的输出信息");

            return;
        }
    }

    /// <summary>
    /// 用来解除安装包的方法。
    /// </summary>
    public void Uninstall()
    {

    }

    /// <summary>
    /// 用来列出当前安装过的包的方法。
    /// </summary>
    public void ListPackage()
    {

    }

    /// <summary>
    /// 用来验证某个文件Hash的方法。
    /// </summary>
    public void VerifyHash()
    {

    }

    /// <summary>
    /// 用来获取帮助。
    /// </summary>
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
    download 下载指定包");
    }

    /// <summary>
    /// 用来输出当前包管理器的详细信息的。
    /// </summary>
    public void PrintAppVersion()
    {

    }

    /// <summary>
    /// 用来执行下载操作的函数。
    /// </summary>
    public void Download()
    {

    }

    /// <summary>
    /// 用来更新某个包的方法。
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// 用来从源中搜索某个东西的方法。
    /// </summary>
    public void Search()
    {

    }

    public void Build()
    {

    }
}