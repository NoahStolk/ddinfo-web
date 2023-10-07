namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardAllowedCategory
{
	public required SpawnsetGameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }
}
