using DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Utils;

public static class EnumConvert
{
	public static string GetString(CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.TimeAttack => nameof(CustomLeaderboardCategory.TimeAttack),
		CustomLeaderboardCategory.Speedrun => nameof(CustomLeaderboardCategory.Speedrun),
		CustomLeaderboardCategory.Race => nameof(CustomLeaderboardCategory.Race),
		_ => nameof(CustomLeaderboardCategory.Survival),
	};

	public static CustomLeaderboardCategory GetCustomLeaderboardCategory(string str) => str switch
	{
		nameof(CustomLeaderboardCategory.TimeAttack) => CustomLeaderboardCategory.TimeAttack,
		nameof(CustomLeaderboardCategory.Speedrun) => CustomLeaderboardCategory.Speedrun,
		nameof(CustomLeaderboardCategory.Race) => CustomLeaderboardCategory.Race,
		_ => CustomLeaderboardCategory.Survival,
	};
}
