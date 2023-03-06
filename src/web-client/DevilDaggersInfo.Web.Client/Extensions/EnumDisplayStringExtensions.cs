using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Web;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class EnumDisplayStringExtensions
{
	public static string ToDisplayString(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => "DDCL",
		CustomLeaderboardsClient.DdstatsRust => "ddstats-rust",
		CustomLeaderboardsClient.DdinfoTools => "DDINFO",
		_ => throw new UnreachableException(),
	};

	public static string ToDisplayString(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => nameof(GameMode.Survival),
		GameMode.TimeAttack => "Time Attack",
		GameMode.Race => nameof(GameMode.Race),
		_ => throw new UnreachableException(),
	};
}
