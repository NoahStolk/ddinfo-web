namespace DevilDaggersInfo.Web.Shared.Dto.Public.LeaderboardStatistics;

public record GetArrayStatistics
{
	public GetArrayStatistic Times { get; init; } = null!;
	public GetArrayStatistic Kills { get; init; } = null!;
	public GetArrayStatistic Gems { get; init; } = null!;
	public GetArrayStatistic DaggersFired { get; init; } = null!;
	public GetArrayStatistic DaggersHit { get; init; } = null!;
	public GetArrayStatistic Accuracy { get; init; } = null!;
}
