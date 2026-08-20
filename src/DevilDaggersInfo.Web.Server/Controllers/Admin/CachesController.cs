using DevilDaggersInfo.Web.ApiSpec.Admin.Caches;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/cache")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public sealed class CachesController(
	LeaderboardStatisticsCache leaderboardStatisticsCache,
	ILeaderboardHistoryCache leaderboardHistoryCache,
	ModArchiveCache modArchiveCache,
	ILogger<CachesController> logger)
	: ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCacheEntry>> GetCaches()
	{
		return new List<GetCacheEntry>
		{
			new() { Name = "LeaderboardHistory", Count = leaderboardHistoryCache.GetCount() },
			new() { Name = "LeaderboardStatistics", Count = leaderboardStatisticsCache.GetCount() },
			new() { Name = "ModArchive", Count = modArchiveCache.Count },
		};
	}

	[HttpPost("clear-cache")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult ClearCache([FromBody] string cacheType)
	{
		switch (cacheType)
		{
			case "LeaderboardHistory": leaderboardHistoryCache.Clear(); break;
			case "LeaderboardStatistics": leaderboardStatisticsCache.Initiate(); break;
			case "ModArchive": modArchiveCache.Clear(); break;
			default: return NotFound();
		}

		logger.LogInformation("Memory cache '{CacheType}' was cleared.", cacheType);

		return Ok();
	}
}
