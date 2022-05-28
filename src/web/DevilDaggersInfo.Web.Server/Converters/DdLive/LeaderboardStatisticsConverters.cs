using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.Shared.Dto.DdLive.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DdLive;

public static class LeaderboardStatisticsConverters
{
	private static GetArrayStatisticDdLive ToGetArrayStatisticDdLive(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};

	public static GetArrayStatisticsDdLive ToGetArrayStatisticsDdLive(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToGetArrayStatisticDdLive(),
		DaggersFired = arrayStatistics.DaggersFired.ToGetArrayStatisticDdLive(),
		DaggersHit = arrayStatistics.DaggersHit.ToGetArrayStatisticDdLive(),
		Gems = arrayStatistics.Gems.ToGetArrayStatisticDdLive(),
		Kills = arrayStatistics.Kills.ToGetArrayStatisticDdLive(),
		Times = arrayStatistics.Times.ToGetArrayStatisticDdLive(),
	};
}
