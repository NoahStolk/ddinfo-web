using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public GetCustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public GetCustomLeaderboardCriteria GemsCollectedCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria EnemiesKilledCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria DaggersFiredCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria DaggersHitCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria Skull1KillsCriteria { get; set; } = new();
}
