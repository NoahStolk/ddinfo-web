namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public GetCustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
