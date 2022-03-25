namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public record GetGlobalCustomLeaderboardEntry
{
	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public int Points { get; set; }

	public int Played { get; set; }
}
