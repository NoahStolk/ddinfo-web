using DevilDaggersDiscordBot;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public abstract class AbstractBackgroundService : BackgroundService
	{
		protected AbstractBackgroundService(IWebHostEnvironment environment)
		{
			Environment = environment;

			BackgroundServiceName = GetType().Name;
		}

		protected IWebHostEnvironment Environment { get; }

		protected string BackgroundServiceName { get; }

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
					await DiscordLogger.TryLog(channel, Environment.EnvironmentName, $":x: Task execution for `{BackgroundServiceName}` failed with exception: `{ex.Message}`");
				}

				if (Interval.TotalMilliseconds > 0)
					await Task.Delay(Interval, stoppingToken);
			}

			await End();
		}

		protected virtual void Begin()
		{
		}

		protected virtual async Task End()
		{
			await DiscordLogger.TryLog(Channel.MonitoringTask, Environment.EnvironmentName, $":x: Cancellation for `{BackgroundServiceName}` was requested.");
		}
	}
}
