using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardsClientExtensions
{
	public static CustomLeaderboardsClient GetClientFromString(this string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" => CustomLeaderboardsClient.DdstatsRust,
		_ => throw new CustomEntryValidationException($"Unknown CustomLeaderboardsClient '{clientString}'."),
	};
}
