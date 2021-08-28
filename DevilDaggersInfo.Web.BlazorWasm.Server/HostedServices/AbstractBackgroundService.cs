using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public abstract class AbstractBackgroundService : BackgroundService
{
	protected AbstractBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger)
	{
		BackgroundServiceMonitor = backgroundServiceMonitor;
		DiscordLogger = discordLogger;

		Name = GetType().Name;
	}

	protected BackgroundServiceMonitor BackgroundServiceMonitor { get; }
	protected DiscordLogger DiscordLogger { get; }

	protected string Name { get; }

	protected virtual bool LogExceptions => true;

	protected abstract TimeSpan Interval { get; }

	protected abstract Task ExecuteTaskAsync(CancellationToken stoppingToken);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Begin();

		while (!stoppingToken.IsCancellationRequested)
		{
			BackgroundServiceMonitor.Update(Name, DateTime.UtcNow);

			try
			{
				await ExecuteTaskAsync(stoppingToken);
			}
			catch (Exception ex)
			{
				if (LogExceptions)
					await DiscordLogger.TryLog(Channel.MonitoringTask, $":x: Task execution for `{Name}` failed with exception: `{ex.Message}`");
			}

			if (Interval.TotalMilliseconds > 0)
				await Task.Delay(Interval, stoppingToken);
		}

		await End();
	}

	protected virtual void Begin()
		=> BackgroundServiceMonitor.Register(Name, Interval);

	protected virtual async Task End()
		=> await DiscordLogger.TryLog(Channel.MonitoringTask, $":x: Cancellation for `{Name}` was requested.");
}
