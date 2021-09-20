namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetTotalCustomLeaderboardData
{
	public int CountDefault { get; init; }
	public int CountSpeedrun { get; init; }
	public int UniquePlayers { get; init; }
	public int UniqueScores { get; init; }
	public int TotalSubmits { get; init; }
}
