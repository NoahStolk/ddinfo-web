using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.HostedServices
{
	public class BackgroundServiceLoggerBackgroundService : AbstractBackgroundService
	{
		public BackgroundServiceLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger)
			: base(environment, backgroundServiceMonitor, discordLogger)
		{
		}

		protected override bool LogExceptions => false;

		protected override TimeSpan Interval => TimeSpan.FromSeconds(5);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (DevilDaggersInfoServerConstants.BackgroundServiceMessage == null)
				return;

			DiscordEmbed? embed = BackgroundServiceMonitor.BuildDiscordEmbed();
			if (embed != null)
				await DiscordLogger.TryEditMessage(DevilDaggersInfoServerConstants.BackgroundServiceMessage, embed);
		}
	}
}
