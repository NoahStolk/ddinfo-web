using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Converters.Public;
using DevilDaggersInfo.Web.Shared.Dto.Public.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

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
			DateTime = _leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(_leaderboardStatisticsCache.FileName),
			IsFetched = _leaderboardStatisticsCache.IsFetched,
			TotalEntries = _leaderboardStatisticsCache.Entries.Count,
			DaggersStatistics = Enumerable.Range(0, LeaderboardStatisticsCache.StatDaggers.Count).Reverse().ToDictionary(i => LeaderboardStatisticsCache.StatDaggers[i].Name, i => _leaderboardStatisticsCache.DaggersStatistics[i]),
			DeathsStatistics = _leaderboardStatisticsCache.DeathsStatistics.OrderBy(kvp => kvp.Key.LeaderboardDeathType).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
			EnemiesStatistics = Enumerable.Range(0, LeaderboardStatisticsCache.StatEnemies.Count).Reverse().ToDictionary(i => LeaderboardStatisticsCache.StatEnemies[i].Name, i => _leaderboardStatisticsCache.EnemiesStatistics[..(i + 1)].Sum()),
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
}
