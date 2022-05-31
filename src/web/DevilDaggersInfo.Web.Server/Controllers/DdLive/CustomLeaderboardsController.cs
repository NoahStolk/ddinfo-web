using DevilDaggersInfo.Web.Server.Converters.DdLive;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Repositories;
using DevilDaggersInfo.Web.Shared.Dto.DdLive.CustomLeaderboards;

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

	[HttpGet("/api/custom-leaderboards/ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetCustomLeaderboardOverviewDdLive>>> GetCustomLeaderboardsOverviewDdLive()
	{
		List<CustomLeaderboardOverview> cls = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync();
		return cls.ConvertAll(cl => cl.ToGetCustomLeaderboardOverviewDdLive());
	}

	[HttpGet("/api/custom-leaderboards/{id}/ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboardDdLive>> GetCustomLeaderboardByIdDdLive(int id)
	{
		SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToGetCustomLeaderboardDdLive(_customEntryRepository.GetExistingCustomEntryReplayIds(cl.CustomEntries.ConvertAll(ce => ce.Id)));
	}
}
