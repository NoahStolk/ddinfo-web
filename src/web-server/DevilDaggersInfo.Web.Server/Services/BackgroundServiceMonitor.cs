using DevilDaggersInfo.Api.Admin.BackgroundServices;

namespace DevilDaggersInfo.Web.Server.Services;

public class BackgroundServiceMonitor
{
	private readonly ConcurrentBag<BackgroundServiceLog> _backgroundServiceLogs = new();

	public void Register(string name, TimeSpan interval)
		=> _backgroundServiceLogs.Add(new(name, interval));

	public void Update(string name, DateTime lastExecuted)
	{
		BackgroundServiceLog? backgroundServiceLog = _backgroundServiceLogs.FirstOrDefault(bsl => bsl.Name == name);
		if (backgroundServiceLog != null)
			backgroundServiceLog.LastExecuted = lastExecuted;
	}

	public List<GetBackgroundServiceEntry> GetEntries()
	{
		return _backgroundServiceLogs
			.OrderBy(bsl => bsl.Name)
			.Select(bsl => new GetBackgroundServiceEntry
			{
				Name =bsl.Name.Replace("BackgroundService", string.Empty),
				Interval = bsl.Interval,
				LastExecuted = bsl.LastExecuted,
			})
			.ToList();
	}

	private sealed class BackgroundServiceLog
	{
		public BackgroundServiceLog(string name, TimeSpan interval)
		{
			Name = name;
			Interval = interval;
		}

		public string Name { get; }
		public TimeSpan Interval { get; }

		public DateTime LastExecuted { get; set; }
	}
}
