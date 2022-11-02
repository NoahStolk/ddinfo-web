using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboard
{
	[Required]
	public int SpawnsetId { get; init; }

	[Required]
	public CustomLeaderboardCategory Category { get; init; }

	public AddCustomLeaderboardDaggers Daggers { get; init; } = new();

	[Required]
	public bool IsFeatured { get; init; }

	public AddCustomLeaderboardCriteria GemsCollectedCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria GemsDespawnedCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria GemsEatenCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria EnemiesKilledCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria DaggersFiredCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria DaggersHitCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria HomingStoredCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria HomingEatenCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria DeathTypeCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria TimeCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria LevelUpTime2Criteria { get; init; } = new();
	public AddCustomLeaderboardCriteria LevelUpTime3Criteria { get; init; } = new();
	public AddCustomLeaderboardCriteria LevelUpTime4Criteria { get; init; } = new();

	public AddCustomLeaderboardCriteria EnemiesAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Skull1KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull2KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull3KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull4KillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria SpiderlingKillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria SpiderEggKillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Squid1KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Squid2KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Squid3KillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria CentipedeKillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria GigapedeKillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria GhostpedeKillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Spider1KillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Spider2KillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria LeviathanKillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria OrbKillsCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria ThornKillsCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Skull1sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull2sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull3sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Skull4sAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria SpiderlingsAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria SpiderEggsAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Squid1sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Squid2sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Squid3sAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria CentipedesAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria GigapedesAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria GhostpedesAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria Spider1sAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria Spider2sAliveCriteria { get; init; } = new();

	public AddCustomLeaderboardCriteria LeviathansAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria OrbsAliveCriteria { get; init; } = new();
	public AddCustomLeaderboardCriteria ThornsAliveCriteria { get; init; } = new();
}
