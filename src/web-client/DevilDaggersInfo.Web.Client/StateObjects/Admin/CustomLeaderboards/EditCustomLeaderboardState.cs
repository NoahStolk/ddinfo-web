using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.CustomLeaderboards;

public class EditCustomLeaderboardState : IStateObject<EditCustomLeaderboard>
{
	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggersState Daggers { get; set; } = new();

	[Required]
	public bool IsFeatured { get; set; }

	public AddCustomLeaderboardCriteriaState GemsCollectedCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState GemsDespawnedCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState GemsEatenCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState EnemiesKilledCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState DaggersFiredCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState DaggersHitCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState HomingStoredCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState HomingEatenCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState DeathTypeCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState TimeCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState LevelUpTime2Criteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState LevelUpTime3Criteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState LevelUpTime4Criteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState EnemiesAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Skull1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull3KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull4KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState SpiderlingKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState SpiderEggKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Squid1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Squid2KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Squid3KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState CentipedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState GigapedeKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState GhostpedeKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Spider1KillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Spider2KillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState LeviathanKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState OrbKillsCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState ThornKillsCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Skull1sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull2sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull3sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Skull4sAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState SpiderlingsAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState SpiderEggsAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Squid1sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Squid2sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Squid3sAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState CentipedesAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState GigapedesAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState GhostpedesAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState Spider1sAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState Spider2sAliveCriteria { get; set; } = new();

	public AddCustomLeaderboardCriteriaState LeviathansAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState OrbsAliveCriteria { get; set; } = new();
	public AddCustomLeaderboardCriteriaState ThornsAliveCriteria { get; set; } = new();

	public EditCustomLeaderboard ToModel() => new()
	{
		Category = Category,
		Daggers = Daggers.ToModel(),
		IsFeatured = IsFeatured,
		GemsCollectedCriteria = GemsCollectedCriteria.ToModel(),
		GemsDespawnedCriteria = GemsDespawnedCriteria.ToModel(),
		GemsEatenCriteria = GemsEatenCriteria.ToModel(),
		EnemiesKilledCriteria = EnemiesKilledCriteria.ToModel(),
		DaggersFiredCriteria = DaggersFiredCriteria.ToModel(),
		DaggersHitCriteria = DaggersHitCriteria.ToModel(),
		HomingStoredCriteria = HomingStoredCriteria.ToModel(),
		HomingEatenCriteria = HomingEatenCriteria.ToModel(),
		DeathTypeCriteria = DeathTypeCriteria.ToModel(),
		TimeCriteria = TimeCriteria.ToModel(),
		LevelUpTime2Criteria = LevelUpTime2Criteria.ToModel(),
		LevelUpTime3Criteria = LevelUpTime3Criteria.ToModel(),
		LevelUpTime4Criteria = LevelUpTime4Criteria.ToModel(),
		EnemiesAliveCriteria = EnemiesAliveCriteria.ToModel(),
		Skull1KillsCriteria = Skull1KillsCriteria.ToModel(),
		Skull2KillsCriteria = Skull2KillsCriteria.ToModel(),
		Skull3KillsCriteria = Skull3KillsCriteria.ToModel(),
		Skull4KillsCriteria = Skull4KillsCriteria.ToModel(),
		SpiderlingKillsCriteria = SpiderlingKillsCriteria.ToModel(),
		SpiderEggKillsCriteria = SpiderEggKillsCriteria.ToModel(),
		Squid1KillsCriteria = Squid1KillsCriteria.ToModel(),
		Squid2KillsCriteria = Squid2KillsCriteria.ToModel(),
		Squid3KillsCriteria = Squid3KillsCriteria.ToModel(),
		CentipedeKillsCriteria = CentipedeKillsCriteria.ToModel(),
		GigapedeKillsCriteria = GigapedeKillsCriteria.ToModel(),
		GhostpedeKillsCriteria = GhostpedeKillsCriteria.ToModel(),
		Spider1KillsCriteria = Spider1KillsCriteria.ToModel(),
		Spider2KillsCriteria = Spider2KillsCriteria.ToModel(),
		LeviathanKillsCriteria = LeviathanKillsCriteria.ToModel(),
		OrbKillsCriteria = OrbKillsCriteria.ToModel(),
		ThornKillsCriteria = ThornKillsCriteria.ToModel(),
		Skull1sAliveCriteria = Skull1sAliveCriteria.ToModel(),
		Skull2sAliveCriteria = Skull2sAliveCriteria.ToModel(),
		Skull3sAliveCriteria = Skull3sAliveCriteria.ToModel(),
		Skull4sAliveCriteria = Skull4sAliveCriteria.ToModel(),
		SpiderlingsAliveCriteria = SpiderlingsAliveCriteria.ToModel(),
		SpiderEggsAliveCriteria = SpiderEggsAliveCriteria.ToModel(),
		Squid1sAliveCriteria = Squid1sAliveCriteria.ToModel(),
		Squid2sAliveCriteria = Squid2sAliveCriteria.ToModel(),
		Squid3sAliveCriteria = Squid3sAliveCriteria.ToModel(),
		CentipedesAliveCriteria = CentipedesAliveCriteria.ToModel(),
		GigapedesAliveCriteria = GigapedesAliveCriteria.ToModel(),
		GhostpedesAliveCriteria = GhostpedesAliveCriteria.ToModel(),
		Spider1sAliveCriteria = Spider1sAliveCriteria.ToModel(),
		Spider2sAliveCriteria = Spider2sAliveCriteria.ToModel(),
		LeviathansAliveCriteria = LeviathansAliveCriteria.ToModel(),
		OrbsAliveCriteria = OrbsAliveCriteria.ToModel(),
		ThornsAliveCriteria = ThornsAliveCriteria.ToModel(),
	};
}
