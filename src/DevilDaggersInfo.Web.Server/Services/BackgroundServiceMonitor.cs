using DevilDaggersInfo.Web.ApiSpec.Admin.BackgroundServices;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Services;

// Injected into BackgroundServicesController, which MVC requires to be public,
// so this cannot be made internal.
public sealed class BackgroundServiceMonitor
{
	private readonly ConcurrentBag<BackgroundServiceLog> _backgroundServiceLogs = [];

	public void Register(string name, TimeSpan interval)
	{
		_backgroundServiceLogs.Add(new BackgroundServiceLog(name, interval));
	}

	public void Update(string name, DateTime lastExecuted)
	{
		BackgroundServiceLog? backgroundServiceLog = _backgroundServiceLogs.FirstOrDefault(bsl => bsl.Name == name);
		backgroundServiceLog?.LastExecuted = lastExecuted;
	}

	public List<GetBackgroundServiceEntry> GetEntries()
	{
		return
		[
			.. _backgroundServiceLogs
				.OrderBy(bsl => bsl.Name)
				.Select(bsl => new GetBackgroundServiceEntry
				{
					Name = bsl.Name.Replace("BackgroundService", string.Empty),
					Interval = bsl.Interval,
					LastExecuted = bsl.LastExecuted,
				}),
		];
	}

	private sealed class BackgroundServiceLog(string name, TimeSpan interval)
	{
		public string Name { get; } = name;
		public TimeSpan Interval { get; } = interval;

		public DateTime LastExecuted { get; set; }
	}
}
