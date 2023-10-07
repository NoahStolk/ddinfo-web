namespace DevilDaggersInfo.Web.ApiSpec.App.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public required string SpawnsetName { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required SpawnsetGameMode SpawnsetGameMode { get; init; }

	public required List<GetCustomEntry> SortedEntries { get; init; }

	public required List<GetCustomLeaderboardCriteria> Criteria { get; init; }
}
