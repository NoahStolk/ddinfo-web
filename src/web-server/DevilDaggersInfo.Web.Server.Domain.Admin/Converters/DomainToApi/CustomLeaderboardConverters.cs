using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class CustomLeaderboardConverters
{
	public static AdminApi.GetCustomLeaderboardForOverview ToAdminApiOverview(this CustomLeaderboardEntity customLeaderboard)
	{
		if (customLeaderboard.Spawnset == null)
			throw new InvalidOperationException("Spawnset is not included.");

		return new()
		{
			Id = customLeaderboard.Id,
			SpawnsetName = customLeaderboard.Spawnset.Name,
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
			RankSorting = customLeaderboard.RankSorting.ToAdminApi(),
		};
	}

	public static AdminApi.GetCustomLeaderboard ToAdminApi(this CustomLeaderboardEntity customLeaderboard) => new()
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
		RankSorting = customLeaderboard.RankSorting.ToAdminApi(),
		GemsCollectedCriteria = customLeaderboard.GemsCollectedCriteria.ToAdminApi(),
		GemsDespawnedCriteria = customLeaderboard.GemsDespawnedCriteria.ToAdminApi(),
		GemsEatenCriteria = customLeaderboard.GemsEatenCriteria.ToAdminApi(),
		EnemiesKilledCriteria = customLeaderboard.EnemiesKilledCriteria.ToAdminApi(),
		DaggersFiredCriteria = customLeaderboard.DaggersFiredCriteria.ToAdminApi(),
		DaggersHitCriteria = customLeaderboard.DaggersHitCriteria.ToAdminApi(),
		HomingStoredCriteria = customLeaderboard.HomingStoredCriteria.ToAdminApi(),
		HomingEatenCriteria = customLeaderboard.HomingEatenCriteria.ToAdminApi(),
		DeathTypeCriteria = customLeaderboard.DeathTypeCriteria.ToAdminApi(),
		TimeCriteria = customLeaderboard.TimeCriteria.ToAdminApi(),
		LevelUpTime2Criteria = customLeaderboard.LevelUpTime2Criteria.ToAdminApi(),
		LevelUpTime3Criteria = customLeaderboard.LevelUpTime3Criteria.ToAdminApi(),
		LevelUpTime4Criteria = customLeaderboard.LevelUpTime4Criteria.ToAdminApi(),
		EnemiesAliveCriteria = customLeaderboard.EnemiesAliveCriteria.ToAdminApi(),
		Skull1KillsCriteria = customLeaderboard.Skull1KillsCriteria.ToAdminApi(),
		Skull2KillsCriteria = customLeaderboard.Skull2KillsCriteria.ToAdminApi(),
		Skull3KillsCriteria = customLeaderboard.Skull3KillsCriteria.ToAdminApi(),
		Skull4KillsCriteria = customLeaderboard.Skull4KillsCriteria.ToAdminApi(),
		SpiderlingKillsCriteria = customLeaderboard.SpiderlingKillsCriteria.ToAdminApi(),
		SpiderEggKillsCriteria = customLeaderboard.SpiderEggKillsCriteria.ToAdminApi(),
		Squid1KillsCriteria = customLeaderboard.Squid1KillsCriteria.ToAdminApi(),
		Squid2KillsCriteria = customLeaderboard.Squid2KillsCriteria.ToAdminApi(),
		Squid3KillsCriteria = customLeaderboard.Squid3KillsCriteria.ToAdminApi(),
		CentipedeKillsCriteria = customLeaderboard.CentipedeKillsCriteria.ToAdminApi(),
		GigapedeKillsCriteria = customLeaderboard.GigapedeKillsCriteria.ToAdminApi(),
		GhostpedeKillsCriteria = customLeaderboard.GhostpedeKillsCriteria.ToAdminApi(),
		Spider1KillsCriteria = customLeaderboard.Spider1KillsCriteria.ToAdminApi(),
		Spider2KillsCriteria = customLeaderboard.Spider2KillsCriteria.ToAdminApi(),
		LeviathanKillsCriteria = customLeaderboard.LeviathanKillsCriteria.ToAdminApi(),
		OrbKillsCriteria = customLeaderboard.OrbKillsCriteria.ToAdminApi(),
		ThornKillsCriteria = customLeaderboard.ThornKillsCriteria.ToAdminApi(),
		Skull1sAliveCriteria = customLeaderboard.Skull1sAliveCriteria.ToAdminApi(),
		Skull2sAliveCriteria = customLeaderboard.Skull2sAliveCriteria.ToAdminApi(),
		Skull3sAliveCriteria = customLeaderboard.Skull3sAliveCriteria.ToAdminApi(),
		Skull4sAliveCriteria = customLeaderboard.Skull4sAliveCriteria.ToAdminApi(),
		SpiderlingsAliveCriteria = customLeaderboard.SpiderlingsAliveCriteria.ToAdminApi(),
		SpiderEggsAliveCriteria = customLeaderboard.SpiderEggsAliveCriteria.ToAdminApi(),
		Squid1sAliveCriteria = customLeaderboard.Squid1sAliveCriteria.ToAdminApi(),
		Squid2sAliveCriteria = customLeaderboard.Squid2sAliveCriteria.ToAdminApi(),
		Squid3sAliveCriteria = customLeaderboard.Squid3sAliveCriteria.ToAdminApi(),
		CentipedesAliveCriteria = customLeaderboard.CentipedesAliveCriteria.ToAdminApi(),
		GigapedesAliveCriteria = customLeaderboard.GigapedesAliveCriteria.ToAdminApi(),
		GhostpedesAliveCriteria = customLeaderboard.GhostpedesAliveCriteria.ToAdminApi(),
		Spider1sAliveCriteria = customLeaderboard.Spider1sAliveCriteria.ToAdminApi(),
		Spider2sAliveCriteria = customLeaderboard.Spider2sAliveCriteria.ToAdminApi(),
		LeviathansAliveCriteria = customLeaderboard.LeviathansAliveCriteria.ToAdminApi(),
		OrbsAliveCriteria = customLeaderboard.OrbsAliveCriteria.ToAdminApi(),
		ThornsAliveCriteria = customLeaderboard.ThornsAliveCriteria.ToAdminApi(),
	};

	private static AdminApi.GetCustomLeaderboardCriteria ToAdminApi(this CustomLeaderboardCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator.ToAdminApi(),
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToShortString() : null,
	};

	private static AdminApi.CustomLeaderboardRankSorting ToAdminApi(this Entities.Enums.CustomLeaderboardRankSorting category) => category switch
	{
		Entities.Enums.CustomLeaderboardRankSorting.TimeDesc => AdminApi.CustomLeaderboardRankSorting.TimeDesc,
		Entities.Enums.CustomLeaderboardRankSorting.TimeAsc => AdminApi.CustomLeaderboardRankSorting.TimeAsc,
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
}