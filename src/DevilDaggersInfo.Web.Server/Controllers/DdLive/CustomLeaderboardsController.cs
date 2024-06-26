using DevilDaggersInfo.Web.ApiSpec.DdLive.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomEntryRepository _customEntryRepository;
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;

	public CustomLeaderboardsController(CustomEntryRepository customEntryRepository, CustomLeaderboardRepository customLeaderboardRepository)
	{
		_customEntryRepository = customEntryRepository;
		_customLeaderboardRepository = customLeaderboardRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetCustomLeaderboardOverviewDdLive>>> GetCustomLeaderboardsOverviewDdLive()
	{
		Domain.Models.Page<CustomLeaderboardOverview> cls = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(
			rankSorting: null,
			gameMode: null,
			spawnsetFilter: null,
			authorFilter: null,
			pageIndex: 0,
			pageSize: int.MaxValue,
			sortBy: CustomLeaderboardSorting.DateLastPlayed,
			ascending: false,
			selectedPlayerId: null,
			onlyFeatured: false);
		return cls.Results.ConvertAll(cl => cl.ToDdLiveApi());
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboardDdLive>> GetCustomLeaderboardByIdDdLive(int id)
	{
		SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToDdLiveApi(_customEntryRepository.GetExistingCustomEntryReplayIds(cl.CustomEntries.ConvertAll(ce => ce.Id)));
	}
}
