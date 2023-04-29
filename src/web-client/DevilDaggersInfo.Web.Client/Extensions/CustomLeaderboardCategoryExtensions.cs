using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static string ToDisplayString(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => "Survival",
		CustomLeaderboardCategory.TimeAttack => "Time Attack",
		CustomLeaderboardCategory.Speedrun => "Speedrun",
		CustomLeaderboardCategory.Race => "Race",
		_ => throw new UnreachableException(),
	};
}
