using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DdLiveApi = DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;

public static class LeaderboardStatisticsConverters
{
	public static DdLiveApi.GetArrayStatisticsDdLive ToGetArrayStatisticsDdLive(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToGetArrayStatisticDdLive(),
		DaggersFired = arrayStatistics.DaggersFired.ToGetArrayStatisticDdLive(),
		DaggersHit = arrayStatistics.DaggersHit.ToGetArrayStatisticDdLive(),
		Gems = arrayStatistics.Gems.ToGetArrayStatisticDdLive(),
		Kills = arrayStatistics.Kills.ToGetArrayStatisticDdLive(),
		Times = arrayStatistics.Times.ToGetArrayStatisticDdLive(),
	};

	private static DdLiveApi.GetArrayStatisticDdLive ToGetArrayStatisticDdLive(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};
}
