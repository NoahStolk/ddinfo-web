using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Obsolete("DDCL 1.8.3 will be removed.")]
[Route("api/ddcl/custom-leaderboards")]
[ApiController]
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
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHashObsolete([FromQuery] byte[] hash)
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	{
		throw new BadRequestException("DDCL 1.8.3 is no longer supported. Go to https://www.devildaggers.info/ and download DDINFO TOOLS to connect to custom leaderboards");

		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}
}
