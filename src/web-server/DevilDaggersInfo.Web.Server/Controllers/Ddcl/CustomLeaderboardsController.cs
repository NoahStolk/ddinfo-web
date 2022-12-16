using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-leaderboards")]
[ApiController]
[Obsolete("DDCL 1.8.3 will be removed.")]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;

	public CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository)
	{
		_customLeaderboardRepository = customLeaderboardRepository;
	}

	// FORBIDDEN: Used by ddstats-rust.
	// TODO: Remove when DDCL 1.8.3.0 is obsolete.
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHashObsolete([FromQuery] byte[] hash)
	{
		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}
}
