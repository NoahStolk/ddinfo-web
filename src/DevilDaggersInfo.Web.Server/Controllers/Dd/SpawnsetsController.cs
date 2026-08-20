using DevilDaggersInfo.Web.ApiSpec.Dd;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Controllers.Dd;

[Route("api/dd/spawnsets")]
[ApiController]
public sealed class SpawnsetsController(ApplicationDbContext dbContext) : ControllerBase
{
	[HttpGet("/api/spawnsets/name-by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetNameByHash>> GetSpawnsetNameByHash([FromQuery] byte[] hash)
	{
		var spawnset = await dbContext.Spawnsets.Select(s => new { s.Name, s.Md5Hash }).FirstOrDefaultAsync(s => s.Md5Hash == hash);
		if (spawnset == null)
			return NotFound();

		return new GetSpawnsetNameByHash { Name = spawnset.Name };
	}
}
