using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class ResponseTimeMonitor
{
	private readonly List<ResponseTimeLog> _responseTimeLogs = new();

	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<ResponseTimeMonitor> _logger;

	public ResponseTimeMonitor(IFileSystemService fileSystemService, ILogger<ResponseTimeMonitor> logger)
	{
		_fileSystemService = fileSystemService;
		_logger = logger;
	}

	public void Add(string path, int responseTimeTicks, DateTime dateTime)
		=> _responseTimeLogs.Add(new(path, responseTimeTicks, dateTime));

	// TODO: Remove dateTime param and just dump everything.
	public void DumpLogs(DateTime dateTime)
	{
		if (_responseTimeLogs.Count == 0)
			return;

		string filePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ResponseTimes), $"{dateTime:yyyyMMddHHmmss}.bin");
		if (File.Exists(filePath))
			return;

		// Process paths here instead of in Add method to save performance during request.
		List<ResponseTimeLog> normalizedLogs = _responseTimeLogs
			.Where(rtl => rtl.DateTime.Year == dateTime.Year && rtl.DateTime.Month == dateTime.Month && rtl.DateTime.Day == dateTime.Day)
			.Select(rtl => new ResponseTimeLog(ProcessPath(rtl.Path), rtl.ResponseTimeTicks, rtl.DateTime))
			.ToList();

		_responseTimeLogs.Clear();

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		IEnumerable<IGrouping<string, ResponseTimeLog>> groups = normalizedLogs.GroupBy(rtl => rtl.Path);
		bw.Write(groups.Count());

		foreach (IGrouping<string, ResponseTimeLog> group in groups)
		{
			bw.Write(group.Key);
			bw.Write(group.Count());

			foreach (ResponseTimeLog rtl in group)
			{
				bw.Write(rtl.ResponseTimeTicks);
				bw.Write((byte)rtl.DateTime.Hour);
				bw.Write((byte)rtl.DateTime.Minute);
				bw.Write((byte)rtl.DateTime.Second);
			}
		}

		File.WriteAllBytes(filePath, ms.ToArray());

		_logger.LogInformation("Created {fileName} with {logs} logs.", Path.GetFileName(filePath), normalizedLogs.Count);

		static string ProcessPath(string path)
		{
			foreach (string part in path.Split('/'))
			{
				if (int.TryParse(part, out int _))
					path = path.Replace(part, "{id}");
			}

			return path;
		}
	}

	public List<GetResponseTimeEntry> GetLogEntries(DateOnly date)
	{
		List<ResponseTimeLog> logs = new();
		foreach (string filePath in _fileSystemService.TryGetFiles(DataSubDirectory.ResponseTimes).Where(p => Path.GetFileName(p).StartsWith(date.ToString("yyyyMMdd"))))
		{
			using MemoryStream ms = new(File.ReadAllBytes(filePath));
			using BinaryReader br = new(ms);

			int pathCount = br.ReadInt32();

			for (int i = 0; i < pathCount; i++)
			{
				string requestPath = br.ReadString();
				int requestCount = br.ReadInt32();

				for (int j = 0; j < requestCount; j++)
					logs.Add(new(requestPath, br.ReadInt32(), new(date.Year, date.Month, date.Day, br.ReadByte(), br.ReadByte(), br.ReadByte(), DateTimeKind.Utc)));
			}
		}

		List<GetResponseTimeEntry> entries = new();
		foreach (IGrouping<string, ResponseTimeLog> group in logs.GroupBy(rtl => rtl.Path).OrderBy(rtl => rtl.Key))
		{
			int count = group.Count();
			double averageResponseTimeTicks = group.Average(rl => rl.ResponseTimeTicks);
			int minResponseTimeTicks = group.Min(rl => rl.ResponseTimeTicks);
			int maxResponseTimeTicks = group.Max(rl => rl.ResponseTimeTicks);
			entries.Add(new()
			{
				RequestPath = group.Key,
				RequestCount = count,
				AverageResponseTimeTicks = averageResponseTimeTicks,
				MinResponseTimeTicks = minResponseTimeTicks,
				MaxResponseTimeTicks = maxResponseTimeTicks,
			});
		}

		return entries;
	}

	private readonly record struct ResponseTimeLog(string Path, int ResponseTimeTicks, DateTime DateTime);
}
