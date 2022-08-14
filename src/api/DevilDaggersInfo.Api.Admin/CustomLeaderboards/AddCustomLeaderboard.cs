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

	public AddCustomLeaderboardCriteria EnemiesKilledCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria DaggersFiredCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteria DaggersHitCriteria { get; set; } = new();

	public AddCustomLeaderboardEnemyCriteria Skull1KillsCriteria { get; set; } = new();
}
