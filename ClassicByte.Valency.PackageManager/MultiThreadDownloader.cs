namespace ClassicByte.Valency.PackageManager
{
	public static class MultiThreadDownloader
	{
		public static async Task DownloadFileAsync(string url, string savePath, int threadCount)
		{
			var startDate = DateTime.Now;
			using var httpClient = new HttpClient();
			// ��ȡ�ļ��ܳ���
			var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
			response.EnsureSuccessStatusCode();
			if (!response.Content.Headers.ContentLength.HasValue)
				throw new InvalidOperationException("�޷���ȡ�ļ����ȣ����������ܲ�֧�ַֿ����ء�");

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

			// �ϲ��ֿ��ļ�
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
			Console.WriteLine($"������ɣ���ʱ��{duration.TotalSeconds}�룬�ļ���С��{totalLength / 1024 / 1024}MB");
		}

		public static TimeSpan DownloadFile(string url, string savePath, int threadCount)
		{
			var startDate = DateTime.Now;
			using var httpClient = new HttpClient();
			// ��ȡ�ļ��ܳ���
			var headRequest = new HttpRequestMessage(HttpMethod.Head, url);
			var response = httpClient.Send(headRequest);
			response.EnsureSuccessStatusCode();
			if (!response.Content.Headers.ContentLength.HasValue)
				throw new InvalidOperationException("�޷���ȡ�ļ����ȣ����������ܲ�֧�ַֿ����ء�");

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

				int idx = i; // ����հ�����
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

			// �ϲ��ֿ��ļ�
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