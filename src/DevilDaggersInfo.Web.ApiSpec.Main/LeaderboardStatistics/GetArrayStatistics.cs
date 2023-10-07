namespace DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardStatistics;

public record GetArrayStatistics
{
	public required GetArrayStatistic Times { get; init; }

	public required GetArrayStatistic Kills { get; init; }

	public required GetArrayStatistic Gems { get; init; }

	public required GetArrayStatistic DaggersFired { get; init; }

	public required GetArrayStatistic DaggersHit { get; init; }

	public required GetArrayStatistic Accuracy { get; init; }
}
