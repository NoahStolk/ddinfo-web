namespace DevilDaggersInfo.Api.Main.LeaderboardStatistics;

public record GetArrayStatistic
{
	public required double Average { get; init; }

	public required double Median { get; init; }

	public required double Mode { get; init; }
}
