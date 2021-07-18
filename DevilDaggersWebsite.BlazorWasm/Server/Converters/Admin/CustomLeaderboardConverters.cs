using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class CustomLeaderboardConverters
	{
		public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboard customLeaderboard) => new()
		{
			Id = customLeaderboard.Id,
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze / 10000.0,
			TimeSilver = customLeaderboard.TimeSilver / 10000.0,
			TimeGolden = customLeaderboard.TimeGolden / 10000.0,
			TimeDevil = customLeaderboard.TimeDevil / 10000.0,
			TimeLeviathan = customLeaderboard.TimeLeviathan / 10000.0,
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
		};
	}
}
