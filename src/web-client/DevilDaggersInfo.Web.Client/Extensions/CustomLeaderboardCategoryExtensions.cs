using DevilDaggersInfo.Types.Web;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static string GetDescription(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => "Survive as long as you can.",
		CustomLeaderboardCategory.TimeAttack => "Kill all enemies as quickly as possible.",
		CustomLeaderboardCategory.Speedrun => "Die as quickly as possible.",
		CustomLeaderboardCategory.Race => "Reach the dagger as quickly as possible.",
		_ => throw new UnreachableException(),
	};
}
