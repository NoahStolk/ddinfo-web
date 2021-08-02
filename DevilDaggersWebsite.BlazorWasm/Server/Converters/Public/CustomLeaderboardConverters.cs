using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Public
{
	public static class CustomLeaderboardConverters
	{
		public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboard customLeaderboard) => new()
		{
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze,
			TimeSilver = customLeaderboard.TimeSilver,
			TimeGolden = customLeaderboard.TimeGolden,
			TimeDevil = customLeaderboard.TimeDevil,
			TimeLeviathan = customLeaderboard.TimeLeviathan,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
			IsAscending = customLeaderboard.Category.IsAscending(),
		};

		public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboard customLeaderboard) => new()
		{
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze.ToSecondsTime(),
			TimeSilver = customLeaderboard.TimeSilver.ToSecondsTime(),
			TimeGolden = customLeaderboard.TimeGolden.ToSecondsTime(),
			TimeDevil = customLeaderboard.TimeDevil.ToSecondsTime(),
			TimeLeviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
		};
	}
}
