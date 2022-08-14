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
			Bronze = customLeaderboard.TimeBronze.ToSecondsTime(),
			Silver = customLeaderboard.TimeSilver.ToSecondsTime(),
			Golden = customLeaderboard.TimeGolden.ToSecondsTime(),
			Devil = customLeaderboard.TimeDevil.ToSecondsTime(),
			Leviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
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
			Bronze = customLeaderboard.TimeBronze.ToSecondsTime(),
			Silver = customLeaderboard.TimeSilver.ToSecondsTime(),
			Golden = customLeaderboard.TimeGolden.ToSecondsTime(),
			Devil = customLeaderboard.TimeDevil.ToSecondsTime(),
			Leviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
		},
		IsFeatured = customLeaderboard.IsFeatured,
		Category = customLeaderboard.Category,
		GemsCollectedCriteria = customLeaderboard.GemsCollectedCriteria.ToGetCustomLeaderboardCriteria(),
		EnemiesKilledCriteria = customLeaderboard.EnemiesKilledCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersFiredCriteria = customLeaderboard.DaggersFiredCriteria.ToGetCustomLeaderboardCriteria(),
		DaggersHitCriteria = customLeaderboard.DaggersHitCriteria.ToGetCustomLeaderboardCriteria(),
		Skull1KillsCriteria = customLeaderboard.Skull1KillsCriteria.ToGetCustomLeaderboardEnemyCriteria(),
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
