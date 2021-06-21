using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public abstract class AbstractBackgroundService : BackgroundService
	{
		protected AbstractBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor)
		{
			Environment = environment;
			BackgroundServiceMonitor = backgroundServiceMonitor;

			Name = GetType().Name;
		}

		protected IWebHostEnvironment Environment { get; }
		protected BackgroundServiceMonitor BackgroundServiceMonitor { get; }

		protected string Name { get; }

		protected abstract TimeSpan Interval { get; }

		protected abstract Task ExecuteTaskAsync(CancellationToken stoppingToken);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Begin();

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await ExecuteTaskAsync(stoppingToken);
				}
				catch (Exception ex)
				{
					Channel channel = Environment.IsDevelopment() ? Channel.MonitoringTest : Channel.MonitoringTask;
					await DiscordLogger.TryLog(channel, Environment.EnvironmentName, $":x: Task execution for `{Name}` failed with exception: `{ex.Message}`");
				}

				BackgroundServiceMonitor.Update(Name, DateTime.UtcNow);

				if (Interval.TotalMilliseconds > 0)
					await Task.Delay(Interval, stoppingToken);
			}

			await End();
		}

		protected virtual void Begin()
			=> BackgroundServiceMonitor.Register(Name, Interval);

		protected virtual async Task End()
			=> await DiscordLogger.TryLog(Channel.MonitoringTask, Environment.EnvironmentName, $":x: Cancellation for `{Name}` was requested.");
	}
}
