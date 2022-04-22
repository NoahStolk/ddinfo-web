using DevilDaggersInfo.Core.Spawnset.Enums;

namespace DevilDaggersInfo.Web.Shared.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static bool IsAscending(this CustomLeaderboardCategory category)
		=> category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Speedrun or CustomLeaderboardCategory.Race;

	public static GameMode GetRequiredGameModeForCategory(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival or CustomLeaderboardCategory.Speedrun or CustomLeaderboardCategory.Pacifist => GameMode.Survival,
		CustomLeaderboardCategory.TimeAttack => GameMode.TimeAttack,
		CustomLeaderboardCategory.Race => GameMode.Race,
		_ => throw new NotSupportedException($"{nameof(CustomLeaderboardCategory)} {category} is not supported."),
	};
}
