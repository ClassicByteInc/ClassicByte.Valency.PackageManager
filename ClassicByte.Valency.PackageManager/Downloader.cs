#nullable disable

using System.Diagnostics;
using System.Net;
namespace ClassicByte.Valency.PackageManager;

public class MultiThreadDownloader
{
    private const string _extention = "filepart";
    private static long _totalBytesDownloaded;
    private static long _totalFileSize;

    // 进度更新委托
    public delegate void ProgressHandler(long totalSize, long downloaded, double progress);
    public static event ProgressHandler OnProgressChanged;

    public static void DownloadFile(string uri, string savePath, int threadCount = 4)
    {
        var startTime = DateTime.Now;
        Debug.WriteLine($"开始下载操作:{uri}。");
        _totalBytesDownloaded = 0;
        Debug.WriteLine("开始获取文件大小");
        _totalFileSize = GetFileSize(uri);
        Debug.WriteLine("get file size successed.");

        // 创建临时目录
        string tempDir = Directory.CreateDirectory(Path.Combine(UtilPath.Workspace.FullName, "Temp", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff"))).FullName;
        Debug.WriteLine($"created temp dir :{tempDir}");
        // 启动下载线程
        Debug.WriteLine($"start downloading,threads count : {threadCount}");
        Thread[] threads = new Thread[threadCount];
        for (int i = 0; i < threadCount; i++)
        {
            int threadId = i;
            threads[i] = new Thread(() =>
            {
                Debug.WriteLine($"thread[{threadId}] begin downloading file.");
                DownloadPart(uri, tempDir, threadId, _totalFileSize / threadCount);
            });
            threads[i].Start();
        }

        // 启动进度监控线程
        //Thread progressThread = new(ReportProgress);
        //progressThread.Start();
        Debug.WriteLine("download files parts successed");
        // 等待完成
        foreach (var t in threads) t.Join();
        //progressThread.Join();
        var successDownloadTime = DateTime.Now;
        Debug.WriteLine("begin merging fileparts");
        // 合并文件
        MergeFiles(tempDir, Path.Combine(savePath,GetFileNameFromUri(new Uri(uri))));
        var successMergeFileTime = DateTime.Now;

        Debug.WriteLine($"Download used : {successDownloadTime - startTime}");
        Debug.WriteLine($"Merge used : {successMergeFileTime - successDownloadTime}");
        Debug.WriteLine($"All time used : {successMergeFileTime - startTime}");

        Directory.Delete(tempDir, true);
    }

    private static void DownloadPart(string uri, string tempDir, int threadId, long blockSize)
    {
        long start = threadId * blockSize;
        long end = (threadId == (_totalFileSize / blockSize) - 1) ?
            _totalFileSize - 1 : start + blockSize - 1;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AddRange(start, end);

            using WebResponse response = request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using FileStream fs = new(
                Path.Combine(tempDir, $"{threadId:00}.{_extention}"),
                FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] buffer = new byte[4096 * 4]; // 16KB缓冲区
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, bytesRead);
                Interlocked.Add(ref _totalBytesDownloaded, bytesRead);
            }
        }
        catch (WebException)
        {
            throw;
        }
    }

    private static void ReportProgress()
    {
        while (true)
        {
            double progress = (_totalFileSize > 0) ?
                (double)_totalBytesDownloaded / _totalFileSize * 100 : 0;

            // 触发进度事件
            OnProgressChanged?.Invoke(_totalFileSize, _totalBytesDownloaded, progress);

            Thread.Sleep(200); // 控制刷新频率
            if (_totalBytesDownloaded >= _totalFileSize) break;
        }
    }

    // 其他辅助方法同原始实现...

    private static long GetFileSize(string uri)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
        req.Method = "HEAD";
        using WebResponse resp = req.GetResponse();
        return resp.ContentLength;
    }

    private static void MergeFiles(string tempDir, string savePath)
    {
        using FileStream fs = new(savePath, FileMode.Create);
        var files = Directory.GetFiles(tempDir, $"*.{_extention}").ToList();
        files.Sort();

        foreach (string tempFile in files)
        {
            byte[] data = File.ReadAllBytes(tempFile);
            fs.Write(data, 0, data.Length);
        }
    }

    public static string GetFileNameFromUri(Uri uri)
    {
        // 获取规范化路径（自动处理URL编码和路径分隔符）
        string decodedPath = uri.LocalPath;

        // 使用系统路径处理方法
        string fileName = Path.GetFileName(decodedPath);

        // 处理以目录结尾的URL（如http://a.com/dir/）
        return string.IsNullOrEmpty(fileName) ? "default" : fileName;
    }
}
