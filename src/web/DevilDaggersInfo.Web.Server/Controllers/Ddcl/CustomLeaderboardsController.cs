using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, SpawnsetHashCache spawnsetHashCache)
	{
		_dbContext = dbContext;
		_spawnsetHashCache = spawnsetHashCache;
	}

	// FORBIDDEN: Used by ddstats-rust.
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			return NotFound();

		var spawnset = await _dbContext.Spawnsets
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Name == data.Name);
		if (spawnset == null)
			return NotFound();

		if (!await _dbContext.CustomLeaderboards.AnyAsync(cl => cl.SpawnsetId == spawnset.Id))
			return NotFound();

		return Ok();
	}
}
