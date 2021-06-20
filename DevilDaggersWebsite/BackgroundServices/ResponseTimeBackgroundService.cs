using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class ResponseTimeBackgroundService : AbstractBackgroundService
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ResponseTimeContainer _responseTimeContainer;

		private DateTime _measurementStart;

		public ResponseTimeBackgroundService(IWebHostEnvironment environment, ResponseTimeContainer responseTimeContainer)
			: base(environment)
		{
			_environment = environment;
			_responseTimeContainer = responseTimeContainer;
		}

		protected override TimeSpan Interval => TimeSpan.FromHours(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			await DiscordLogger.TryLog(Channel.MonitoringTest, _environment.EnvironmentName, _responseTimeContainer.CreateLog(_measurementStart, DateTime.UtcNow));

			_responseTimeContainer.Clear();

			_measurementStart = DateTime.UtcNow;
		}

		protected override void Begin()
		{
			_measurementStart = DateTime.UtcNow;
		}
	}
}
