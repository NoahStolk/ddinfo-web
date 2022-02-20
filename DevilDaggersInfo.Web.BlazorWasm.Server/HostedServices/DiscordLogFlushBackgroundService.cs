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
		DiscordChannel? channel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringLog].DiscordChannel;
		if (channel == null)
			return;

		while (_logContainerService.LogEntries.Count > 0)
		{
			DiscordEmbed embed = _logContainerService.LogEntries[0];
			await channel.SendMessageAsyncSafe(null, embed);
			_logContainerService.RemoveFirst();
		}
	}
}
