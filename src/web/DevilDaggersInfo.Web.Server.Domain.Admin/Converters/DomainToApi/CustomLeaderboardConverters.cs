using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
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
			Daggers = customLeaderboard.ToAdminApiDaggers(),
			IsFeatured = customLeaderboard.IsFeatured,
			DateCreated = customLeaderboard.DateCreated,
			RankSorting = customLeaderboard.RankSorting.ToAdminApi(),
		};
	}

	public static AdminApi.GetCustomLeaderboard ToAdminApi(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetId = customLeaderboard.SpawnsetId,
		Daggers = customLeaderboard.ToAdminApiDaggers(),
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

	private static AdminApi.GetCustomLeaderboardDaggers ToAdminApiDaggers(this CustomLeaderboardEntity customLeaderboard)
	{
		bool isTime = customLeaderboard.RankSorting.IsTime();
		return new()
		{
			Bronze = isTime ? customLeaderboard.Bronze.ToSecondsTime() : customLeaderboard.Bronze,
			Silver = isTime ? customLeaderboard.Silver.ToSecondsTime() : customLeaderboard.Silver,
			Golden = isTime ? customLeaderboard.Golden.ToSecondsTime() : customLeaderboard.Golden,
			Devil = isTime ? customLeaderboard.Devil.ToSecondsTime() : customLeaderboard.Devil,
			Leviathan = isTime ? customLeaderboard.Leviathan.ToSecondsTime() : customLeaderboard.Leviathan,
		};
	}

	private static AdminApi.GetCustomLeaderboardCriteria ToAdminApi(this CustomLeaderboardCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator.ToAdminApi(),
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToShortString() : null,
	};

	private static AdminApi.CustomLeaderboardRankSorting ToAdminApi(this Entities.Enums.CustomLeaderboardRankSorting category) => category switch
	{
		Entities.Enums.CustomLeaderboardRankSorting.TimeDesc => AdminApi.CustomLeaderboardRankSorting.TimeDesc,
		Entities.Enums.CustomLeaderboardRankSorting.TimeAsc => AdminApi.CustomLeaderboardRankSorting.TimeAsc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsCollectedDesc => AdminApi.CustomLeaderboardRankSorting.GemsCollectedDesc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsCollectedAsc => AdminApi.CustomLeaderboardRankSorting.GemsCollectedAsc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsDespawnedDesc => AdminApi.CustomLeaderboardRankSorting.GemsDespawnedDesc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsDespawnedAsc => AdminApi.CustomLeaderboardRankSorting.GemsDespawnedAsc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsEatenDesc => AdminApi.CustomLeaderboardRankSorting.GemsEatenDesc,
		Entities.Enums.CustomLeaderboardRankSorting.GemsEatenAsc => AdminApi.CustomLeaderboardRankSorting.GemsEatenAsc,
		Entities.Enums.CustomLeaderboardRankSorting.EnemiesKilledDesc => AdminApi.CustomLeaderboardRankSorting.EnemiesKilledDesc,
		Entities.Enums.CustomLeaderboardRankSorting.EnemiesKilledAsc => AdminApi.CustomLeaderboardRankSorting.EnemiesKilledAsc,
		Entities.Enums.CustomLeaderboardRankSorting.EnemiesAliveDesc => AdminApi.CustomLeaderboardRankSorting.EnemiesAliveDesc,
		Entities.Enums.CustomLeaderboardRankSorting.EnemiesAliveAsc => AdminApi.CustomLeaderboardRankSorting.EnemiesAliveAsc,
		Entities.Enums.CustomLeaderboardRankSorting.HomingStoredDesc => AdminApi.CustomLeaderboardRankSorting.HomingStoredDesc,
		Entities.Enums.CustomLeaderboardRankSorting.HomingStoredAsc => AdminApi.CustomLeaderboardRankSorting.HomingStoredAsc,
		Entities.Enums.CustomLeaderboardRankSorting.HomingEatenDesc => AdminApi.CustomLeaderboardRankSorting.HomingEatenDesc,
		Entities.Enums.CustomLeaderboardRankSorting.HomingEatenAsc => AdminApi.CustomLeaderboardRankSorting.HomingEatenAsc,
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
