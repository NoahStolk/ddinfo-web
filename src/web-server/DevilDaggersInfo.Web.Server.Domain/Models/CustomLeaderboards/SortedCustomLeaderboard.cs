using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class SortedCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public int SpawnsetAuthorId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public string? SpawnsetHtmlDescription { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int TotalRunsSubmitted { get; init; }

	public required List<CustomEntry> CustomEntries { get; init; }

	public required List<CustomLeaderboardCriteria> Criteria { get; init; }
}
