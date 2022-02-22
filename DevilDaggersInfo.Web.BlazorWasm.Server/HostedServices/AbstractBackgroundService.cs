namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public abstract class AbstractBackgroundService : BackgroundService
{
	protected AbstractBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<AbstractBackgroundService> logger)
	{
		BackgroundServiceMonitor = backgroundServiceMonitor;
		Logger = logger;

		Name = GetType().Name;
	}

	protected BackgroundServiceMonitor BackgroundServiceMonitor { get; }
	protected ILogger<AbstractBackgroundService> Logger { get; }

	protected string Name { get; }

	protected abstract TimeSpan Interval { get; }

	protected abstract Task ExecuteTaskAsync(CancellationToken stoppingToken);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		BackgroundServiceMonitor.Register(Name, Interval);

		while (!stoppingToken.IsCancellationRequested)
		{
			BackgroundServiceMonitor.Update(Name, DateTime.UtcNow);

			try
			{
				await ExecuteTaskAsync(stoppingToken);
			}
			catch (OperationCanceledException ex)
			{
				Logger.LogError(ex, "OperationCanceledException was thrown for background service '{name}' during execution. This probably means the application is shutting down, but this is not a graceful exit. The task might not have completed successfully.", Name);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Task execution for `{name}` failed.", Name);
			}

			if (Interval.TotalMilliseconds > 0)
			{
				try
				{
					await Task.Delay(Interval, stoppingToken);
				}
				catch (OperationCanceledException ex)
				{
					Logger.LogError(ex, "OperationCanceledException was thrown for background service '{name}' during delay. This probably means the application is shutting down, but this is not a graceful exit.", Name);
				}
			}
		}
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("Background service '{name}' is stopping. This probably means the application is shutting down gracefully.", Name);

		await base.StopAsync(cancellationToken);
	}
}
