using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Extensions;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class CacheLoggerBackgroundService : AbstractBackgroundService
	{
		public CacheLoggerBackgroundService(IWebHostEnvironment environment)
			: base(environment)
		{
		}

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

			await DiscordLogger.EditMessage(ServerConstants.CacheMessage, builder.Build());
		}
	}
}
