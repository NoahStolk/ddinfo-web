using DevilDaggersInfo.Web.ApiSpec.Admin.Caches;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/cache")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CachesController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly ILogger<CachesController> _logger;

	public CachesController(
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		ILeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache,
		ILogger<CachesController> logger)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
		_logger = logger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCacheEntry>> GetCaches()
	{
		return new List<GetCacheEntry>
		{
			new() { Name = "LeaderboardHistory", Count = _leaderboardHistoryCache.GetCount() },
			new() { Name = "LeaderboardStatistics", Count = _leaderboardStatisticsCache.GetCount() },
			new() { Name = "ModArchive", Count = _modArchiveCache.GetCount() },
		};
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
			default: return NotFound();
		}

		_logger.LogInformation("Memory cache '{CacheType}' was cleared.", cacheType);

		return Ok();
	}
}
