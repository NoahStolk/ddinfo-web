using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayerCustomLeaderboardStatistics
{
	public required CustomLeaderboardCategory CustomLeaderboardCategory { get; init; }

	public required int LeviathanDaggerCount { get; init; }

	public required int DevilDaggerCount { get; init; }

	public required int GoldenDaggerCount { get; init; }

	public required int SilverDaggerCount { get; init; }

	public required int BronzeDaggerCount { get; init; }

	public required int DefaultDaggerCount { get; init; }

	public required int LeaderboardsPlayedCount { get; init; }

	public required int TotalCount { get; init; }
}
