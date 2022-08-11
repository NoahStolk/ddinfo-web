using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.Utils;

// TODO: Source-generate this for all enums.
public static class EnumConvert
{
	public static string GetString(CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.TimeAttack => nameof(CustomLeaderboardCategory.TimeAttack),
		CustomLeaderboardCategory.Speedrun => nameof(CustomLeaderboardCategory.Speedrun),
		CustomLeaderboardCategory.Race => nameof(CustomLeaderboardCategory.Race),
		CustomLeaderboardCategory.Pacifist => nameof(CustomLeaderboardCategory.Pacifist),
		CustomLeaderboardCategory.RaceNoShooting => nameof(CustomLeaderboardCategory.RaceNoShooting),
		_ => nameof(CustomLeaderboardCategory.Survival),
	};

	public static CustomLeaderboardCategory GetCustomLeaderboardCategory(string str) => str switch
	{
		nameof(CustomLeaderboardCategory.TimeAttack) => CustomLeaderboardCategory.TimeAttack,
		nameof(CustomLeaderboardCategory.Speedrun) => CustomLeaderboardCategory.Speedrun,
		nameof(CustomLeaderboardCategory.Race) => CustomLeaderboardCategory.Race,
		nameof(CustomLeaderboardCategory.Pacifist) => CustomLeaderboardCategory.Pacifist,
		nameof(CustomLeaderboardCategory.RaceNoShooting) => CustomLeaderboardCategory.RaceNoShooting,
		_ => CustomLeaderboardCategory.Survival,
	};
}
