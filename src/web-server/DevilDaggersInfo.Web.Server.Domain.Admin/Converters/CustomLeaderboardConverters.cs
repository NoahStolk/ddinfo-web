using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class CustomLeaderboardConverters
{
	public static AdminApi.GetCustomLeaderboardForOverview ToGetCustomLeaderboardForOverview(this CustomLeaderboardEntity customLeaderboard) => new()
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

	public static AdminApi.GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard) => new()
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
	};
}
