using DevilDaggersInfo.Web.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.Server.Caches.SpawnsetSummaries;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly ILogger<AdminController> _logger;

	public AdminController(
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		LeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		SpawnsetSummaryCache spawnsetSummaryCache,
		SpawnsetHashCache spawnsetHashCache,
		ILogger<AdminController> logger)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_spawnsetHashCache = spawnsetHashCache;
		_logger = logger;
	}

	[HttpPost("clear-cache")]
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

		_logger.LogInformation("Memory cache '{cacheType}' was cleared.", cacheType);

		return Ok();
	}
}
