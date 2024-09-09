using DevilDaggersInfo.Web.ApiSpec.Main;
using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MainApi = DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

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
		[Required] GameMode gameMode,
		[Required] CustomLeaderboardRankSorting rankSorting,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		Domain.Models.Page<MainApi.CustomLeaderboardOverview> cls = await _customLeaderboardRepository.GetCustomLeaderboardOverviewsAsync(
			rankSorting: rankSorting.ToDomain(),
			gameMode: gameMode.ToDomain(),
			spawnsetFilter: spawnsetFilter,
			authorFilter: authorFilter,
			pageIndex: pageIndex,
			pageSize: pageSize,
			sortBy: sortBy?.ToDomain(),
			ascending: ascending,
			selectedPlayerId: null,
			onlyFeatured: false);
		return new Page<GetCustomLeaderboardOverview>
		{
			Results = cls.Results.ConvertAll(cl => cl.ToMainApi()),
			TotalResults = cls.TotalResults,
		};
	}

	[HttpGet("global-leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetGlobalCustomLeaderboard>> GetGlobalCustomLeaderboardForCategory([Required] GameMode gameMode, [Required] CustomLeaderboardRankSorting rankSorting)
	{
		MainApi.GlobalCustomLeaderboard globalCustomLeaderboard = await _customLeaderboardRepository.GetGlobalCustomLeaderboardAsync(gameMode.ToDomain(), rankSorting.ToDomain());
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
			DefaultBonus = GlobalCustomLeaderboardUtils.DefaultBonus,
			BronzeBonus = GlobalCustomLeaderboardUtils.BronzeBonus,
			SilverBonus = GlobalCustomLeaderboardUtils.SilverBonus,
			GoldenBonus = GlobalCustomLeaderboardUtils.GoldenBonus,
			DevilBonus = GlobalCustomLeaderboardUtils.DevilBonus,
			LeviathanBonus = GlobalCustomLeaderboardUtils.LeviathanBonus,
			RankingMultiplier = GlobalCustomLeaderboardUtils.RankingMultiplier,
		};
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<GetTotalCustomLeaderboardData>> GetTotalCustomLeaderboardData()
	{
		MainApi.CustomLeaderboardsTotalData totalData = await _customLeaderboardRepository.GetCustomLeaderboardsTotalDataAsync();
		return new GetTotalCustomLeaderboardData
		{
			LeaderboardsPerGameMode = totalData.LeaderboardsPerGameMode.ToDictionary(kvp => kvp.Key.ToMainApi(), kvp => kvp.Value),
			PlayersPerGameMode = totalData.PlayersPerGameMode.ToDictionary(kvp => kvp.Key.ToMainApi(), kvp => kvp.Value),
			ScoresPerGameMode = totalData.ScoresPerGameMode.ToDictionary(kvp => kvp.Key.ToMainApi(), kvp => kvp.Value),
			SubmitsPerGameMode = totalData.SubmitsPerGameMode.ToDictionary(kvp => kvp.Key.ToMainApi(), kvp => kvp.Value),
			TotalPlayers = totalData.TotalPlayers,
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		MainApi.SortedCustomLeaderboard cl = await _customLeaderboardRepository.GetSortedCustomLeaderboardByIdAsync(id);
		return cl.ToMainApi();
	}

	[HttpGet("allowed-categories")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetCustomLeaderboardAllowedCategory>>> GetCustomLeaderboardAllowedCategories()
	{
		List<MainApi.CustomLeaderboardAllowedCategory> allowedCategories = await _customLeaderboardRepository.GetCustomLeaderboardAllowedCategories();
		return allowedCategories.ConvertAll(ac => ac.ToMainApi());
	}
}
