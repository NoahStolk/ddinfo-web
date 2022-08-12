using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class EnumDisplayStringExtensions
{
	public static string ToDisplayString(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => "DDCL",
		CustomLeaderboardsClient.DdstatsRust => "ddstats-rust",
		_ => throw new InvalidEnumConversionException(client),
	};

	public static string ToDisplayString(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => nameof(GameMode.Survival),
		GameMode.TimeAttack => "Time Attack",
		GameMode.Race => nameof(GameMode.Race),
		_ => throw new InvalidEnumConversionException(gameMode),
	};
}
