namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class GlobalCustomLeaderboard
{
	public List<GlobalCustomLeaderboardEntry> Entries { get; init; } = new();

	public int TotalLeaderboards { get; init; }

	public int TotalPoints { get; init; }
}
