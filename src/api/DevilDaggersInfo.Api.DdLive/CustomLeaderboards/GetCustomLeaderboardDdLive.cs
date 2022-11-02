using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.DdLive.CustomLeaderboards;

public record GetCustomLeaderboardDdLive
{
	public int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public string? SpawnsetHtmlDescription { get; init; }

	public GetCustomLeaderboardDaggersDdLive? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public required List<GetCustomEntryDdLive> CustomEntries { get; init; }
}
