using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.Shared.Dto.Public.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class LeaderboardStatisticsConverters
{
	public static GetArrayStatistic ToGetArrayStatistic(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};

	public static GetArrayStatistics ToGetArrayStatistics(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToGetArrayStatistic(),
		DaggersFired = arrayStatistics.DaggersFired.ToGetArrayStatistic(),
		DaggersHit = arrayStatistics.DaggersHit.ToGetArrayStatistic(),
		Gems = arrayStatistics.Gems.ToGetArrayStatistic(),
		Kills = arrayStatistics.Kills.ToGetArrayStatistic(),
		Times = arrayStatistics.Times.ToGetArrayStatistic(),
	};
}
