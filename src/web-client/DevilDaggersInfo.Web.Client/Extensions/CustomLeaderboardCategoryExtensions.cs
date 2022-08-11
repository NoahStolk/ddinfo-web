using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static string GetDescription(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => "Survive as long as you can.",
		CustomLeaderboardCategory.TimeAttack => "Kill all enemies as quickly as possible.",
		CustomLeaderboardCategory.Speedrun => "Jump into the void as quickly as possible. Note that this category has been superseded by the Race category; new Speedrun leaderboards will not be added.",
		CustomLeaderboardCategory.Race => "Reach the dagger as quickly as possible.",
		CustomLeaderboardCategory.Pacifist => "Survive as long as you can, but without killing any enemies.",
		CustomLeaderboardCategory.RaceNoShooting => "Reach the dagger as quickly as possible, but without shooting (disallowing dagger jumps).",
		_ => throw new InvalidEnumConversionException(category),
	};
}
