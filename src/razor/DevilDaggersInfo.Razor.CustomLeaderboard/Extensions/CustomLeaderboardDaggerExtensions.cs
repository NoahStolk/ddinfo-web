using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Extensions;

public static class CustomLeaderboardDaggerExtensions
{
	public static string GetColorCode(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Leviathan => DaggersV3_2.Leviathan.Color.HexCode,
		CustomLeaderboardDagger.Devil => DaggersV3_2.Devil.Color.HexCode,
		CustomLeaderboardDagger.Golden => DaggersV3_2.Golden.Color.HexCode,
		CustomLeaderboardDagger.Silver => DaggersV3_2.Silver.Color.HexCode,
		CustomLeaderboardDagger.Bronze => DaggersV3_2.Bronze.Color.HexCode,
		_ => DaggersV3_2.Default.Color.HexCode,
	};
}
