namespace DevilDaggersInfo.Web.Shared.Dto.DdLive.LeaderboardStatistics;

public record GetArrayStatisticsDdLive
{
	public GetArrayStatisticDdLive Times { get; init; } = null!;
	public GetArrayStatisticDdLive Kills { get; init; } = null!;
	public GetArrayStatisticDdLive Gems { get; init; } = null!;
	public GetArrayStatisticDdLive DaggersFired { get; init; } = null!;
	public GetArrayStatisticDdLive DaggersHit { get; init; } = null!;
	public GetArrayStatisticDdLive Accuracy { get; init; } = null!;
}
