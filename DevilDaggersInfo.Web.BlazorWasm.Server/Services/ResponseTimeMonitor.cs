using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class ResponseTimeMonitor
{
	private readonly List<ResponseTimeLog> _responseTimeLogs = new();

	public void Add(string path, long responseTimeTicks, DateTime dateTime)
	{
		int indexOfLastSeparator = path.LastIndexOf('/');
		if (indexOfLastSeparator != -1)
		{
			string finalPart = path[(indexOfLastSeparator + 1)..];
			if (int.TryParse(finalPart, out int _))
				path = $"{path[..(indexOfLastSeparator + 1)]}{{id}}";
		}

		_responseTimeLogs.Add(new(path, responseTimeTicks, dateTime));
	}

	public List<GetResponseTimeEntry> GetEntries(DateTime startDateTime, DateTime endDateTime)
	{
		if (_responseTimeLogs.Count == 0)
			return new();

		List<GetResponseTimeEntry> entries = new();
		foreach (IGrouping<string, ResponseTimeLog> group in _responseTimeLogs.Where(rtl => rtl.DateTime > startDateTime && rtl.DateTime < endDateTime).GroupBy(rl => rl.Path.ToLower()).OrderBy(rl => rl.Key))
		{
			int count = group.Count();
			double averageResponseTimeTicks = group.Average(rl => rl.ResponseTimeTicks);
			long minResponseTimeTicks = group.Min(rl => rl.ResponseTimeTicks);
			long maxResponseTimeTicks = group.Max(rl => rl.ResponseTimeTicks);
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

	public int GetCount()
		=> _responseTimeLogs.Count;

	public void Clear()
		=> _responseTimeLogs.Clear();

	private readonly record struct ResponseTimeLog(string Path, long ResponseTimeTicks, DateTime DateTime);
}
