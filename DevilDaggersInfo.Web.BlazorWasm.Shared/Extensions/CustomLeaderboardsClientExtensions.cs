namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

public static class CustomLeaderboardsClientExtensions
{
	public static CustomLeaderboardsClient GetClientFromString(this string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" => CustomLeaderboardsClient.DdstatsRust,
		_ => throw new Exception($"Unknown CustomLeaderboardsClient '{clientString}'."),
	};

	public static string ToDisplayString(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => "DDCL",
		CustomLeaderboardsClient.DdstatsRust => "ddstats-rust",
		_ => throw new Exception($"Unknown CustomLeaderboardsClient '{client}'."),
	};
}
