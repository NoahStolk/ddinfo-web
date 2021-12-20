using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/leaderboard-statistics")]
[ApiController]
public class LeaderboardStatisticsController : ControllerBase
{
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;

	public LeaderboardStatisticsController(LeaderboardStatisticsCache leaderboardStatisticsCache)
	{
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatistics> GetLeaderboardStatistics()
	{
		return new GetLeaderboardStatistics
		{
			DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryJsonFileNameToDateTime(_leaderboardStatisticsCache.FileName),
			IsFetched = _leaderboardStatisticsCache.IsFetched,
			TotalEntries = _leaderboardStatisticsCache.Entries.Count,
			DaggersStatistics = _leaderboardStatisticsCache.DaggersStatistics.OrderBy(kvp => kvp.Key.UnlockSecond).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
			DeathsStatistics = _leaderboardStatisticsCache.DeathsStatistics.OrderBy(kvp => kvp.Key.LeaderboardDeathType).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
			EnemiesStatistics = _leaderboardStatisticsCache.EnemiesStatistics.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
			TimesStatistics = _leaderboardStatisticsCache.TimesStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			KillsStatistics = _leaderboardStatisticsCache.KillsStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			GemsStatistics = _leaderboardStatisticsCache.GemsStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			DaggersFiredStatistics = _leaderboardStatisticsCache.DaggersFiredStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			DaggersHitStatistics = _leaderboardStatisticsCache.DaggersHitStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			PlayersWithLevel1 = _leaderboardStatisticsCache.PlayersWithLevel1,
			PlayersWithLevel2 = _leaderboardStatisticsCache.PlayersWithLevel2,
			PlayersWithLevel3Or4 = _leaderboardStatisticsCache.PlayersWithLevel3Or4,
			GlobalStatistics = _leaderboardStatisticsCache.GlobalArrayStatistics.ToGetArrayStatistics(),
		};
	}

	[HttpGet("ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatisticsDdLive> GetLeaderboardStatisticsDdLive()
	{
		return new GetLeaderboardStatisticsDdLive
		{
			DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryJsonFileNameToDateTime(_leaderboardStatisticsCache.FileName),
			IsFetched = _leaderboardStatisticsCache.IsFetched,
			TotalEntries = _leaderboardStatisticsCache.Entries.Count,
			Top1000Statistics = _leaderboardStatisticsCache.Top1000ArrayStatistics.ToGetArrayStatistics(),
		};
	}
}
