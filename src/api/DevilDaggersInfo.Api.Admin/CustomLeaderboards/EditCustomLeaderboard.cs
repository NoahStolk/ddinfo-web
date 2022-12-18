using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record EditCustomLeaderboard
{
	public required CustomLeaderboardCategory Category { get; init; }

	public required AddCustomLeaderboardDaggers Daggers { get; init; }

	public required bool IsFeatured { get; init; }

	public required AddCustomLeaderboardCriteria GemsCollectedCriteria { get; init; }

	public required AddCustomLeaderboardCriteria GemsDespawnedCriteria { get; init; }

	public required AddCustomLeaderboardCriteria GemsEatenCriteria { get; init; }

	public required AddCustomLeaderboardCriteria EnemiesKilledCriteria { get; init; }

	public required AddCustomLeaderboardCriteria DaggersFiredCriteria { get; init; }

	public required AddCustomLeaderboardCriteria DaggersHitCriteria { get; init; }

	public required AddCustomLeaderboardCriteria HomingStoredCriteria { get; init; }

	public required AddCustomLeaderboardCriteria HomingEatenCriteria { get; init; }

	public required AddCustomLeaderboardCriteria DeathTypeCriteria { get; init; }

	public required AddCustomLeaderboardCriteria TimeCriteria { get; init; }

	public required AddCustomLeaderboardCriteria LevelUpTime2Criteria { get; init; }
	public required AddCustomLeaderboardCriteria LevelUpTime3Criteria { get; init; }
	public required AddCustomLeaderboardCriteria LevelUpTime4Criteria { get; init; }

	public required AddCustomLeaderboardCriteria EnemiesAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Skull1KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull2KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull3KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull4KillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria SpiderlingKillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria SpiderEggKillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Squid1KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Squid2KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Squid3KillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria CentipedeKillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria GigapedeKillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria GhostpedeKillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Spider1KillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Spider2KillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria LeviathanKillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria OrbKillsCriteria { get; init; }
	public required AddCustomLeaderboardCriteria ThornKillsCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Skull1sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull2sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull3sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Skull4sAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria SpiderlingsAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria SpiderEggsAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Squid1sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Squid2sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Squid3sAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria CentipedesAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria GigapedesAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria GhostpedesAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria Spider1sAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria Spider2sAliveCriteria { get; init; }

	public required AddCustomLeaderboardCriteria LeviathansAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria OrbsAliveCriteria { get; init; }
	public required AddCustomLeaderboardCriteria ThornsAliveCriteria { get; init; }
}
