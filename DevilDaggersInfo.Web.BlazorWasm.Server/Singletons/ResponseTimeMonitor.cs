using DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;
using System.Text;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;

public class ResponseTimeMonitor
{
	private const int _maxLinesPerMessage = 10;

	private readonly List<ResponseTimeLog> _responseTimeLogs = new();

	public void Add(string path, long responseTimeTicks, DateTime dateTime)
		=> _responseTimeLogs.Add(new(path, responseTimeTicks, dateTime));

	public List<string> CreateLogs(DateTime startDateTime, DateTime endDateTime)
	{
		StringBuilder sb = new($"```Response times\nBetween {startDateTime:yyyy-MM-dd HH:mm:ss} and {endDateTime:yyyy-MM-dd HH:mm:ss}\n\n");

		if (_responseTimeLogs.Count == 0)
		{
			sb.Append("Nobody accessed the website during this period.```");
			return new() { sb.ToString() };
		}

		List<string> logs = new();

		sb.AppendFormat("{0,-50}", nameof(ResponseTimeLog.Path))
			.AppendFormat("{0,10}", "Requests")
			.AppendFormat("{0,25}", "AverageResponseTime")
			.AppendFormat("{0,12}", "MinTime")
			.AppendFormat("{0,12}", "MaxTime")
			.AppendLine();
		int i = 0;
		foreach (IGrouping<string, ResponseTimeLog> group in _responseTimeLogs.GroupBy(rl => rl.Path.ToLower()).OrderBy(rl => rl.Key))
		{
			int count = group.Count();
			double averageResponseTimeTicks = group.Average(rl => rl.ResponseTimeTicks);
			long minResponseTimeTicks = group.Min(rl => rl.ResponseTimeTicks);
			long maxResponseTimeTicks = group.Max(rl => rl.ResponseTimeTicks);
			sb.AppendFormat("{0,-50}", group.Key.TrimAfter(50))
				.AppendFormat("{0,10}", count)
				.AppendFormat("{0,25}", GetFormattedTime(averageResponseTimeTicks))
				.AppendFormat("{0,12}", GetFormattedTime(minResponseTimeTicks))
				.AppendFormat("{0,12}", GetFormattedTime(maxResponseTimeTicks))
				.AppendLine();

			i++;

			if (i >= _maxLinesPerMessage)
			{
				sb.AppendLine("```");
				logs.Add(sb.ToString());

				sb.Clear();
				sb.AppendLine("```");
				i = 0;
			}
		}

		return logs;
	}

	public void Clear()
		=> _responseTimeLogs.Clear();

	private static string GetFormattedTime(double ticks)
	{
		if (ticks >= TimeSpan.TicksPerSecond)
			return $"{ticks / (float)TimeSpan.TicksPerSecond:0.00} s";

		if (ticks >= TimeSpan.TicksPerMillisecond)
			return $"{ticks / (float)TimeSpan.TicksPerMillisecond:0.0} ms";

		return $"{ticks / 10f:0} μs";
	}

	private static string GetFormattedTime(long ticks)
	{
		if (ticks >= TimeSpan.TicksPerSecond)
			return $"{ticks / (float)TimeSpan.TicksPerSecond:0.00} s";

		if (ticks >= TimeSpan.TicksPerMillisecond)
			return $"{ticks / (float)TimeSpan.TicksPerMillisecond:0.0} ms";

		return $"{ticks / 10f:0} μs";
	}

	private class ResponseTimeLog
	{
		public ResponseTimeLog(string path, long responseTimeTicks, DateTime dateTime)
		{
			Path = path;
			ResponseTimeTicks = responseTimeTicks;
			DateTime = dateTime;
		}

		public string Path { get; }

		public long ResponseTimeTicks { get; }

		public DateTime DateTime { get; }
	}
}
