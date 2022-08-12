using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Types.Web.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static string ToDisplayString(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => nameof(CustomLeaderboardCategory.Survival),
		CustomLeaderboardCategory.TimeAttack => "Time Attack",
		CustomLeaderboardCategory.Speedrun => nameof(CustomLeaderboardCategory.Speedrun),
		CustomLeaderboardCategory.Race => nameof(CustomLeaderboardCategory.Race),
		CustomLeaderboardCategory.Pacifist => nameof(CustomLeaderboardCategory.Pacifist),
		CustomLeaderboardCategory.RaceNoShooting => "Race (no shooting)",
		_ => throw new InvalidEnumConversionException(category),
	};
}
