using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;
using DdLiveApi = DevilDaggersInfo.Web.ApiSpec.DdLive.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;

public static class LeaderboardStatisticsConverters
{
	public static DdLiveApi.GetArrayStatisticsDdLive ToDdLiveApi(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToDdLiveApi(),
		DaggersFired = arrayStatistics.DaggersFired.ToDdLiveApi(),
		DaggersHit = arrayStatistics.DaggersHit.ToDdLiveApi(),
		Gems = arrayStatistics.Gems.ToDdLiveApi(),
		Kills = arrayStatistics.Kills.ToDdLiveApi(),
		Times = arrayStatistics.Times.ToDdLiveApi(),
	};

	private static DdLiveApi.GetArrayStatisticDdLive ToDdLiveApi(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};
}
