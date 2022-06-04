using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static GameMode GetRequiredGameModeForCategory(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival or CustomLeaderboardCategory.Speedrun or CustomLeaderboardCategory.Pacifist => GameMode.Survival,
		CustomLeaderboardCategory.TimeAttack => GameMode.TimeAttack,
		CustomLeaderboardCategory.Race => GameMode.Race,
		_ => throw new InvalidEnumConversionException(category),
	};
}
