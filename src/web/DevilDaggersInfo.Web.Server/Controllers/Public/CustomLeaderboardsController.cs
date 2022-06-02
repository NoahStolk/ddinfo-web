using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.Public;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Repositories;
using DevilDaggersInfo.Web.Shared.Dto;
using DevilDaggersInfo.Web.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

[Route("api/custom-leaderboards")]
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
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Page<GetCustomLeaderboardOverview>>> GetCustomLeaderboards(
		CustomLeaderboardCategory category,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PagingConstants.PageSizeMin, PagingConstants.PageSizeMax)] int pageSize = PagingConstants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		(List<CustomLeaderboardOverview> cls, int total) = await _customLeaderboardRepository.GetSortedCustomLeaderboardOverviewsAsync(category, spawnsetFilter, authorFilter, pageIndex, pageSize, sortBy, ascending);
		return new Page<GetCustomLeaderboardOverview>
		{
			Results = cls.ConvertAll(cl => cl.ToGetCustomLeaderboardOverview()),
			TotalResults = total,
		};
	}

	[HttpGet("global-leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetGlobalCustomLeaderboard>> GetGlobalCustomLeaderboardForCategory(CustomLeaderboardCategory category)
	{
		GlobalCustomLeaderboard globalCustomLeaderboard = await _customLeaderboardRepository.GetGlobalCustomLeaderboardAsync(category);
		return new GetGlobalCustomLeaderboard
		{
			Entries = globalCustomLeaderboard.Entries
				.ConvertAll(e => new GetGlobalCustomLeaderboardEntry
				{
					DefaultDaggerCount = e.DefaultDaggerCount,
					BronzeDaggerCount = e.BronzeDaggerCount,
					SilverDaggerCount = e.SilverDaggerCount,
					GoldenDaggerCount = e.GoldenDaggerCount,
					DevilDaggerCount = e.DevilDaggerCount,
					LeviathanDaggerCount = e.LeviathanDaggerCount,
					LeaderboardsPlayedCount = e.LeaderboardsPlayedCount,
					PlayerId = e.PlayerId,
					PlayerName = e.PlayerName,
					Points = e.Points,
				})
				.OrderByDescending(ce => ce.Points)
				.ThenByDescending(ce => ce.LeaderboardsPlayedCount)
				.ToList(),
			TotalLeaderboards = globalCustomLeaderboard.TotalLeaderboards,
			TotalPoints = globalCustomLeaderboard.TotalPoints,
		};
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<GetTotalCustomLeaderboardData>> GetTotalCustomLeaderboardData()
	{
		CustomLeaderboardsTotalData totalData = await _customLeaderboardRepository.GetCustomLeaderboardsTotalDataAsync();
		return new GetTotalCustomLeaderboardData
		{
			LeaderboardsPerCategory = totalData.LeaderboardsPerCategory,
			PlayersPerCategory = totalData.PlayersPerCategory,
			ScoresPerCategory = totalData.ScoresPerCategory,
			SubmitsPerCategory = totalData.SubmitsPerCategory,
			TotalPlayers = totalData.TotalPlayers,
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToGetCustomLeaderboard();
	}
}
