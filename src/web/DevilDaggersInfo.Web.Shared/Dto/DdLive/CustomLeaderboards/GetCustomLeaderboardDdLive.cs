using DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.Shared.Dto.DdLive.CustomLeaderboards;

public record GetCustomLeaderboardDdLive
{
	public int SpawnsetId { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public string SpawnsetAuthorName { get; init; } = null!;

	public string? SpawnsetHtmlDescription { get; init; }

	public GetCustomLeaderboardDaggersDdLive? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public List<GetCustomEntryDdLive> CustomEntries { get; init; } = new();
}
