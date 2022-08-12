using DevilDaggersInfo.Api.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

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
		=> _leaderboardHistoryStatisticsRepository.GetLeaderboardHistoryStatistics();
}
