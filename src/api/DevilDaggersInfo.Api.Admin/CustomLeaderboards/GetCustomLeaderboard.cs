using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public required GetCustomLeaderboardDaggers Daggers { get; init; }

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public required GetCustomLeaderboardCriteria GemsCollectedCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GemsDespawnedCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GemsEatenCriteria { get; init; }
	public required GetCustomLeaderboardCriteria EnemiesKilledCriteria { get; init; }
	public required GetCustomLeaderboardCriteria DaggersFiredCriteria { get; init; }
	public required GetCustomLeaderboardCriteria DaggersHitCriteria { get; init; }
	public required GetCustomLeaderboardCriteria HomingStoredCriteria { get; init; }
	public required GetCustomLeaderboardCriteria HomingEatenCriteria { get; init; }
	public required GetCustomLeaderboardCriteria DeathTypeCriteria { get; init; }
	public required GetCustomLeaderboardCriteria TimeCriteria { get; init; }
	public required GetCustomLeaderboardCriteria LevelUpTime2Criteria { get; init; }
	public required GetCustomLeaderboardCriteria LevelUpTime3Criteria { get; init; }
	public required GetCustomLeaderboardCriteria LevelUpTime4Criteria { get; init; }
	public required GetCustomLeaderboardCriteria EnemiesAliveCriteria { get; init; }

	public required GetCustomLeaderboardCriteria Skull1KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull2KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull3KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull4KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria SpiderlingKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria SpiderEggKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid1KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid2KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid3KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria CentipedeKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GigapedeKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GhostpedeKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Spider1KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Spider2KillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria LeviathanKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria OrbKillsCriteria { get; init; }
	public required GetCustomLeaderboardCriteria ThornKillsCriteria { get; init; }

	public required GetCustomLeaderboardCriteria Skull1sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull2sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull3sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Skull4sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria SpiderlingsAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria SpiderEggsAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid1sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid2sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Squid3sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria CentipedesAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GigapedesAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria GhostpedesAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Spider1sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria Spider2sAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria LeviathansAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria OrbsAliveCriteria { get; init; }
	public required GetCustomLeaderboardCriteria ThornsAliveCriteria { get; init; }
}
