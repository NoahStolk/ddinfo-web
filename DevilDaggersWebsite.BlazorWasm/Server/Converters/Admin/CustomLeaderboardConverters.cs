using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class CustomLeaderboardConverters
	{
		public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboard customLeaderboard) => new()
		{
			Id = customLeaderboard.Id,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze * 0.0001,
			TimeSilver = customLeaderboard.TimeSilver * 0.0001,
			TimeGolden = customLeaderboard.TimeGolden * 0.0001,
			TimeDevil = customLeaderboard.TimeDevil * 0.0001,
			TimeLeviathan = customLeaderboard.TimeLeviathan * 0.0001,
			IsArchived = customLeaderboard.IsArchived,
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
		};
	}
}
