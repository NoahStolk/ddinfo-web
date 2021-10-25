using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DSharpPlus.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.HostedServices
{
	public class CacheLoggerBackgroundService : AbstractBackgroundService
	{
		private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
		private readonly ModArchiveCache _modArchiveCache;
		private readonly SpawnsetDataCache _spawnsetDataCache;
		private readonly SpawnsetHashCache _spawnsetHashCache;

		public CacheLoggerBackgroundService(
			BackgroundServiceMonitor backgroundServiceMonitor,
			DiscordLogger discordLogger,
			LeaderboardStatisticsCache leaderboardStatisticsCache,
			LeaderboardHistoryCache leaderboardHistoryCache,
			ModArchiveCache modArchiveCache,
			SpawnsetDataCache spawnsetDataCache,
			SpawnsetHashCache spawnsetHashCache)
			: base(backgroundServiceMonitor, discordLogger)
		{
			_leaderboardStatisticsCache = leaderboardStatisticsCache;
			_leaderboardHistoryCache = leaderboardHistoryCache;
			_modArchiveCache = modArchiveCache;
			_spawnsetDataCache = spawnsetDataCache;
			_spawnsetHashCache = spawnsetHashCache;
		}

		protected override bool LogExceptions => false;

		protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (DevilDaggersInfoServerConstants.CacheMessage == null)
				return;

			DiscordEmbedBuilder builder = new()
			{
				Title = $"Cache {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			builder.AddFieldObject(nameof(LeaderboardStatisticsCache), _leaderboardStatisticsCache.LogState());
			builder.AddFieldObject(nameof(LeaderboardHistoryCache), _leaderboardHistoryCache.LogState());
			builder.AddFieldObject(nameof(ModArchiveCache), _modArchiveCache.LogState());
			builder.AddFieldObject(nameof(SpawnsetDataCache), _spawnsetDataCache.LogState());
			builder.AddFieldObject(nameof(SpawnsetHashCache), _spawnsetHashCache.LogState());

			await DiscordLogger.TryEditMessage(DevilDaggersInfoServerConstants.CacheMessage, builder.Build());
		}
	}
}
