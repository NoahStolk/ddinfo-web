using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-history-statistics")]
[ApiController]
public class LeaderboardHistoryStatisticsController : ControllerBase
{
	private readonly LeaderboardHistoryStatisticsRepository _leaderboardHistoryStatisticsRepository;

	public LeaderboardHistoryStatisticsController(LeaderboardHistoryStatisticsRepository leaderboardHistoryStatisticsRepository)
	{
		_leaderboardHistoryStatisticsRepository = leaderboardHistoryStatisticsRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetLeaderboardHistoryStatistics> GetLeaderboardHistoryStatistics()
	{
		return _leaderboardHistoryStatisticsRepository.GetLeaderboardHistoryStatistics();
	}
}
