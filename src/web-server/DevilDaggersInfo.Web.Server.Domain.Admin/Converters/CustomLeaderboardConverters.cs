using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class CustomLeaderboardConverters
{
	// ! Navigation property.
	public static GetCustomLeaderboardForOverview ToGetCustomLeaderboardForOverview(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.Spawnset!.Name,
		Daggers = new()
		{
			Bronze = customLeaderboard.Bronze.ToSecondsTime(),
			Silver = customLeaderboard.Silver.ToSecondsTime(),
			Golden = customLeaderboard.Golden.ToSecondsTime(),
			Devil = customLeaderboard.Devil.ToSecondsTime(),
			Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
		},
		IsFeatured = customLeaderboard.IsFeatured,
		DateCreated = customLeaderboard.DateCreated,
		Category = customLeaderboard.Category.ToAdminApi(),
	};

	public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetId = customLeaderboard.SpawnsetId,
		Daggers = new()
		{
			Bronze = customLeaderboard.Bronze.ToSecondsTime(),
			Silver = customLeaderboard.Silver.ToSecondsTime(),
			Golden = customLeaderboard.Golden.ToSecondsTime(),
			Devil = customLeaderboard.Devil.ToSecondsTime(),
			Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
		},
		IsFeatured = customLeaderboard.IsFeatured,
		Category = customLeaderboard.Category.ToAdminApi(),
		GemsCollectedCriteria = customLeaderboard.GemsCollectedCriteria.ToGetCustomLeaderboardCriteria(),
		GemsDespawnedCriteria = customLeaderboard.GemsDespawnedCriteria.ToGetCustomLeaderboardCriteria(),
		GemsEatenCriteria = customLeaderboard.GemsEatenCriteria.ToGetCustomLeaderboardCriteria(),
		EnemiesKilledCriteria = customLeaderboard.EnemiesKilledCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersFiredCriteria = customLeaderboard.DaggersFiredCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersHitCriteria = customLeaderboard.DaggersHitCriteria.ToGetCustomLeaderboardCriteria(),
		HomingStoredCriteria = customLeaderboard.HomingStoredCriteria.ToGetCustomLeaderboardCriteria(),
		HomingEatenCriteria = customLeaderboard.HomingEatenCriteria.ToGetCustomLeaderboardCriteria(),
		DeathTypeCriteria = customLeaderboard.DeathTypeCriteria.ToGetCustomLeaderboardCriteria(),
		TimeCriteria = customLeaderboard.TimeCriteria.ToGetCustomLeaderboardCriteria(),
		LevelUpTime2Criteria = customLeaderboard.LevelUpTime2Criteria.ToGetCustomLeaderboardCriteria(),
		LevelUpTime3Criteria = customLeaderboard.LevelUpTime3Criteria.ToGetCustomLeaderboardCriteria(),
		LevelUpTime4Criteria = customLeaderboard.LevelUpTime4Criteria.ToGetCustomLeaderboardCriteria(),
		EnemiesAliveCriteria = customLeaderboard.EnemiesAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Skull1KillsCriteria = customLeaderboard.Skull1KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Skull2KillsCriteria = customLeaderboard.Skull2KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Skull3KillsCriteria = customLeaderboard.Skull3KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Skull4KillsCriteria = customLeaderboard.Skull4KillsCriteria.ToGetCustomLeaderboardCriteria(),
		SpiderlingKillsCriteria = customLeaderboard.SpiderlingKillsCriteria.ToGetCustomLeaderboardCriteria(),
		SpiderEggKillsCriteria = customLeaderboard.SpiderEggKillsCriteria.ToGetCustomLeaderboardCriteria(),
		Squid1KillsCriteria = customLeaderboard.Squid1KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Squid2KillsCriteria = customLeaderboard.Squid2KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Squid3KillsCriteria = customLeaderboard.Squid3KillsCriteria.ToGetCustomLeaderboardCriteria(),
		CentipedeKillsCriteria = customLeaderboard.CentipedeKillsCriteria.ToGetCustomLeaderboardCriteria(),
		GigapedeKillsCriteria = customLeaderboard.GigapedeKillsCriteria.ToGetCustomLeaderboardCriteria(),
		GhostpedeKillsCriteria = customLeaderboard.GhostpedeKillsCriteria.ToGetCustomLeaderboardCriteria(),
		Spider1KillsCriteria = customLeaderboard.Spider1KillsCriteria.ToGetCustomLeaderboardCriteria(),
		Spider2KillsCriteria = customLeaderboard.Spider2KillsCriteria.ToGetCustomLeaderboardCriteria(),
		LeviathanKillsCriteria = customLeaderboard.LeviathanKillsCriteria.ToGetCustomLeaderboardCriteria(),
		OrbKillsCriteria = customLeaderboard.OrbKillsCriteria.ToGetCustomLeaderboardCriteria(),
		ThornKillsCriteria = customLeaderboard.ThornKillsCriteria.ToGetCustomLeaderboardCriteria(),
		Skull1sAliveCriteria = customLeaderboard.Skull1sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Skull2sAliveCriteria = customLeaderboard.Skull2sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Skull3sAliveCriteria = customLeaderboard.Skull3sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Skull4sAliveCriteria = customLeaderboard.Skull4sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		SpiderlingsAliveCriteria = customLeaderboard.SpiderlingsAliveCriteria.ToGetCustomLeaderboardCriteria(),
		SpiderEggsAliveCriteria = customLeaderboard.SpiderEggsAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Squid1sAliveCriteria = customLeaderboard.Squid1sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Squid2sAliveCriteria = customLeaderboard.Squid2sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Squid3sAliveCriteria = customLeaderboard.Squid3sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		CentipedesAliveCriteria = customLeaderboard.CentipedesAliveCriteria.ToGetCustomLeaderboardCriteria(),
		GigapedesAliveCriteria = customLeaderboard.GigapedesAliveCriteria.ToGetCustomLeaderboardCriteria(),
		GhostpedesAliveCriteria = customLeaderboard.GhostpedesAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Spider1sAliveCriteria = customLeaderboard.Spider1sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		Spider2sAliveCriteria = customLeaderboard.Spider2sAliveCriteria.ToGetCustomLeaderboardCriteria(),
		LeviathansAliveCriteria = customLeaderboard.LeviathansAliveCriteria.ToGetCustomLeaderboardCriteria(),
		OrbsAliveCriteria = customLeaderboard.OrbsAliveCriteria.ToGetCustomLeaderboardCriteria(),
		ThornsAliveCriteria = customLeaderboard.ThornsAliveCriteria.ToGetCustomLeaderboardCriteria(),
	};

	private static GetCustomLeaderboardCriteria ToGetCustomLeaderboardCriteria(this CustomLeaderboardCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator.ToAdminApi(),
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToShortString() : null,
	};

	private static CustomLeaderboardCategory ToAdminApi(this Types.Web.CustomLeaderboardCategory category) => category switch
	{
		Types.Web.CustomLeaderboardCategory.Survival => CustomLeaderboardCategory.Survival,
		Types.Web.CustomLeaderboardCategory.TimeAttack => CustomLeaderboardCategory.TimeAttack,
		Types.Web.CustomLeaderboardCategory.Speedrun => CustomLeaderboardCategory.Speedrun,
		Types.Web.CustomLeaderboardCategory.Race => CustomLeaderboardCategory.Race,
		_ => throw new UnreachableException(),
	};

	public static Types.Web.CustomLeaderboardCategory ToDomain(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => Types.Web.CustomLeaderboardCategory.Survival,
		CustomLeaderboardCategory.TimeAttack => Types.Web.CustomLeaderboardCategory.TimeAttack,
		CustomLeaderboardCategory.Speedrun => Types.Web.CustomLeaderboardCategory.Speedrun,
		CustomLeaderboardCategory.Race => Types.Web.CustomLeaderboardCategory.Race,
		_ => throw new UnreachableException(),
	};

	private static CustomLeaderboardDagger ToAdminApi(this Types.Web.CustomLeaderboardDagger dagger) => dagger switch
	{
		Types.Web.CustomLeaderboardDagger.Default => CustomLeaderboardDagger.Default,
		Types.Web.CustomLeaderboardDagger.Bronze => CustomLeaderboardDagger.Bronze,
		Types.Web.CustomLeaderboardDagger.Silver => CustomLeaderboardDagger.Silver,
		Types.Web.CustomLeaderboardDagger.Golden => CustomLeaderboardDagger.Golden,
		Types.Web.CustomLeaderboardDagger.Devil => CustomLeaderboardDagger.Devil,
		Types.Web.CustomLeaderboardDagger.Leviathan => CustomLeaderboardDagger.Leviathan,
		_ => throw new UnreachableException(),
	};

	private static CustomLeaderboardCriteriaType ToAdminApi(this Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GemsCollected => CustomLeaderboardCriteriaType.GemsCollected,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GemsDespawned => CustomLeaderboardCriteriaType.GemsDespawned,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GemsEaten => CustomLeaderboardCriteriaType.GemsEaten,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.EnemiesKilled => CustomLeaderboardCriteriaType.EnemiesKilled,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.DaggersFired => CustomLeaderboardCriteriaType.DaggersFired,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.DaggersHit => CustomLeaderboardCriteriaType.DaggersHit,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.HomingStored => CustomLeaderboardCriteriaType.HomingStored,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.HomingEaten => CustomLeaderboardCriteriaType.HomingEaten,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull1Kills => CustomLeaderboardCriteriaType.Skull1Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull2Kills => CustomLeaderboardCriteriaType.Skull2Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull3Kills => CustomLeaderboardCriteriaType.Skull3Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull4Kills => CustomLeaderboardCriteriaType.Skull4Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.SpiderlingKills => CustomLeaderboardCriteriaType.SpiderlingKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.SpiderEggKills => CustomLeaderboardCriteriaType.SpiderEggKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid1Kills => CustomLeaderboardCriteriaType.Squid1Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid2Kills => CustomLeaderboardCriteriaType.Squid2Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid3Kills => CustomLeaderboardCriteriaType.Squid3Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.CentipedeKills => CustomLeaderboardCriteriaType.CentipedeKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GigapedeKills => CustomLeaderboardCriteriaType.GigapedeKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GhostpedeKills => CustomLeaderboardCriteriaType.GhostpedeKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Spider1Kills => CustomLeaderboardCriteriaType.Spider1Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Spider2Kills => CustomLeaderboardCriteriaType.Spider2Kills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.LeviathanKills => CustomLeaderboardCriteriaType.LeviathanKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.OrbKills => CustomLeaderboardCriteriaType.OrbKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.ThornKills => CustomLeaderboardCriteriaType.ThornKills,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull1sAlive => CustomLeaderboardCriteriaType.Skull1sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull2sAlive => CustomLeaderboardCriteriaType.Skull2sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull3sAlive => CustomLeaderboardCriteriaType.Skull3sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Skull4sAlive => CustomLeaderboardCriteriaType.Skull4sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.SpiderlingsAlive => CustomLeaderboardCriteriaType.SpiderlingsAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.SpiderEggsAlive => CustomLeaderboardCriteriaType.SpiderEggsAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid1sAlive => CustomLeaderboardCriteriaType.Squid1sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid2sAlive => CustomLeaderboardCriteriaType.Squid2sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Squid3sAlive => CustomLeaderboardCriteriaType.Squid3sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.CentipedesAlive => CustomLeaderboardCriteriaType.CentipedesAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GigapedesAlive => CustomLeaderboardCriteriaType.GigapedesAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.GhostpedesAlive => CustomLeaderboardCriteriaType.GhostpedesAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Spider1sAlive => CustomLeaderboardCriteriaType.Spider1sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Spider2sAlive => CustomLeaderboardCriteriaType.Spider2sAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.LeviathansAlive => CustomLeaderboardCriteriaType.LeviathansAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.OrbsAlive => CustomLeaderboardCriteriaType.OrbsAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.ThornsAlive => CustomLeaderboardCriteriaType.ThornsAlive,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.DeathType => CustomLeaderboardCriteriaType.DeathType,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.Time => CustomLeaderboardCriteriaType.Time,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.LevelUpTime2 => CustomLeaderboardCriteriaType.LevelUpTime2,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.LevelUpTime3 => CustomLeaderboardCriteriaType.LevelUpTime3,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.LevelUpTime4 => CustomLeaderboardCriteriaType.LevelUpTime4,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaType.EnemiesAlive => CustomLeaderboardCriteriaType.EnemiesAlive,
		_ => throw new UnreachableException(),
	};

	private static CustomLeaderboardCriteriaOperator ToAdminApi(this Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Any => CustomLeaderboardCriteriaOperator.Any,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Equal => CustomLeaderboardCriteriaOperator.Equal,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.LessThan => CustomLeaderboardCriteriaOperator.LessThan,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.GreaterThan => CustomLeaderboardCriteriaOperator.GreaterThan,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.LessThanOrEqual => CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Modulo => CustomLeaderboardCriteriaOperator.Modulo,
		Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.NotEqual => CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	public static Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator ToDomain(this CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		CustomLeaderboardCriteriaOperator.Any => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Any,
		CustomLeaderboardCriteriaOperator.Equal => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Equal,
		CustomLeaderboardCriteriaOperator.LessThan => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.LessThan,
		CustomLeaderboardCriteriaOperator.GreaterThan => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.GreaterThan,
		CustomLeaderboardCriteriaOperator.LessThanOrEqual => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		CustomLeaderboardCriteriaOperator.Modulo => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.Modulo,
		CustomLeaderboardCriteriaOperator.NotEqual => Types.Core.CustomLeaderboards.CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};
}
