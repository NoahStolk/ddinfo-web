using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices
{
	public class ResponseTimeLoggerBackgroundService : AbstractBackgroundService
	{
		private readonly ResponseTimeMonitor _responseTimeMonitor;

		private DateTime _measurementStart;

		public ResponseTimeLoggerBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger, ResponseTimeMonitor responseTimeMonitor)
			: base(backgroundServiceMonitor, discordLogger)
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
				await DiscordLogger.TryLog(Channel.MonitoringTask, log, null, includeEnvironmentName);
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
