using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Types.Web;
using MainApi = DevilDaggersInfo.Api.Main;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class EnumDisplayStringExtensions
{
	public static string ToDisplayString(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => "DDCL",
		CustomLeaderboardsClient.DdstatsRust => "ddstats-rust",
		_ => throw new InvalidEnumConversionException(client),
	};

	public static string ToDisplayString(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => nameof(CustomLeaderboardCategory.Survival),
		CustomLeaderboardCategory.TimeAttack => "Time Attack",
		CustomLeaderboardCategory.Speedrun => nameof(CustomLeaderboardCategory.Speedrun),
		CustomLeaderboardCategory.Race => nameof(CustomLeaderboardCategory.Race),
		CustomLeaderboardCategory.Pacifist => nameof(CustomLeaderboardCategory.Pacifist),
		CustomLeaderboardCategory.RaceNoShooting => "Race (no shooting)",
		_ => throw new InvalidEnumConversionException(category),
	};

	public static string ToDisplayString(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => nameof(GameMode.Survival),
		GameMode.TimeAttack => "Time Attack",
		GameMode.Race => nameof(GameMode.Race),
		_ => throw new InvalidEnumConversionException(gameMode),
	};

	public static string ToDisplayString(this MainApi.Spawnsets.GameMode gameMode) => gameMode switch
	{
		MainApi.Spawnsets.GameMode.Survival => nameof(MainApi.Spawnsets.GameMode.Survival),
		MainApi.Spawnsets.GameMode.TimeAttack => "Time Attack",
		MainApi.Spawnsets.GameMode.Race => nameof(MainApi.Spawnsets.GameMode.Race),
		_ => throw new InvalidEnumConversionException(gameMode),
	};
}
