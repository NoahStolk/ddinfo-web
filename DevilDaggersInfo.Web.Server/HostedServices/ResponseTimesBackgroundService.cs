namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class ResponseTimesBackgroundService : AbstractBackgroundService
{
	private readonly ResponseTimeMonitor _responseTimeMonitor;

	public ResponseTimesBackgroundService(ResponseTimeMonitor responseTimeMonitor, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<ResponseTimesBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_responseTimeMonitor = responseTimeMonitor;
	}

	protected override TimeSpan Interval => TimeSpan.FromHours(2);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		_responseTimeMonitor.DumpLogs();
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		_responseTimeMonitor.DumpLogs();

		await base.StopAsync(cancellationToken);
	}
}
