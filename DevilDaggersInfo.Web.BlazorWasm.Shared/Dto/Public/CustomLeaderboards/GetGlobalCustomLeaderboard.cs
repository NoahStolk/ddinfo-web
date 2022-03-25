namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public record GetGlobalCustomLeaderboard
{
	public List<GetGlobalCustomLeaderboardEntry> Entries { get; init; } = new();
}
