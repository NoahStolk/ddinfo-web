namespace DevilDaggersInfo.Web.ApiSpec.DdLive.CustomLeaderboards;

public record GetCustomLeaderboardDdLive
{
	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required string? SpawnsetHtmlDescription { get; init; }

	public required GetCustomLeaderboardDaggersDdLive? Daggers { get; init; }

	public required DateTime? DateLastPlayed { get; init; }

	public required DateTime DateCreated { get; init; }

	public required int SubmitCount { get; init; }

	public required bool IsFeatured { get; init; }

	public required CustomLeaderboardCategoryDdLive Category { get; init; }

	public required List<GetCustomEntryDdLive> CustomEntries { get; init; }
}
