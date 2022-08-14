using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("CustomLeaderboards")]
public class CustomLeaderboardEntity : IAuditable
{
	[Key]
	public int Id { get; init; }

	[Column("SpawnsetFileId")]
	public int SpawnsetId { get; set; }

	[ForeignKey(nameof(SpawnsetId))]
	public SpawnsetEntity Spawnset { get; set; } = null!;

	public CustomLeaderboardCategory Category { get; set; }

	public int TimeBronze { get; set; }

	public int TimeSilver { get; set; }

	public int TimeGolden { get; set; }

	public int TimeDevil { get; set; }

	public int TimeLeviathan { get; set; }

	public DateTime? DateLastPlayed { get; set; }

	public DateTime DateCreated { get; set; }

	public int TotalRunsSubmitted { get; set; }

	public bool IsFeatured { get; set; }

	public CustomLeaderboardCriteria GemsCollectedCriteria { get; set; } = new();

	public CustomLeaderboardCriteria GemsDespawnedCriteria { get; set; } = new();

	public CustomLeaderboardCriteria GemsEatenCriteria { get; set; } = new();

	public CustomLeaderboardCriteria EnemiesKilledCriteria { get; set; } = new();

	public CustomLeaderboardCriteria DaggersFiredCriteria { get; set; } = new();

	public CustomLeaderboardCriteria DaggersHitCriteria { get; set; } = new();

	public CustomLeaderboardCriteria HomingStoredCriteria { get; set; } = new();

	public CustomLeaderboardCriteria HomingEatenCriteria { get; set; } = new();

	public CustomLeaderboardEnemyCriteria Skull1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Skull2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Skull3KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Skull4KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria SpiderlingKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria SpiderEggKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Squid1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Squid2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Squid3KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria CentipedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria GigapedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria GhostpedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Spider1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria Spider2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria LeviathanKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria OrbKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteria ThornKillsCriteria { get; set; } = new();

	public List<CustomEntryEntity>? CustomEntries { get; set; }

	public CustomLeaderboardDagger? GetDaggerFromTime(int time)
		=> IsFeatured ? CustomLeaderboardUtils.GetDaggerFromTime(Category, time, TimeLeviathan, TimeDevil, TimeGolden, TimeSilver, TimeBronze) : null;
}
