using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Singletons;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class BackgroundServiceLoggerBackgroundService : AbstractBackgroundService
	{
		public BackgroundServiceLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor)
			: base(environment, backgroundServiceMonitor)
		{
		}

		protected override bool LogExceptions => false;

		protected override TimeSpan Interval => TimeSpan.FromSeconds(5);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (ServerConstants.BackgroundServiceMessage == null)
				return;

			DiscordEmbed? embed = BackgroundServiceMonitor.BuildDiscordEmbed();
			if (embed != null)
				await DiscordLogger.EditMessage(ServerConstants.BackgroundServiceMessage, embed);
		}
	}
}
