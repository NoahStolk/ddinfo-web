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

	[Column("TimeBronze")]
	public int Bronze { get; set; }

	[Column("TimeSilver")]
	public int Silver { get; set; }

	[Column("TimeGolden")]
	public int Golden { get; set; }

	[Column("TimeDevil")]
	public int Devil { get; set; }

	[Column("TimeLeviathan")]
	public int Leviathan { get; set; }

	public DateTime? DateLastPlayed { get; set; }

	public DateTime DateCreated { get; set; }

	public int TotalRunsSubmitted { get; set; }

	public bool IsFeatured { get; set; }

	public CustomLeaderboardCriteriaEntityValue GemsCollectedCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue GemsDespawnedCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue GemsEatenCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue EnemiesKilledCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue DaggersFiredCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue DaggersHitCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue HomingStoredCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue HomingEatenCriteria { get; set; } = new();

	public CustomLeaderboardEnemyCriteriaEntityValue Skull1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Skull2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Skull3KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Skull4KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue SpiderlingKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue SpiderEggKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Squid1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Squid2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Squid3KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue CentipedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue GigapedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue GhostpedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Spider1KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue Spider2KillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue LeviathanKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue OrbKillsCriteria { get; set; } = new();
	public CustomLeaderboardEnemyCriteriaEntityValue ThornKillsCriteria { get; set; } = new();

	public List<CustomEntryEntity>? CustomEntries { get; set; }

	public CustomLeaderboardDagger? DaggerFromTime(int time)
		=> IsFeatured ? CustomLeaderboardUtils.GetDaggerFromTime(Category, time, Leviathan, Devil, Golden, Silver, Bronze) : null;
}
