using DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomLeaderboards;
using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class CustomLeaderboardConverters
	{
		public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboard customLeaderboard) => new()
		{
			Id = customLeaderboard.Id,
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze / 10000f,
			TimeSilver = customLeaderboard.TimeSilver / 10000f,
			TimeGolden = customLeaderboard.TimeGolden / 10000f,
			TimeDevil = customLeaderboard.TimeDevil / 10000f,
			TimeLeviathan = customLeaderboard.TimeLeviathan / 10000f,
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
		};
	}
}
