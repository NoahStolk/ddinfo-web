using DevilDaggersInfo.Api.Main.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class LeaderboardStatisticsConverters
{
	private static GetArrayStatistic ToGetArrayStatistic(this ArrayStatistic arrayStatistic) => new()
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
