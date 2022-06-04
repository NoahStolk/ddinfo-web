namespace DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;

public record GetLeaderboardStatisticsDdLive
{
	public DateTime DateTime { get; init; }

	public bool IsFetched { get; init; }

	public int TotalEntries { get; init; }

	public GetArrayStatisticsDdLive Statistics { get; init; } = null!;

	[Obsolete("Use Statistics instead.")]
	public GetArrayStatisticsDdLive Top1000Statistics => Statistics;
}
