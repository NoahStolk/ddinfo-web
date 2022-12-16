namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetGlobalCustomLeaderboard
{
	public required List<GetGlobalCustomLeaderboardEntry> Entries { get; init; }

	public required int TotalLeaderboards { get; init; }

	public required int TotalPoints { get; init; }

	public required int RankingMultiplier { get; init; }

	public required int LeviathanBonus { get; init; }

	public required int DevilBonus { get; init; }

	public required int GoldenBonus { get; init; }

	public required int SilverBonus { get; init; }

	public required int BronzeBonus { get; init; }

	public required int DefaultBonus { get; init; }
}
