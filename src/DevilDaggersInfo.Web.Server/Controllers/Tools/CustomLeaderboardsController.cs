using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;
using DevilDaggersInfo.Web.Server.Domain.Converters.CoreToDomain;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Tools;

[Route("api/app/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;

	public CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository)
	{
		_customLeaderboardRepository = customLeaderboardRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCustomLeaderboardForOverview>>> GetCustomLeaderboards(int selectedPlayerId)
	{
		Domain.Models.Page<CustomLeaderboardOverview> customLeaderboards = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(
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
		return customLeaderboards.Results.ConvertAll(cl => cl.ToAppApi());
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToAppApi();
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardBySpawnsetHash([FromQuery] byte[] hash)
	{
		int customLeaderboardId = await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		SortedCustomLeaderboard customLeaderboard = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(customLeaderboardId);
		return customLeaderboard.ToAppApi();
	}

	[HttpHead("exists")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}

	[HttpGet("allowed-categories")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCustomLeaderboardAllowedCategory>> GetCustomLeaderboardAllowedCategories()
	{
		List<(DevilDaggersInfo.Core.Spawnset.GameMode GameMode, Domain.Entities.Enums.CustomLeaderboardRankSorting RankSorting)> allowedCategories = CustomLeaderboardUtils.GetAllowedGameModeAndRankSortingCombinations();
		return allowedCategories.ConvertAll(ac => new GetCustomLeaderboardAllowedCategory
		{
			GameMode = ac.GameMode.ToDomain().ToAppApi(),
			RankSorting = ac.RankSorting.ToAppApi(),
		});
	}
}
