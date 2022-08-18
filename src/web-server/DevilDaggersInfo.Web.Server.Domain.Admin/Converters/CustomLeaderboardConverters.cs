using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardForOverview ToGetCustomLeaderboardForOverview(this CustomLeaderboardEntity customLeaderboard) => new()
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
		Category = customLeaderboard.Category,
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
		Category = customLeaderboard.Category,
		GemsCollectedCriteria = customLeaderboard.GemsCollectedCriteria.ToGetCustomLeaderboardCriteria(),
		GemsDespawnedCriteria = customLeaderboard.GemsDespawnedCriteria.ToGetCustomLeaderboardCriteria(),
		GemsEatenCriteria = customLeaderboard.GemsEatenCriteria.ToGetCustomLeaderboardCriteria(),
		EnemiesKilledCriteria = customLeaderboard.EnemiesKilledCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersFiredCriteria = customLeaderboard.DaggersFiredCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersHitCriteria = customLeaderboard.DaggersHitCriteria.ToGetCustomLeaderboardCriteria(),
		HomingStoredCriteria = customLeaderboard.HomingStoredCriteria.ToGetCustomLeaderboardCriteria(),
		HomingEatenCriteria = customLeaderboard.HomingEatenCriteria.ToGetCustomLeaderboardCriteria(),
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
	};

	private static GetCustomLeaderboardCriteria ToGetCustomLeaderboardCriteria(this CustomLeaderboardCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToString() : null,
	};

	private static GetCustomLeaderboardCriteria ToGetCustomLeaderboardCriteria(this CustomLeaderboardEnemyCriteriaEntityValue criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
		Expression = criteria.Expression == null ? null : Expression.TryParse(criteria.Expression, out Expression? expression) ? expression.ToString() : null,
	};
}
