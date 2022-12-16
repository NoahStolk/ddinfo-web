using DevilDaggersInfo.Web.Server.Services;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public abstract class AbstractBackgroundService : BackgroundService
{
	private readonly BackgroundServiceMonitor _backgroundServiceMonitor;
	private readonly string _name;

	protected AbstractBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<AbstractBackgroundService> logger)
	{
		_backgroundServiceMonitor = backgroundServiceMonitor;
		Logger = logger;

		_name = GetType().Name;
	}

	protected ILogger<AbstractBackgroundService> Logger { get; }

	protected abstract TimeSpan Interval { get; }

	protected abstract Task ExecuteTaskAsync(CancellationToken stoppingToken);

	protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_backgroundServiceMonitor.Register(_name, Interval);

		while (!stoppingToken.IsCancellationRequested)
		{
			_backgroundServiceMonitor.Update(_name, DateTime.UtcNow);

			try
			{
				await ExecuteTaskAsync(stoppingToken);
			}
			catch (OperationCanceledException ex)
			{
				Logger.LogError(ex, "OperationCanceledException was thrown for background service '{name}' during execution. This probably means the application is shutting down, but this is not a graceful exit. The task might not have completed successfully.", _name);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Task execution for `{name}` failed.", _name);
			}

			if (Interval.TotalMilliseconds > 0)
				await Task.Delay(Interval, stoppingToken);
		}
	}
}
