namespace ClassicByte.Valency.PackageManager
{
	public static class MultiThreadDownloader
	{
		public static async Task DownloadFileAsync(string url, string savePath, int threadCount)
		{
			var startDate = DateTime.Now;
			using var httpClient = new HttpClient();
			// 获取文件总长度
			var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
			response.EnsureSuccessStatusCode();
			if (!response.Content.Headers.ContentLength.HasValue)
				throw new InvalidOperationException("无法获取文件长度，服务器可能不支持分块下载。");

			long totalLength = response.Content.Headers.ContentLength.Value;
			long partSize = totalLength / threadCount;
			var tasks = new Task[threadCount];
			var tempFiles = new string[threadCount];

			for (int i = 0; i < threadCount; i++)
			{
				long start = i * partSize;
				long end = (i == threadCount - 1) ? totalLength - 1 : (start + partSize - 1);
				string tempFile = $"{savePath}.part{i}";
				tempFiles[i] = tempFile;

				tasks[i] = Task.Run(async () =>
				{
					var req = new HttpRequestMessage(HttpMethod.Get, url);
					req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);
					using var partResp = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
					partResp.EnsureSuccessStatusCode();
					using var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None);
					await partResp.Content.CopyToAsync(fs);
				});
			}

			await Task.WhenAll(tasks);

			// 合并分块文件
			using (var output = new FileStream(savePath, FileMode.Create, FileAccess.Write))
			{
				foreach (var tempFile in tempFiles)
				{
					using var input = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
					await input.CopyToAsync(output);
					input.Close();
					File.Delete(tempFile);
				}
			}

			var endDate = DateTime.Now;
			var duration = endDate - startDate;
			Console.WriteLine($"下载完成，耗时：{duration.TotalSeconds}秒，文件大小：{totalLength / 1024 / 1024}MB");
		}

		public static TimeSpan DownloadFile(string url, string savePath, int threadCount)
		{
			var startDate = DateTime.Now;
			using var httpClient = new HttpClient();
			// 获取文件总长度
			var headRequest = new HttpRequestMessage(HttpMethod.Head, url);
			var response = httpClient.Send(headRequest);
			response.EnsureSuccessStatusCode();
			if (!response.Content.Headers.ContentLength.HasValue)
				throw new InvalidOperationException("无法获取文件长度，服务器可能不支持分块下载。");

			long totalLength = response.Content.Headers.ContentLength.Value;
			long partSize = totalLength / threadCount;
			var threads = new Thread[threadCount];
			var tempFiles = new string[threadCount];
			Exception? threadException = null;
			object exceptionLock = new object();

			for (int i = 0; i < threadCount; i++)
			{
				long start = i * partSize;
				long end = (i == threadCount - 1) ? totalLength - 1 : (start + partSize - 1);
				string tempFile = Path.Combine(UtilPath.Workspace.FullName, "Temp", $"{savePath}.part{i}");
				tempFiles[i] = tempFile;

				int idx = i; // 避免闭包问题
				threads[i] = new Thread(() =>
				{
					try
					{
						var req = new HttpRequestMessage(HttpMethod.Get, url);
						req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);
						using var partResp = httpClient.Send(req, HttpCompletionOption.ResponseHeadersRead);
						partResp.EnsureSuccessStatusCode();
						using var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None);
						using var stream = partResp.Content.ReadAsStream();
						stream.CopyTo(fs);
					}
					catch (Exception ex)
					{
						lock (exceptionLock)
						{
							threadException ??= ex;
						}
					}
				});
				threads[i].Start();
			}

			foreach (var t in threads)
				t.Join();

			if (threadException != null)
				throw threadException;

			// 合并分块文件
			using (var output = new FileStream(savePath, FileMode.Create, FileAccess.Write))
			{
				foreach (var tempFile in tempFiles)
				{
					using var input = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
					input.CopyTo(output);
					input.Close();
					File.Delete(tempFile);
				}
			}

			var endDate = DateTime.Now;
			return endDate - startDate;
		}
	}
}