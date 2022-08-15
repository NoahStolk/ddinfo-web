using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
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
		Skull1KillsCriteria = customLeaderboard.Skull1KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Skull2KillsCriteria = customLeaderboard.Skull2KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Skull3KillsCriteria = customLeaderboard.Skull3KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Skull4KillsCriteria = customLeaderboard.Skull4KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		SpiderlingKillsCriteria = customLeaderboard.SpiderlingKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		SpiderEggKillsCriteria = customLeaderboard.SpiderEggKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Squid1KillsCriteria = customLeaderboard.Squid1KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Squid2KillsCriteria = customLeaderboard.Squid2KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Squid3KillsCriteria = customLeaderboard.Squid3KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		CentipedeKillsCriteria = customLeaderboard.CentipedeKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		GigapedeKillsCriteria = customLeaderboard.GigapedeKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		GhostpedeKillsCriteria = customLeaderboard.GhostpedeKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Spider1KillsCriteria = customLeaderboard.Spider1KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		Spider2KillsCriteria = customLeaderboard.Spider2KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		LeviathanKillsCriteria = customLeaderboard.LeviathanKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		OrbKillsCriteria = customLeaderboard.OrbKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
		ThornKillsCriteria = customLeaderboard.ThornKillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
	};

	private static GetCustomLeaderboardCriteria ToGetCustomLeaderboardCriteria(this CustomLeaderboardCriteria criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
	};

	private static GetCustomLeaderboardEnemyCriteria ToGetCustomLeaderboardEnemyCriteria(this CustomLeaderboardEnemyCriteria criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
	};

	public static CustomLeaderboardCriteria ToEntity(this AddCustomLeaderboardCriteria criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
	};

	public static CustomLeaderboardEnemyCriteria ToEntity(this AddCustomLeaderboardEnemyCriteria criteria) => new()
	{
		Operator = criteria.Operator,
		Value = criteria.Value,
	};
}
