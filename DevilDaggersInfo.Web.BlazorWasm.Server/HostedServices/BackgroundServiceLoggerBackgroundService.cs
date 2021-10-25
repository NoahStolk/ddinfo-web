using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class BackgroundServiceLoggerBackgroundService : AbstractBackgroundService
{
	public BackgroundServiceLoggerBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<BackgroundServiceLoggerBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
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
			await DevilDaggersInfoServerConstants.BackgroundServiceMessage.TryEdit(embed);
	}
}
