namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

public class GetArrayStatistics
{
	public GetArrayStatistic Times { get; init; } = null!;
	public GetArrayStatistic Kills { get; init; } = null!;
	public GetArrayStatistic Gems { get; init; } = null!;
	public GetArrayStatistic DaggersFired { get; init; } = null!;
	public GetArrayStatistic DaggersHit { get; init; } = null!;
}
