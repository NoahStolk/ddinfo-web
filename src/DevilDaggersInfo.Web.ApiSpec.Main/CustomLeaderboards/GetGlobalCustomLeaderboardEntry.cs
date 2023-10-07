namespace DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

public record GetGlobalCustomLeaderboardEntry
{
	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required int Points { get; init; }

	public required int LeviathanDaggerCount { get; init; }

	public required int DevilDaggerCount { get; init; }

	public required int GoldenDaggerCount { get; init; }

	public required int SilverDaggerCount { get; init; }

	public required int BronzeDaggerCount { get; init; }

	public required int DefaultDaggerCount { get; init; }

	public required int LeaderboardsPlayedCount { get; init; }
}
