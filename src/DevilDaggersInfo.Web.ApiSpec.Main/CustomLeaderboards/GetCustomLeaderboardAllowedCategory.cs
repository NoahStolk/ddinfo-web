using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

public record GetCustomLeaderboardAllowedCategory
{
	public required GameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required int LeaderboardCount { get; init; }
}
