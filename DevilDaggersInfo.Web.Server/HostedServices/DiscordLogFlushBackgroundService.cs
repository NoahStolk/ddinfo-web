using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class DiscordLogFlushBackgroundService : AbstractBackgroundService
{
	private readonly LogContainerService _logContainerService;
	private readonly IWebHostEnvironment _environment;

	public DiscordLogFlushBackgroundService(LogContainerService logContainerService, IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<DiscordLogFlushBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_logContainerService = logContainerService;
		_environment = environment;
	}

	protected override TimeSpan Interval => TimeSpan.FromSeconds(2);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		DiscordChannel? logChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringLog, _environment);
		if (logChannel != null)
			await _logContainerService.LogToLogChannel(logChannel);

		DiscordChannel? auditLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MaintainersAuditLog, _environment);
		if (auditLogChannel != null)
			await _logContainerService.LogToAuditLogChannel(auditLogChannel);

		DiscordChannel? validClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardValid, _environment);
		if (validClLogChannel != null)
			await _logContainerService.LogClLogsToChannel(true, validClLogChannel);

		DiscordChannel? invalidClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardInvalid, _environment);
		if (invalidClLogChannel != null)
			await _logContainerService.LogClLogsToChannel(false, invalidClLogChannel);
	}
}
