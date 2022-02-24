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

	public void DumpLogs()
	{
		if (_responseTimeLogs.Count == 0)
			return;

		foreach (IGrouping<DateTime, ResponseTimeLog> dateGroup in _responseTimeLogs.GroupBy(rtl => rtl.DateTime.Date))
		{
			if (!dateGroup.Any())
				continue;

			DateTime earliestDateTime = dateGroup.Min(rtl => rtl.DateTime);
			DateTime latestDateTime = dateGroup.Max(rtl => rtl.DateTime);

			string filePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ResponseTimes), $"{latestDateTime:yyyyMMddHHmmss}.bin");
			if (File.Exists(filePath))
			{
				_logger.LogWarning("File {file} already exists.", Path.GetFileName(filePath));
				continue;
			}

			using MemoryStream ms = new();
			using BinaryWriter bw = new(ms);

			// Process paths here instead of in Add method to save performance during request.
			IEnumerable<IGrouping<string, ResponseTimeLog>> pathGroups = dateGroup.GroupBy(rtl => ProcessPath(rtl.Path));
			bw.Write(pathGroups.Count());

			foreach (IGrouping<string, ResponseTimeLog> pathGroup in pathGroups)
			{
				bw.Write(pathGroup.Key);
				bw.Write(pathGroup.Count());

				foreach (ResponseTimeLog rtl in pathGroup)
				{
					bw.Write(rtl.ResponseTimeTicks);
					bw.Write((byte)rtl.DateTime.Hour);
					bw.Write((byte)rtl.DateTime.Minute);
					bw.Write((byte)rtl.DateTime.Second);
				}
			}

			File.WriteAllBytes(filePath, ms.ToArray());

			_logger.LogInformation("Created {fileName} with {logs} logs from {from} until {until}.", Path.GetFileName(filePath), dateGroup.Count(), earliestDateTime, latestDateTime);
		}

		_responseTimeLogs.Clear();

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

	public GetResponseTimes GetLogEntries(DateOnly date)
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

		Dictionary<string, GetRequestPathEntry> entriesByPath = new();

		const int minuteInterval = 10;
		const int intervalCount = 24 * 60 / minuteInterval;
		Dictionary<string, Dictionary<int, GetRequestPathEntry>> entriesByTimeByPath = new();
		foreach (IGrouping<string, ResponseTimeLog> group in logs.GroupBy(rtl => rtl.Path).OrderBy(rtl => rtl.Key))
		{
			entriesByPath.Add(group.Key, new()
			{
				RequestCount = group.Count(),
				AverageResponseTimeTicks = group.Average(rtl => rtl.ResponseTimeTicks),
				MinResponseTimeTicks = group.Min(rtl => rtl.ResponseTimeTicks),
				MaxResponseTimeTicks = group.Max(rtl => rtl.ResponseTimeTicks),
			});

			Dictionary<int, GetRequestPathEntry> entriesByTime = Enumerable.Range(0, intervalCount).Select(i => i * minuteInterval).ToDictionary(i => i, _ => new GetRequestPathEntry());
			foreach (KeyValuePair<int, GetRequestPathEntry> kvp in entriesByTime)
			{
				IEnumerable<ResponseTimeLog> logsThisInterval = group.Where(rtl => rtl.DateTime.Hour * 60 + rtl.DateTime.Minute > kvp.Key && rtl.DateTime.Hour * 60 + rtl.DateTime.Minute < kvp.Key + minuteInterval);
				bool isEmpty = !logsThisInterval.Any();

				kvp.Value.RequestCount = isEmpty ? 0 : logsThisInterval.Count();
				kvp.Value.AverageResponseTimeTicks = isEmpty ? 0 : logsThisInterval.Average(rtl => rtl.ResponseTimeTicks);
				kvp.Value.MinResponseTimeTicks = isEmpty ? 0 : logsThisInterval.Min(rtl => rtl.ResponseTimeTicks);
				kvp.Value.MaxResponseTimeTicks = isEmpty ? 0 : logsThisInterval.Max(rtl => rtl.ResponseTimeTicks);
			}

			entriesByTimeByPath.Add(group.Key, entriesByTime);
		}

		return new()
		{
			ResponseTimeSummaryByRequestPath = entriesByPath,
			ResponseTimesByTimeByRequestPath = entriesByTimeByPath,
			MinuteInterval = minuteInterval,
		};
	}

	private readonly record struct ResponseTimeLog(string Path, int ResponseTimeTicks, DateTime DateTime);
}
