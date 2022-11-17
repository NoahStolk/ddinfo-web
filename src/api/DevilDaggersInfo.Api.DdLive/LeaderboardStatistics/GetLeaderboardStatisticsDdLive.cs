namespace DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;

public record GetLeaderboardStatisticsDdLive
{
	public DateTime DateTime { get; init; }

	public bool IsFetched { get; init; }

	public int TotalEntries { get; init; }

	public required GetArrayStatisticsDdLive Statistics { get; init; }
}
