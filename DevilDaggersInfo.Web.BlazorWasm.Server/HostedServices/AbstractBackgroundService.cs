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
					Logger.LogError(ex, "Task execution for `{name}` failed.", Name);
			}

			if (Interval.TotalMilliseconds > 0)
				await Task.Delay(Interval, stoppingToken);
		}

		End();
	}

	protected virtual void Begin()
		=> BackgroundServiceMonitor.Register(Name, Interval);

	protected virtual void End()
		=> Logger.LogError("Cancellation for `{name}` was requested.", Name);
}
