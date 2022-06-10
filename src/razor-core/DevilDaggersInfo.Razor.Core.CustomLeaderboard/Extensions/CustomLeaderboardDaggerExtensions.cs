using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Razor.Core.CustomLeaderboard.Extensions;

public static class CustomLeaderboardDaggerExtensions
{
	public static string GetColorCode(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Leviathan => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Leviathan.Color.HexCode,
		CustomLeaderboardDagger.Devil => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Devil.Color.HexCode,
		CustomLeaderboardDagger.Golden => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Golden.Color.HexCode,
		CustomLeaderboardDagger.Silver => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Silver.Color.HexCode,
		CustomLeaderboardDagger.Bronze => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Bronze.Color.HexCode,
		_ => DevilDaggersInfo.Core.Wiki.DaggersV3_2.Default.Color.HexCode,
	};
}
