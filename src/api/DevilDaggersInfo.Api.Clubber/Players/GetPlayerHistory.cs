namespace DevilDaggersInfo.Api.Clubber.Players;

public record GetPlayerHistory
{
	public required int? BestRank { get; init; }

	public required bool HidePastUsernames { get; init; }

	public required List<string> Usernames { get; init; }

	public required List<GetPlayerHistoryScoreEntry> ScoreHistory { get; init; }

	public required List<GetPlayerHistoryActivityEntry> ActivityHistory { get; init; }

	public required List<GetPlayerHistoryRankEntry> RankHistory { get; init; }
}
