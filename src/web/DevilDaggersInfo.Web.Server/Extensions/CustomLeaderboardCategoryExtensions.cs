using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Entities.Enums;

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
		_ => throw new InvalidEnumConversionException(category),
	};
}
