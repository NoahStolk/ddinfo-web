using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class CustomLeaderboardConverters
{
	// ! Navigation property.
	public static AdminApi.GetCustomLeaderboardForOverview ToGetCustomLeaderboardForOverview(this CustomLeaderboardEntity customLeaderboard) => new()
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

	public static AdminApi.GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard) => new()
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

	private static AdminApi.GetCustomLeaderboardCriteria ToGetCustomLeaderboardCriteria(this CustomLeaderboardCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator.ToAdminApi(),
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToShortString() : null,
	};

	private static AdminApi.CustomLeaderboardCategory ToAdminApi(this Entities.Enums.CustomLeaderboardCategory category) => category switch
	{
		Entities.Enums.CustomLeaderboardCategory.Survival => AdminApi.CustomLeaderboardCategory.Survival,
		Entities.Enums.CustomLeaderboardCategory.TimeAttack => AdminApi.CustomLeaderboardCategory.TimeAttack,
		Entities.Enums.CustomLeaderboardCategory.Speedrun => AdminApi.CustomLeaderboardCategory.Speedrun,
		Entities.Enums.CustomLeaderboardCategory.Race => AdminApi.CustomLeaderboardCategory.Race,
		_ => throw new UnreachableException(),
	};

	private static AdminApi.CustomLeaderboardCriteriaOperator ToAdminApi(this CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		CustomLeaderboardCriteriaOperator.Any => AdminApi.CustomLeaderboardCriteriaOperator.Any,
		CustomLeaderboardCriteriaOperator.Equal => AdminApi.CustomLeaderboardCriteriaOperator.Equal,
		CustomLeaderboardCriteriaOperator.LessThan => AdminApi.CustomLeaderboardCriteriaOperator.LessThan,
		CustomLeaderboardCriteriaOperator.GreaterThan => AdminApi.CustomLeaderboardCriteriaOperator.GreaterThan,
		CustomLeaderboardCriteriaOperator.LessThanOrEqual => AdminApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => AdminApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		CustomLeaderboardCriteriaOperator.Modulo => AdminApi.CustomLeaderboardCriteriaOperator.Modulo,
		CustomLeaderboardCriteriaOperator.NotEqual => AdminApi.CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	public static Entities.Enums.CustomLeaderboardCategory ToDomain(this AdminApi.CustomLeaderboardCategory category) => category switch
	{
		AdminApi.CustomLeaderboardCategory.Survival => Entities.Enums.CustomLeaderboardCategory.Survival,
		AdminApi.CustomLeaderboardCategory.TimeAttack => Entities.Enums.CustomLeaderboardCategory.TimeAttack,
		AdminApi.CustomLeaderboardCategory.Speedrun => Entities.Enums.CustomLeaderboardCategory.Speedrun,
		AdminApi.CustomLeaderboardCategory.Race => Entities.Enums.CustomLeaderboardCategory.Race,
		_ => throw new UnreachableException(),
	};

	public static CustomLeaderboardCriteriaOperator ToDomain(this AdminApi.CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		AdminApi.CustomLeaderboardCriteriaOperator.Any => CustomLeaderboardCriteriaOperator.Any,
		AdminApi.CustomLeaderboardCriteriaOperator.Equal => CustomLeaderboardCriteriaOperator.Equal,
		AdminApi.CustomLeaderboardCriteriaOperator.LessThan => CustomLeaderboardCriteriaOperator.LessThan,
		AdminApi.CustomLeaderboardCriteriaOperator.GreaterThan => CustomLeaderboardCriteriaOperator.GreaterThan,
		AdminApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual => CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		AdminApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		AdminApi.CustomLeaderboardCriteriaOperator.Modulo => CustomLeaderboardCriteriaOperator.Modulo,
		AdminApi.CustomLeaderboardCriteriaOperator.NotEqual => CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};
}
