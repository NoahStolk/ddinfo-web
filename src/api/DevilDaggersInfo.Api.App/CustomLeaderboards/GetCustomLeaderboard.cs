namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public required string SpawnsetName { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	[Obsolete("Use RankSorting instead.")]
	public required CustomLeaderboardCategory Category { get; init; }

	[Obsolete("Use RankSorting instead.")]
	public required bool IsAscending { get; init; }

	public required List<GetCustomEntry> SortedEntries { get; init; }

	public required List<GetCustomLeaderboardCriteria> Criteria { get; init; }
}
