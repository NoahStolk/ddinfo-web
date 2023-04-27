using DevilDaggersInfo.Api.Main.CustomLeaderboards;
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

	public static string ToDisplayString(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => "Survival",
		CustomLeaderboardCategory.TimeAttack => "Time Attack",
		CustomLeaderboardCategory.Speedrun => "Speedrun",
		CustomLeaderboardCategory.Race => "Race",
		_ => throw new UnreachableException(),
	};
}
