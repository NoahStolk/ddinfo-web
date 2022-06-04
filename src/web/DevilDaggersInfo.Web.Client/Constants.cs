namespace DevilDaggersInfo.Web.Client;

public static class Constants
{
#pragma warning disable S1075 // URIs should not be hardcoded
	public const string DiscordUrl = "https://discord.gg/NF32j8S";
#pragma warning restore S1075 // URIs should not be hardcoded

	public static DateTime TrackingCustomLeaderboardSubmitCounts { get; } = new(2020, 8, 25);

	public static DateTime TrackingToolDownloadCounts { get; } = new(2020, 9, 20);
}
