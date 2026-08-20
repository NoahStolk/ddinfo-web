using DevilDaggersInfo.Web.ApiSpec.DdLive.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/leaderboard-statistics")]
[ApiController]
public sealed class LeaderboardStatisticsController(LeaderboardStatisticsCache leaderboardStatisticsCache) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLive(LeaderboardStatisticsLimitDdLive? top)
	{
		return new GetLeaderboardStatisticsDdLive
		{
			DateTime = leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(leaderboardStatisticsCache.FileName),
			IsFetched = leaderboardStatisticsCache.IsFetched,
			TotalEntries = leaderboardStatisticsCache.EntryCount,
			Statistics = (top switch
			{
				LeaderboardStatisticsLimitDdLive.Top1000 => leaderboardStatisticsCache.Top1000ArrayStatistics,
				LeaderboardStatisticsLimitDdLive.Top100 => leaderboardStatisticsCache.Top100ArrayStatistics,
				LeaderboardStatisticsLimitDdLive.Top10 => leaderboardStatisticsCache.Top10ArrayStatistics,
				_ => leaderboardStatisticsCache.GlobalArrayStatistics,
			}).ToDdLiveApi(),
		};
	}
}
