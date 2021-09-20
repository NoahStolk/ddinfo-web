using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class BackgroundServiceLoggerBackgroundService : AbstractBackgroundService
{
	public BackgroundServiceLoggerBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger)
		: base(backgroundServiceMonitor, discordLogger)
	{
	}

	protected override bool LogExceptions => false;

	protected override TimeSpan Interval => TimeSpan.FromSeconds(15);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		if (DevilDaggersInfoServerConstants.BackgroundServiceMessage == null)
			return;

		DiscordEmbed? embed = BackgroundServiceMonitor.BuildDiscordEmbed();
		if (embed != null)
			await DiscordLogger.TryEditMessage(DevilDaggersInfoServerConstants.BackgroundServiceMessage, embed);
	}
}
