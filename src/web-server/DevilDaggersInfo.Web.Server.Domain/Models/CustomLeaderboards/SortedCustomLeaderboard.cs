using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record SortedCustomLeaderboard
{
	public required int Id { get; init; }

	public required int SpawnsetId { get; init; }

	public required int SpawnsetAuthorId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required string? SpawnsetHtmlDescription { get; init; }

	public required SpawnsetGameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required CustomLeaderboardDaggers? Daggers { get; init; }

	public required DateTime? DateLastPlayed { get; init; }

	public required DateTime DateCreated { get; init; }

	public required int TotalRunsSubmitted { get; init; }

	public required List<CustomEntry> CustomEntries { get; init; }

	public required List<CustomLeaderboardCriteria> Criteria { get; init; }
}
