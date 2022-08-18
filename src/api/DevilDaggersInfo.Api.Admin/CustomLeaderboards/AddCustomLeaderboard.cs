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

	public AddCustomLeaderboardCriteria Skull1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Skull2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Skull3KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Skull4KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria SpiderlingKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria SpiderEggKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria Squid1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Squid2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Squid3KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria CentipedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria GigapedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria GhostpedeKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria Spider1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria Spider2KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria LeviathanKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria OrbKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteria ThornKillsCriteria { get; set; } = new();
}
