namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

public class GetLeaderboardStatisticsDdLive
{
	public DateTime DateTime { get; init; }
	public bool IsFetched { get; init; }
	public int TotalEntries { get; init; }
	public GetArrayStatistics Top1000Statistics { get; init; } = null!;
}
