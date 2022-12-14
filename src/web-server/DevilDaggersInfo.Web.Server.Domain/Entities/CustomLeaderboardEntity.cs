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
	public SpawnsetEntity Spawnset { get; set; } = null!; // TODO: Nullable.

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

	public DateTime DateCreated { get; init; }

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
	public CustomLeaderboardCriteriaEntityValue DeathTypeCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue TimeCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue LevelUpTime2Criteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue LevelUpTime3Criteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue LevelUpTime4Criteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue EnemiesAliveCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue Skull1KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull2KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull3KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull4KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue SpiderlingKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue SpiderEggKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid1KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid2KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid3KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue CentipedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue GigapedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue GhostpedeKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Spider1KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Spider2KillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue LeviathanKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue OrbKillsCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue ThornKillsCriteria { get; set; } = new();

	public CustomLeaderboardCriteriaEntityValue Skull1sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull2sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull3sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Skull4sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue SpiderlingsAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue SpiderEggsAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid1sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid2sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Squid3sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue CentipedesAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue GigapedesAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue GhostpedesAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Spider1sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue Spider2sAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue LeviathansAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue OrbsAliveCriteria { get; set; } = new();
	public CustomLeaderboardCriteriaEntityValue ThornsAliveCriteria { get; set; } = new();

	public List<CustomEntryEntity>? CustomEntries { get; set; }

	public CustomLeaderboardDagger? DaggerFromTime(int time)
		=> IsFeatured ? CustomLeaderboardUtils.GetDaggerFromTime(Category, time, Leviathan, Devil, Golden, Silver, Bronze) : null;
}
