using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class ResponseTimeLoggerBackgroundService : AbstractBackgroundService
	{
		private readonly ResponseTimeMonitor _responseTimeMonitor;

		private DateTime _measurementStart;

		public ResponseTimeLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, ResponseTimeMonitor responseTimeMonitor)
			: base(environment, backgroundServiceMonitor)
		{
			_responseTimeMonitor = responseTimeMonitor;
		}

		protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			DateTime now = DateTime.UtcNow;
			if (now.Hour != 12 || now.Minute != 0)
				return;

			bool includeEnvironmentName = true;
			foreach (string log in _responseTimeMonitor.CreateLogs(_measurementStart, now))
			{
				await DiscordLogger.TryLog(Channel.MonitoringTask, Environment.EnvironmentName, log, null, includeEnvironmentName);
				includeEnvironmentName = false;
			}

			_responseTimeMonitor.Clear();

			_measurementStart = DateTime.UtcNow; // Get UtcNow again. Logging takes time.
		}

		protected override void Begin()
		{
			_measurementStart = DateTime.UtcNow;
		}
	}
}
