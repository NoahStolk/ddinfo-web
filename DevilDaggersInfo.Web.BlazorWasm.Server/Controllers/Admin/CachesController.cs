using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/caches")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
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
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult ClearCache([FromBody] string cacheType)
	{
		switch (cacheType)
		{
			case "LeaderboardHistory": _leaderboardHistoryCache.Clear(); break;
			case "LeaderboardStatistics": _leaderboardStatisticsCache.Initiate(); break;
			case "ModArchive": _modArchiveCache.Clear(); break;
			case "SpawnsetHash": _spawnsetHashCache.Clear(); break;
			case "SpawnsetSummary": _spawnsetSummaryCache.Clear(); break;
			default: return NotFound();
		}

		return Ok();
	}
}
