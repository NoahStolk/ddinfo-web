using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public record GetPlayerCustomLeaderboardStatistics
{
	public required GameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required int LeviathanDaggerCount { get; init; }

	public required int DevilDaggerCount { get; init; }

	public required int GoldenDaggerCount { get; init; }

	public required int SilverDaggerCount { get; init; }

	public required int BronzeDaggerCount { get; init; }

	public required int DefaultDaggerCount { get; init; }

	public required int LeaderboardsPlayedCount { get; init; }

	public required int TotalCount { get; init; }
}
