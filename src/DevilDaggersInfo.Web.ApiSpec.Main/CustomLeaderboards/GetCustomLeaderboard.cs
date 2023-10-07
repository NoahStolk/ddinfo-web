using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required string? SpawnsetHtmlDescription { get; init; }

	public required GameMode SpawnsetGameMode { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	public required DateTime? DateLastPlayed { get; init; }

	public required DateTime DateCreated { get; init; }

	public required int SubmitCount { get; init; }

	public required bool IsFeatured { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required List<GetCustomEntry> CustomEntries { get; set; }

	public required List<GetCustomLeaderboardCriteria> Criteria { get; init; }
}
