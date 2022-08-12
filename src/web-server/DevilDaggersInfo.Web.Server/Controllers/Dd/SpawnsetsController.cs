using DevilDaggersInfo.Api.Dd;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;

namespace DevilDaggersInfo.Web.Server.Controllers.Dd;

[Route("api/dd/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public SpawnsetsController(SpawnsetHashCache spawnsetHashCache)
	{
		_spawnsetHashCache = spawnsetHashCache;
	}

	[HttpGet("/api/spawnsets/name-by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetSpawnsetNameByHash> GetSpawnsetNameByHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			return NotFound();

		return new GetSpawnsetNameByHash { Name = data.Name };
	}
}
