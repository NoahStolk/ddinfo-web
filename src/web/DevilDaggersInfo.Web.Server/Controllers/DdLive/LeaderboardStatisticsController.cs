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

	[Obsolete("Use the new route instead.")]
	[HttpGet("/api/leaderboard-statistics/ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLiveObsolete([Required] LeaderboardStatisticsLimitDdLive top)
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
				_ => _leaderboardStatisticsCache.Top10ArrayStatistics,
			}).ToGetArrayStatisticsDdLive(),
		};
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLive(LeaderboardStatisticsLimitDdLive? top)
	{
		if (top == null)
		{
			return new GetLeaderboardStatisticsDdLive
			{
				DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(_leaderboardStatisticsCache.FileName),
				IsFetched = _leaderboardStatisticsCache.IsFetched,
				TotalEntries = _leaderboardStatisticsCache.Entries.Count,
				Statistics = _leaderboardStatisticsCache.GlobalArrayStatistics.ToGetArrayStatisticsDdLive(),
			};
		}

		return new GetLeaderboardStatisticsDdLive
		{
			DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(_leaderboardStatisticsCache.FileName),
			IsFetched = _leaderboardStatisticsCache.IsFetched,
			TotalEntries = _leaderboardStatisticsCache.Entries.Count,
			Statistics = (top switch
			{
				LeaderboardStatisticsLimitDdLive.Top1000 => _leaderboardStatisticsCache.Top1000ArrayStatistics,
				LeaderboardStatisticsLimitDdLive.Top100 => _leaderboardStatisticsCache.Top100ArrayStatistics,
				_ => _leaderboardStatisticsCache.Top10ArrayStatistics,
			}).ToGetArrayStatisticsDdLive(),
		};
	}
}
