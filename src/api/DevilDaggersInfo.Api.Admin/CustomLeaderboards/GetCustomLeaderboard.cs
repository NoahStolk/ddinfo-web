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
	public GetCustomLeaderboardCriteria DeathTypeCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria TimeCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime2Criteria { get; set; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime3Criteria { get; set; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime4Criteria { get; set; } = new();
	public GetCustomLeaderboardCriteria EnemiesAliveCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria Skull1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull2KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull3KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull4KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria SpiderlingKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria SpiderEggKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid2KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid3KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria CentipedeKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria GigapedeKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria GhostpedeKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Spider1KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Spider2KillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria LeviathanKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria OrbKillsCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria ThornKillsCriteria { get; set; } = new();

	public GetCustomLeaderboardCriteria Skull1sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull2sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull3sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Skull4sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria SpiderlingsAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria SpiderEggsAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid1sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid2sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Squid3sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria CentipedesAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria GigapedesAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria GhostpedesAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Spider1sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria Spider2sAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria LeviathansAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria OrbsAliveCriteria { get; set; } = new();
	public GetCustomLeaderboardCriteria ThornsAliveCriteria { get; set; } = new();
}
