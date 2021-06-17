using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Tasks
{
	public abstract class HostedService : BackgroundService
	{
		protected abstract TimeSpan Interval { get; }

		protected abstract Task ProcessDataAsync(CancellationToken stoppingToken);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await ProcessDataAsync(stoppingToken);

				if (Interval.TotalMilliseconds > 0)
					await Task.Delay(Interval, stoppingToken);
			}
		}
	}
}
