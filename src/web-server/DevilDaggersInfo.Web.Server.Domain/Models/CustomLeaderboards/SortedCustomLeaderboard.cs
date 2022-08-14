using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class SortedCustomLeaderboard
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public int SpawnsetAuthorId { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public string SpawnsetAuthorName { get; init; } = null!;

	public string? SpawnsetHtmlDescription { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int TotalRunsSubmitted { get; init; }

	public List<CustomEntry> CustomEntries { get; init; } = new(); // TODO: C# 11 required.

	public List<string> Criteria { get; init; } = new();
}
