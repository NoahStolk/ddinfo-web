using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboard
{
	[Required]
	public int SpawnsetId { get; set; }

	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggers Daggers { get; set; } = new();

	[Required]
	public bool IsFeatured { get; set; }

	public AddCustomLeaderboardCriteria GemsCollectedCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria GemsDespawnedCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria GemsEatenCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria EnemiesKilledCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria DaggersFiredCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria DaggersHitCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria HomingStoredCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria HomingEatenCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria Skull1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Skull2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Skull3KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Skull4KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria SpiderlingKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria SpiderEggKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria Squid1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Squid2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Squid3KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria CentipedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria GigapedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria GhostpedeKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria Spider1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria Spider2KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria LeviathanKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria OrbKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardEnemyCriteria ThornKillsCriteria { get; set; } = new();
}
