using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

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
