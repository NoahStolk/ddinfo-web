using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;

	public CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository)
	{
		_customLeaderboardRepository = customLeaderboardRepository;
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardBySpawnsetHash([FromQuery] byte[] hash)
	{
		int customLeaderboardId = await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		SortedCustomLeaderboard customLeaderboard = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(customLeaderboardId);
		return customLeaderboard.ToDdclApi();
	}

	[HttpGet("overview")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCustomLeaderboardForOverview>>> GetCustomLeaderboardOverview(int selectedPlayerId)
	{
		List<CustomLeaderboardOverview> customLeaderboards = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(selectedPlayerId);
		return customLeaderboards.ConvertAll(cl => cl.ToDdclApi());
	}

	[HttpHead("exists")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}

	// FORBIDDEN: Used by ddstats-rust.
	// TODO: Remove when DDCL 1.8.3.0 is obsolete.
	[Obsolete("Use the new 'exists' route.")]
	[HttpHead("/api/custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHashObsolete([FromQuery] byte[] hash)
	{
		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}
}
