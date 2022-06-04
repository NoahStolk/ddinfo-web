using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static bool IsAscending(this CustomLeaderboardCategory category)
		=> category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Speedrun or CustomLeaderboardCategory.Race;
}
