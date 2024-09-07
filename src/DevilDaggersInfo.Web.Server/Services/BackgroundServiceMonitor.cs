using DevilDaggersInfo.Web.ApiSpec.Admin.BackgroundServices;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Services;

public class BackgroundServiceMonitor
{
	private readonly ConcurrentBag<BackgroundServiceLog> _backgroundServiceLogs = [];

	public void Register(string name, TimeSpan interval)
	{
		_backgroundServiceLogs.Add(new BackgroundServiceLog(name, interval));
	}

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
