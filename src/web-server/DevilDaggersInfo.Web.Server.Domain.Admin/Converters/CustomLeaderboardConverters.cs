using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
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
		Category = customLeaderboard.Category.ToAdminApi(),
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
		Category = customLeaderboard.Category.ToAdminApi(),
	};

	public static AdminApi.CustomLeaderboardCategory ToAdminApi(this CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.Survival => AdminApi.CustomLeaderboardCategory.Survival,
		CustomLeaderboardCategory.TimeAttack => AdminApi.CustomLeaderboardCategory.TimeAttack,
		CustomLeaderboardCategory.Speedrun => AdminApi.CustomLeaderboardCategory.Speedrun,
		CustomLeaderboardCategory.Race => AdminApi.CustomLeaderboardCategory.Race,
		CustomLeaderboardCategory.Pacifist => AdminApi.CustomLeaderboardCategory.Pacifist,
		CustomLeaderboardCategory.RaceNoShooting => AdminApi.CustomLeaderboardCategory.RaceNoShooting,
		_ => throw new InvalidEnumConversionException(customLeaderboardCategory),
	};
}
