using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		TimeBronze = customLeaderboard.TimeBronze.ToSecondsTime(),
		TimeSilver = customLeaderboard.TimeSilver.ToSecondsTime(),
		TimeGolden = customLeaderboard.TimeGolden.ToSecondsTime(),
		TimeDevil = customLeaderboard.TimeDevil.ToSecondsTime(),
		TimeLeviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
		IsArchived = customLeaderboard.IsArchived,
		DateCreated = customLeaderboard.DateCreated,
		Category = customLeaderboard.Category,
	};
}
