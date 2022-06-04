namespace DevilDaggersInfo.Web.Client;

public static class Constants
{
#pragma warning disable S1075 // URIs should not be hardcoded
	public const string DiscordUrl = "https://discord.gg/NF32j8S";
#pragma warning restore S1075 // URIs should not be hardcoded

	public static DateTime TrackingCustomLeaderboardSubmitCounts { get; } = new(2020, 8, 25);

	public static DateTime TrackingToolDownloadCounts { get; } = new(2020, 9, 20);

	// NOTE: Duplicate code (Api.Admin).
	public const int BinaryMaxFileSize = 256 * 1024 * 1024;
	public const int BinaryMaxFiles = 80;

	// NOTE: Duplicate code (Api.Admin).
	public const int ScreenshotMaxFileSize = 1024 * 1024;
	public const int ScreenshotMaxFiles = 16;

	public const int PageSizeDefault = 25;
	public const int PageSizeMin = 15;
	public const int PageSizeMax = 35;

	public static IEnumerable<int> PageSizeOptions { get; } = Enumerable.Range(3, 5).Select(i => i * 5);
}
