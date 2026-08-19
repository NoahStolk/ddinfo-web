namespace DevilDaggersInfo.Web.ApiSpec.DdLive.LeaderboardStatistics;

public sealed record GetArrayStatisticDdLive
{
	public required double Average { get; init; }

	public required double Median { get; init; }

	public required double Mode { get; init; }
}
