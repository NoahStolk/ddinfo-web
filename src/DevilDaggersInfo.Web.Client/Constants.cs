namespace DevilDaggersInfo.Web.Client;

public static class Constants
{
#pragma warning disable S1075 // URIs should not be hardcoded
	public const string DiscordUrl = "https://discord.gg/NF32j8S";
#pragma warning restore S1075 // URIs should not be hardcoded

	// TODO: Move to main and admin API projects?
	public const int PageSizeDefault = 25;
	public const int PageSizeMin = 15;
	public const int PageSizeMax = 35;

	public static DateTime TrackingCustomLeaderboardSubmitCounts { get; } = new(2020, 8, 25, 0, 0, 0, DateTimeKind.Utc);

	public static IEnumerable<int> PageSizeOptions { get; } = Enumerable.Range(3, 5).Select(i => i * 5);
}
