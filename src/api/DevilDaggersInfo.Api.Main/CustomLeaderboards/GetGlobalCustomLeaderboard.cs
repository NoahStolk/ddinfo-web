namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetGlobalCustomLeaderboard
{
	public List<GetGlobalCustomLeaderboardEntry> Entries { get; init; } = new();

	public int TotalLeaderboards { get; init; }

	public int TotalPoints { get; init; }
}
