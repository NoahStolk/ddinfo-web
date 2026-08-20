using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Tools;

[Route("api/app/custom-leaderboards")]
[ApiController]
public sealed class CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCustomLeaderboardForOverview>>> GetCustomLeaderboards(int selectedPlayerId)
	{
		Domain.Models.Page<CustomLeaderboardOverview> customLeaderboards = await customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(
			rankSorting: null,
			gameMode: null,
			spawnsetFilter: null,
			authorFilter: null,
			pageIndex: 0,
			pageSize: int.MaxValue,
			sortBy: CustomLeaderboardSorting.DateLastPlayed,
			ascending: false,
			selectedPlayerId: selectedPlayerId,
			onlyFeatured: false);
		return customLeaderboards.Results.ConvertAll(cl => cl.ToToolsApi());
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		SortedCustomLeaderboard cl = await customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToToolsApi();
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardBySpawnsetHash([FromQuery] byte[] hash)
	{
		int customLeaderboardId = await customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		SortedCustomLeaderboard customLeaderboard = await customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(customLeaderboardId);
		return customLeaderboard.ToToolsApi();
	}

	[HttpHead("exists")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		await customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}

	[HttpGet("allowed-categories")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCustomLeaderboardAllowedCategory>>> GetCustomLeaderboardAllowedCategories()
	{
		List<CustomLeaderboardAllowedCategory> customLeaderboardAllowedCategories = await customLeaderboardRepository.GetCustomLeaderboardAllowedCategories();
		return customLeaderboardAllowedCategories.ConvertAll(ac => ac.ToToolsApi());
	}
}
