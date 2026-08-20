using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboard-statistics")]
[ApiController]
public sealed class LeaderboardStatisticsController(LeaderboardStatisticsCache leaderboardStatisticsCache) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetLeaderboardStatistics> GetLeaderboardStatistics()
	{
		return new GetLeaderboardStatistics
		{
			DateTime = leaderboardStatisticsCache.FileName == null ? DateTime.MinValue : HistoryUtils.HistoryFileNameToDateTime(leaderboardStatisticsCache.FileName),
			IsFetched = leaderboardStatisticsCache.IsFetched,
			TotalEntries = leaderboardStatisticsCache.EntryCount,
			DaggersStatistics = Enumerable.Range(0, LeaderboardStatisticsCache.StatDaggers.Count).Reverse().ToDictionary(i => LeaderboardStatisticsCache.StatDaggers[i].Name, i => leaderboardStatisticsCache.DaggersStatistics[i]),
			DeathsStatistics = leaderboardStatisticsCache.DeathsStatistics.OrderBy(kvp => kvp.Key.LeaderboardDeathType).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
			EnemiesStatistics = Enumerable.Range(0, LeaderboardStatisticsCache.StatEnemies.Count).Reverse().ToDictionary(i => LeaderboardStatisticsCache.StatEnemies[i].Name, i => leaderboardStatisticsCache.EnemiesStatistics[..(i + 1)].Sum()),
			TimesStatistics = leaderboardStatisticsCache.TimesStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			KillsStatistics = leaderboardStatisticsCache.KillsStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			GemsStatistics = leaderboardStatisticsCache.GemsStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			DaggersFiredStatistics = leaderboardStatisticsCache.DaggersFiredStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			DaggersHitStatistics = leaderboardStatisticsCache.DaggersHitStatistics.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			PlayersWithLevel1 = leaderboardStatisticsCache.PlayersWithLevel1,
			PlayersWithLevel2 = leaderboardStatisticsCache.PlayersWithLevel2,
			PlayersWithLevel3Or4 = leaderboardStatisticsCache.PlayersWithLevel3Or4,
			GlobalStatistics = leaderboardStatisticsCache.GlobalArrayStatistics.ToMainApi(),
		};
	}
}
