using DevilDaggersInfo.Web.BlazorWasm.Server.Caches;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/caches")]
[Authorize(Roles = Roles.Admin)]
[ApiController]
public class CachesController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public CachesController(
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		LeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		SpawnsetSummaryCache spawnsetSummaryCache,
		SpawnsetHashCache spawnsetHashCache)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_spawnsetHashCache = spawnsetHashCache;
	}

	[HttpPost("clear")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult ClearCache(CacheType cacheType)
	{
		switch (cacheType)
		{
			case CacheType.LeaderboardStatistics: _leaderboardStatisticsCache.Initiate(); break;
			case CacheType.LeaderboardHistory: _leaderboardHistoryCache.Clear(); break;
			case CacheType.ModArchive: _modArchiveCache.Clear(); break;
			case CacheType.SpawnsetSummary: _spawnsetSummaryCache.Clear(); break;
			case CacheType.SpawnsetHash: _spawnsetHashCache.Clear(); break;
		}

		return Ok();
	}
}
