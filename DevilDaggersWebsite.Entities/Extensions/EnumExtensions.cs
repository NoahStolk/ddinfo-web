using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Extensions
{
	public static class EnumExtensions
	{
		public static bool IsAscending(this CustomLeaderboardCategory category)
			=> category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Speedrun;
	}
}
