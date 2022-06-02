using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Common.Exceptions;

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
		_ => throw new InvalidEnumConversionException(category),
	};
}
