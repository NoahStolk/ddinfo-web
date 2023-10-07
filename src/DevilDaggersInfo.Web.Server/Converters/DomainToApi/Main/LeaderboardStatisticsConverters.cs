using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class LeaderboardStatisticsConverters
{
	private static GetArrayStatistic ToMainApi(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};

	public static GetArrayStatistics ToMainApi(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToMainApi(),
		DaggersFired = arrayStatistics.DaggersFired.ToMainApi(),
		DaggersHit = arrayStatistics.DaggersHit.ToMainApi(),
		Gems = arrayStatistics.Gems.ToMainApi(),
		Kills = arrayStatistics.Kills.ToMainApi(),
		Times = arrayStatistics.Times.ToMainApi(),
	};
}
