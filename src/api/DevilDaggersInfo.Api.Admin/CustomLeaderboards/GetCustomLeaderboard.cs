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

	public GetCustomLeaderboardCriteria GemsDespawnedCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria GemsEatenCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria EnemiesKilledCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria DaggersFiredCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria DaggersHitCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria HomingStoredCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria HomingEatenCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria Skull1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Skull2KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Skull3KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Skull4KillsCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria SpiderlingKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria SpiderEggKillsCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria Squid1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Squid2KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Squid3KillsCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria CentipedeKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria GigapedeKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria GhostpedeKillsCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria Spider1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria Spider2KillsCriteria { get; set; } = new();

	public GetCustomLeaderboardEnemyCriteria LeviathanKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria OrbKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardEnemyCriteria ThornKillsCriteria { get; set; } = new();
}
