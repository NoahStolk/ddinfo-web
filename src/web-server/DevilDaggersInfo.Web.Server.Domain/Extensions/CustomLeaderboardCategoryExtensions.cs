using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

[Obsolete("Category will be replaced by sorting.")]
public static class CustomLeaderboardCategoryExtensions
{
	public static bool IsAscending(this CustomLeaderboardCategory category)
		=> category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Speedrun or CustomLeaderboardCategory.Race;

	public static GameMode RequiredGameModeForCategory(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival or CustomLeaderboardCategory.Speedrun => GameMode.Survival,
		CustomLeaderboardCategory.TimeAttack => GameMode.TimeAttack,
		CustomLeaderboardCategory.Race => GameMode.Race,
		_ => throw new UnreachableException(),
	};

	public static bool IsTimeAttackOrRace(this CustomLeaderboardCategory category)
		=> category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Race;
}
