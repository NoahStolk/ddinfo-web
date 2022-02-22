namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class ResponseTimesBackgroundService : AbstractBackgroundService
{
	private readonly ResponseTimeMonitor _responseTimeMonitor;

	public ResponseTimesBackgroundService(ResponseTimeMonitor responseTimeMonitor, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<ResponseTimesBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_responseTimeMonitor = responseTimeMonitor;
	}

	protected override TimeSpan Interval => TimeSpan.FromMinutes(30);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		_responseTimeMonitor.DumpLogs(DateTime.UtcNow);
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		await base.StopAsync(cancellationToken);
		_responseTimeMonitor.DumpLogs(DateTime.UtcNow);
	}
}
