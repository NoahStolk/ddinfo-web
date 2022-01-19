namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerHistory
{
	public int? BestRank { get; init; }

	public bool HidePastUsernames { get; init; }

	public List<string> Usernames { get; init; } = new();

	public List<GetPlayerHistoryScoreEntry> ScoreHistory { get; init; } = new();

	public List<GetPlayerHistoryActivityEntry> ActivityHistory { get; init; } = new();

	public List<GetPlayerHistoryRankEntry> RankHistory { get; init; } = new();
}
