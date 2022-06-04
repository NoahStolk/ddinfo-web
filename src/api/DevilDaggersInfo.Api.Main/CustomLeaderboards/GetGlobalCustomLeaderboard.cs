namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetGlobalCustomLeaderboard
{
	public List<GetGlobalCustomLeaderboardEntry> Entries { get; init; } = new();

	public int TotalLeaderboards { get; init; }

	public int TotalPoints { get; init; }

	public int RankingMultiplier { get; init; }

	public int LeviathanBonus { get; init; }

	public int DevilBonus { get; init; }

	public int GoldenBonus { get; init; }

	public int SilverBonus { get; init; }

	public int BronzeBonus { get; init; }

	public int DefaultBonus { get; init; }
}
