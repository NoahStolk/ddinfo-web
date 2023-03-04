using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

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
			category: null,
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

	[Obsolete("Use GetCustomLeaderboards instead. This is used by DDINFO TOOLS <= 0.4.0.0.")]
	[HttpGet("overview")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetCustomLeaderboardForOverview>>> GetCustomLeaderboardOverview(
		[Required] CustomLeaderboardCategory category,
		int pageIndex,
		int pageSize,
		int selectedPlayerId,
		bool onlyFeatured)
	{
		Domain.Models.Page<CustomLeaderboardOverview> customLeaderboards = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(
			category: category,
			spawnsetFilter: null,
			authorFilter: null,
			pageIndex: pageIndex,
			pageSize: pageSize,
			sortBy: CustomLeaderboardSorting.DateLastPlayed,
			ascending: false,
			selectedPlayerId: selectedPlayerId,
			onlyFeatured: onlyFeatured);
		return new Page<GetCustomLeaderboardForOverview>
		{
			Results = customLeaderboards.Results.ConvertAll(cl => cl.ToAppApi()),
			TotalResults = customLeaderboards.TotalResults,
		};
	}

	[HttpHead("exists")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		await _customLeaderboardRepository.GetCustomLeaderboardIdBySpawnsetHashAsync(hash);
		return Ok();
	}
}
