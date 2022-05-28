using DevilDaggersInfo.Web.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.Shared.Dto.Dd.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Controllers.Dd;

[Route("api/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public SpawnsetsController(SpawnsetHashCache spawnsetHashCache)
	{
		_spawnsetHashCache = spawnsetHashCache;
	}

	[HttpGet("name-by-hash")]
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
