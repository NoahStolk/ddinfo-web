namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetCustomLeaderboardAllowedCategory
{
	public required SpawnsetGameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required int LeaderboardCount { get; init; }
}
