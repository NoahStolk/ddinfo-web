namespace DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;

public record GetArrayStatisticsDdLive
{
	public required GetArrayStatisticDdLive Times { get; init; }

	public required GetArrayStatisticDdLive Kills { get; init; }

	public required GetArrayStatisticDdLive Gems { get; init; }

	public required GetArrayStatisticDdLive DaggersFired { get; init; }

	public required GetArrayStatisticDdLive DaggersHit { get; init; }

	public required GetArrayStatisticDdLive Accuracy { get; init; }
}
