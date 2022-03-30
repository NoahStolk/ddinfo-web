using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

// TODO: Source-generate this for all enums.
public static class EnumConvert
{
	public static string GetString(CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.TimeAttack => nameof(CustomLeaderboardCategory.TimeAttack),
		CustomLeaderboardCategory.Speedrun => nameof(CustomLeaderboardCategory.Speedrun),
		CustomLeaderboardCategory.Race => nameof(CustomLeaderboardCategory.Race),
		CustomLeaderboardCategory.Pacifist => nameof(CustomLeaderboardCategory.Pacifist),
		_ => "Survival",
	};

	public static CustomLeaderboardCategory GetCustomLeaderboardCategory(string str) => str switch
	{
		nameof(CustomLeaderboardCategory.TimeAttack) => CustomLeaderboardCategory.TimeAttack,
		nameof(CustomLeaderboardCategory.Speedrun) => CustomLeaderboardCategory.Speedrun,
		nameof(CustomLeaderboardCategory.Race) => CustomLeaderboardCategory.Race,
		nameof(CustomLeaderboardCategory.Pacifist) => CustomLeaderboardCategory.Pacifist,
		_ => CustomLeaderboardCategory.Survival,
	};
}
