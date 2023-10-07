namespace DevilDaggersInfo.Web.ApiSpec.App.CustomLeaderboards;

public record GetCustomLeaderboardAllowedCategory
{
	public required SpawnsetGameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }
}
