using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public GetCustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public GetCustomLeaderboardCriteria GemsCollectedCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GemsDespawnedCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GemsEatenCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria EnemiesKilledCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria DaggersFiredCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria DaggersHitCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria HomingStoredCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria HomingEatenCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria DeathTypeCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria TimeCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime2Criteria { get; init; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime3Criteria { get; init; } = new();
	public GetCustomLeaderboardCriteria LevelUpTime4Criteria { get; init; } = new();
	public GetCustomLeaderboardCriteria EnemiesAliveCriteria { get; init; } = new();

	public GetCustomLeaderboardCriteria Skull1KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull2KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull3KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull4KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria SpiderlingKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria SpiderEggKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid1KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid2KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid3KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria CentipedeKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GigapedeKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GhostpedeKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Spider1KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Spider2KillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria LeviathanKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria OrbKillsCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria ThornKillsCriteria { get; init; } = new();

	public GetCustomLeaderboardCriteria Skull1sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull2sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull3sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Skull4sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria SpiderlingsAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria SpiderEggsAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid1sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid2sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Squid3sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria CentipedesAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GigapedesAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria GhostpedesAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Spider1sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria Spider2sAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria LeviathansAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria OrbsAliveCriteria { get; init; } = new();
	public GetCustomLeaderboardCriteria ThornsAliveCriteria { get; init; } = new();
}
