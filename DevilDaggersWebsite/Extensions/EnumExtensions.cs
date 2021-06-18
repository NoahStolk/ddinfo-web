using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Extensions
{
	public static class EnumExtensions
	{
		public static bool IsAscending(this CustomLeaderboardCategory category)
			=> category == CustomLeaderboardCategory.TimeAttack || category == CustomLeaderboardCategory.Speedrun;
	}
}
