using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

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
		DaggersFired = arrayStatistics.DaggersFired.ToGetArrayStatistic(),
		DaggersHit = arrayStatistics.DaggersHit.ToGetArrayStatistic(),
		Gems = arrayStatistics.Gems.ToGetArrayStatistic(),
		Kills = arrayStatistics.Kills.ToGetArrayStatistic(),
		Times = arrayStatistics.Times.ToGetArrayStatistic(),
	};
}
