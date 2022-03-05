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

	private enum HttpMethod : byte
	{
		Get,
		Post,
		Put,
		Delete,
		Patch,
		Head,
	}

	public void Add(string method, string path, int responseTimeTicks, DateTime dateTime)
	{
		HttpMethod httpMethod = method.ToUpper() switch
		{
			"POST" => HttpMethod.Post,
			"PUT" => HttpMethod.Put,
			"DELETE" => HttpMethod.Delete,
			"PATCH" => HttpMethod.Patch,
			"HEAD" => HttpMethod.Head,
			_ => HttpMethod.Get,
		};
		_responseTimeLogs.Add(new(new(httpMethod, path), responseTimeTicks, dateTime));
	}

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

			// Version
			bw.Write((byte)1);

			// Process paths here instead of in Add method to save performance during request.
			IEnumerable<IGrouping<Route, ResponseTimeLog>> pathGroups = dateGroup.GroupBy(rtl => ProcessRoute(rtl.Route));
			bw.Write(pathGroups.Count());

			foreach (IGrouping<Route, ResponseTimeLog> pathGroup in pathGroups)
			{
				bw.Write((byte)pathGroup.Key.HttpMethod);
				bw.Write(pathGroup.Key.Path);
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

		static Route ProcessRoute(Route route)
		{
			string newPath = route.Path;

			foreach (string part in newPath.Split('/'))
			{
				if (int.TryParse(part, out int _))
					newPath = newPath.Replace(part, "{id}");
			}

			return route with { Path = newPath };
		}
	}

	public GetResponseTimes GetLogEntries(DateOnly date)
	{
		List<ResponseTimeLog> logs = new();
		foreach (string filePath in _fileSystemService.TryGetFiles(DataSubDirectory.ResponseTimes).Where(p => Path.GetFileName(p).StartsWith(date.ToString("yyyyMMdd"))))
		{
			using MemoryStream ms = new(File.ReadAllBytes(filePath));
			using BinaryReader br = new(ms);

			// Version
			_ = br.ReadByte();

			int pathCount = br.ReadInt32();

			for (int i = 0; i < pathCount; i++)
			{
				HttpMethod httpMethod = (HttpMethod)br.ReadByte();
				string requestPath = br.ReadString();
				int requestCount = br.ReadInt32();

				for (int j = 0; j < requestCount; j++)
					logs.Add(new(new(httpMethod, requestPath), br.ReadInt32(), new(date.Year, date.Month, date.Day, br.ReadByte(), br.ReadByte(), br.ReadByte(), DateTimeKind.Utc)));
			}
		}

		Dictionary<string, GetRequestPathEntry> entriesByPath = new();

		const int minuteInterval = 10;
		const int intervalCount = 24 * 60 / minuteInterval;
		Dictionary<string, Dictionary<int, GetRequestPathEntry>> entriesByTimeByPath = new();
		foreach (IGrouping<Route, ResponseTimeLog> group in logs.GroupBy(rtl => rtl.Route).OrderBy(rtl => rtl.Key))
		{
			entriesByPath.Add(group.Key.ToString(), new()
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

			entriesByTimeByPath.Add(group.Key.ToString(), entriesByTime);
		}

		return new()
		{
			ResponseTimeSummaryByRequestPath = entriesByPath,
			ResponseTimesByTimeByRequestPath = entriesByTimeByPath,
			MinuteInterval = minuteInterval,
		};
	}

	private readonly record struct ResponseTimeLog(Route Route, int ResponseTimeTicks, DateTime DateTime);

	private readonly record struct Route(HttpMethod HttpMethod, string Path) : IComparable<Route>
	{
		public int CompareTo(Route other)
		{
			int routeComparison = other.HttpMethod.CompareTo(other.HttpMethod);
			if (routeComparison != 0)
				return routeComparison;

			return other.Path.CompareTo(Path);
		}

		public override string ToString() => $"{HttpMethod.ToString().ToUpper()} {Path}";
	}
}
