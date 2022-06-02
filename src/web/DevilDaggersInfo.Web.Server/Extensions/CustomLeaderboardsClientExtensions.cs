namespace DevilDaggersInfo.Web.Server.Extensions;

public static class CustomLeaderboardsClientExtensions
{
	public static CustomLeaderboardsClient GetClientFromString(this string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" => CustomLeaderboardsClient.DdstatsRust,
		_ => throw new Exception($"Unknown CustomLeaderboardsClient '{clientString}'."),
	};
}
