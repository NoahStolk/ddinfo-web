using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Extensions;

public static class CustomLeaderboardDaggerExtensions
{
	public static string GetColorCode(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Leviathan => Core.Wiki.DaggersV3_2.Leviathan.Color.HexCode,
		CustomLeaderboardDagger.Devil => Core.Wiki.DaggersV3_2.Devil.Color.HexCode,
		CustomLeaderboardDagger.Golden => Core.Wiki.DaggersV3_2.Golden.Color.HexCode,
		CustomLeaderboardDagger.Silver => Core.Wiki.DaggersV3_2.Silver.Color.HexCode,
		CustomLeaderboardDagger.Bronze => Core.Wiki.DaggersV3_2.Bronze.Color.HexCode,
		_ => Core.Wiki.DaggersV3_2.Default.Color.HexCode,
	};
}
