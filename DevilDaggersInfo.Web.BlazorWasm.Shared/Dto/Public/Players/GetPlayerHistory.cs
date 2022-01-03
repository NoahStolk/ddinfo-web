using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerHistory
{
	public int? BestRank { get; init; }

	public bool HidePastUsernames { get; init; }

	public List<string> Usernames { get; init; } = new();

	public List<GetEntryHistory> History { get; init; } = new();

	public List<GetPlayerActivity> Activity { get; init; } = new();
}
