namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public record AddCustomLeaderboard
{
	[Required]
	public int SpawnsetId { get; set; }

	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggers Daggers { get; set; } = new();

	[Required]
	public bool IsFeatured { get; set; }
}
