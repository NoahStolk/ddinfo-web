namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerHistory
{
	public required int? BestRank { get; init; }

	public required bool HidePastUsernames { get; init; }

	public required List<string> Usernames { get; init; }

	public required List<PlayerHistoryScoreEntry> ScoreHistory { get; init; }

	public required List<PlayerHistoryActivityEntry> ActivityHistory { get; init; }

	public required List<PlayerHistoryRankEntry> RankHistory { get; init; }
}
