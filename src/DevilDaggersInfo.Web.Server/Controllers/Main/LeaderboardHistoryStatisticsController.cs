using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-history-statistics")]
[ApiController]
public sealed class LeaderboardHistoryStatisticsController(LeaderboardHistoryStatisticsRepository leaderboardHistoryStatisticsRepository) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetLeaderboardHistoryStatistics> GetLeaderboardHistoryStatistics()
	{
		return leaderboardHistoryStatisticsRepository.GetLeaderboardHistoryStatistics();
	}
}
