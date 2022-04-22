using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class BackgroundServiceLoggerBackgroundService : AbstractBackgroundService
{
	public BackgroundServiceLoggerBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<BackgroundServiceLoggerBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
	}

	protected override TimeSpan Interval => TimeSpan.FromSeconds(15);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		if (DiscordServerConstants.BackgroundServiceMessage == null)
			return;

		DiscordEmbed? embed = BackgroundServiceMonitor.BuildDiscordEmbed();
		if (embed != null)
			await DiscordServerConstants.BackgroundServiceMessage.TryEdit(embed);
	}
}
