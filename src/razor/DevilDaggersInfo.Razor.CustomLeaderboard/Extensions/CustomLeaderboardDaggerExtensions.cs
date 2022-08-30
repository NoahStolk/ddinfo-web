using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Extensions;

public static class CustomLeaderboardDaggerExtensions
{
	public static string GetColorCode(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Leviathan => Daggers.Leviathan.Color.HexCode,
		CustomLeaderboardDagger.Devil => Daggers.Devil.Color.HexCode,
		CustomLeaderboardDagger.Golden => Daggers.Golden.Color.HexCode,
		CustomLeaderboardDagger.Silver => Daggers.Silver.Color.HexCode,
		CustomLeaderboardDagger.Bronze => Daggers.Bronze.Color.HexCode,
		_ => Daggers.Default.Color.HexCode,
	};
}
