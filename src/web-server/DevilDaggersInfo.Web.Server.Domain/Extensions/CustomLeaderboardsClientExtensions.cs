using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardsClientExtensions
{
	public static CustomLeaderboardsClient ClientFromString(this string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" => CustomLeaderboardsClient.DdstatsRust,
		"ddinfo-tools" => CustomLeaderboardsClient.DdinfoTools,
		_ => throw new CustomEntryValidationException($"Unknown CustomLeaderboardsClient '{clientString}'."),
	};
}
