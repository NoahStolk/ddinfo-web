using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Extensions;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Singletons;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.HostedServices
{
	public class CacheLoggerBackgroundService : AbstractBackgroundService
	{
		public CacheLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor)
			: base(environment, backgroundServiceMonitor)
		{
		}

		protected override bool LogExceptions => false;

		protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (ServerConstants.CacheMessage == null)
				return;

			DiscordEmbedBuilder builder = new()
			{
				Title = $"Cache {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			builder.AddFieldObject(nameof(LeaderboardStatisticsCache), LeaderboardStatisticsCache.Instance.LogState(Environment));
			builder.AddFieldObject(nameof(LeaderboardHistoryCache), LeaderboardHistoryCache.Instance.LogState(Environment));
			builder.AddFieldObject(nameof(ModArchiveCache), ModArchiveCache.Instance.LogState(Environment));
			builder.AddFieldObject(nameof(SpawnsetDataCache), SpawnsetDataCache.Instance.LogState(Environment));
			builder.AddFieldObject(nameof(SpawnsetHashCache), SpawnsetHashCache.Instance.LogState(Environment));

			await DiscordLogger.TryEditMessage(ServerConstants.CacheMessage, builder.Build());
		}
	}
}
