namespace DevilDaggersInfo.Web.ApiSpec.DdLive.LeaderboardStatistics;

public sealed record GetLeaderboardStatisticsDdLive
{
	public required DateTime DateTime { get; init; }

	public required bool IsFetched { get; init; }

	public required int TotalEntries { get; init; }

	public required GetArrayStatisticsDdLive Statistics { get; init; }
}
