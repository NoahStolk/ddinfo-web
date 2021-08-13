using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchive;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetData;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHash;
using DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;
using DSharpPlus.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices
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
