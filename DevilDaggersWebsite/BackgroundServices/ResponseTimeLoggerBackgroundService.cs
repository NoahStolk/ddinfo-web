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
		private readonly ResponseTimeContainer _responseTimeContainer;

		private DateTime _measurementStart;

		public ResponseTimeLoggerBackgroundService(IWebHostEnvironment environment, ResponseTimeContainer responseTimeContainer)
			: base(environment)
		{
			_responseTimeContainer = responseTimeContainer;
		}

		protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			DateTime now = DateTime.UtcNow;
			if (now.Hour != 12 || now.Minute != 0)
				return;

			bool includeEnvironmentName = true;
			foreach (string log in _responseTimeContainer.CreateLogs(_measurementStart, now))
			{
				await DiscordLogger.TryLog(Channel.MonitoringTask, Environment.EnvironmentName, log, null, includeEnvironmentName);
				includeEnvironmentName = false;
			}

			_responseTimeContainer.Clear();

			_measurementStart = DateTime.UtcNow; // Get UtcNow again. Logging takes time.
		}

		protected override void Begin()
		{
			_measurementStart = DateTime.UtcNow;
		}
	}
}
