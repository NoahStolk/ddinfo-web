using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class CacheLoggerBackgroundService : AbstractBackgroundService
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public CacheLoggerBackgroundService(
		BackgroundServiceMonitor backgroundServiceMonitor,
		DiscordLogger discordLogger,
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		LeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		SpawnsetSummaryCache spawnsetSummaryCache,
		SpawnsetHashCache spawnsetHashCache)
		: base(backgroundServiceMonitor, discordLogger)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
		_spawnsetSummaryCache = spawnsetSummaryCache;
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
		builder.AddFieldObject(nameof(SpawnsetSummaryCache), _spawnsetSummaryCache.LogState());
		builder.AddFieldObject(nameof(SpawnsetHashCache), _spawnsetHashCache.LogState());

		await DiscordLogger.TryEditMessage(DevilDaggersInfoServerConstants.CacheMessage, builder.Build());
	}
}
