using DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/leaderboard-statistics")]
public class LeaderboardStatisticsController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;

	public LeaderboardStatisticsController(LeaderboardStatisticsCache leaderboardStatisticsCache)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLive(LeaderboardStatisticsLimitDdLive? top)
	{
		return new GetLeaderboardStatisticsDdLive
		{
			DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(_leaderboardStatisticsCache.FileName),
			IsFetched = _leaderboardStatisticsCache.IsFetched,
			TotalEntries = _leaderboardStatisticsCache.Entries.Count,
			Statistics = (top switch
			{
				LeaderboardStatisticsLimitDdLive.Top1000 => _leaderboardStatisticsCache.Top1000ArrayStatistics,
				LeaderboardStatisticsLimitDdLive.Top100 => _leaderboardStatisticsCache.Top100ArrayStatistics,
				LeaderboardStatisticsLimitDdLive.Top10 => _leaderboardStatisticsCache.Top10ArrayStatistics,
				_ => _leaderboardStatisticsCache.GlobalArrayStatistics,
			}).ToGetArrayStatisticsDdLive(),
		};
	}
}
