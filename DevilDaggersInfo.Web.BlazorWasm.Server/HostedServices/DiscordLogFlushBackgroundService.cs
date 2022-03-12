using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class DiscordLogFlushBackgroundService : AbstractBackgroundService
{
	private readonly LogContainerService _logContainerService;

	public DiscordLogFlushBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, LogContainerService logContainerService, ILogger<DiscordLogFlushBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_logContainerService = logContainerService;
	}

	protected override TimeSpan Interval => TimeSpan.FromSeconds(2);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		DiscordChannel? logChannel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringLog].DiscordChannel;
		if (logChannel != null)
			await _logContainerService.LogToChannel(logChannel);

		DiscordChannel? validClLogChannel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringCustomLeaderboardValid].DiscordChannel;
		if (validClLogChannel != null)
			await _logContainerService.LogClLogsToChannel(true, validClLogChannel);

		DiscordChannel? invalidClLogChannel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringCustomLeaderboardInvalid].DiscordChannel;
		if (invalidClLogChannel != null)
			await _logContainerService.LogClLogsToChannel(false, invalidClLogChannel);
	}
}
