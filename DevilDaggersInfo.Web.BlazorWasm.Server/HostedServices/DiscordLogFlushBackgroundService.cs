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

		DiscordChannel? clLogChannel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringCustomLeaderboard].DiscordChannel;
		if (clLogChannel != null)
			await _logContainerService.LogClLogsToChannel(clLogChannel);
	}
}
