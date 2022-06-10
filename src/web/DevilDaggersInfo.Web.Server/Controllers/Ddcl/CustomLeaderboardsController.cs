using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, SpawnsetHashCache spawnsetHashCache, CustomLeaderboardRepository customLeaderboardRepository)
	{
		_dbContext = dbContext;
		_spawnsetHashCache = spawnsetHashCache;
		_customLeaderboardRepository = customLeaderboardRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardByHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			throw new NotFoundException();

		var spawnset = await _dbContext.Spawnsets
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Name == data.Name);
		if (spawnset == null)
			throw new NotFoundException();

		var customLeaderboard = await _dbContext.CustomLeaderboards
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefaultAsync(cl => cl.SpawnsetId == spawnset.Id);
		if (customLeaderboard == null)
			throw new NotFoundException();

		SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(customLeaderboard.Id);
		return cl.ToDdclApi();
	}

	// FORBIDDEN: Used by ddstats-rust.
	// TODO: Remove when DDCL 1.8.3.0 is obsolete.
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHashObsolete([FromQuery] byte[] hash)
	{
		await CustomLeaderboardExistsBySpawnsetHashRepo(hash);
		return Ok();
	}

	[HttpHead]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		await CustomLeaderboardExistsBySpawnsetHashRepo(hash);
		return Ok();
	}

	private async Task CustomLeaderboardExistsBySpawnsetHashRepo(byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			throw new NotFoundException();

		var spawnset = await _dbContext.Spawnsets
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Name == data.Name);
		if (spawnset == null)
			throw new NotFoundException();

		if (!await _dbContext.CustomLeaderboards.AnyAsync(cl => cl.SpawnsetId == spawnset.Id))
			throw new NotFoundException();
	}
}
