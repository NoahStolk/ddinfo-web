using DevilDaggersInfo.Web.Shared.Dto.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Ddcl;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = !customLeaderboard.IsFeatured ? null : new GetCustomLeaderboardDaggersDdcl
		{
			Bronze = customLeaderboard.TimeBronze,
			Silver = customLeaderboard.TimeSilver,
			Golden = customLeaderboard.TimeGolden,
			Devil = customLeaderboard.TimeDevil,
			Leviathan = customLeaderboard.TimeLeviathan,
		},
		Category = customLeaderboard.Category,
		IsAscending = customLeaderboard.Category.IsAscending(),
	};
}
