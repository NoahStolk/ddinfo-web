using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public string? SpawnsetHtmlDescription { get; init; }

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public required List<GetCustomEntry> CustomEntries { get; set; }

	public required List<GetCustomLeaderboardCriteria> Criteria { get; init; }
}
