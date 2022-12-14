using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardOverview
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public int SpawnsetAuthorId { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int TotalRunsSubmitted { get; init; }

	public int PlayerCount { get; init; }

	public CustomLeaderboardOverviewWorldRecord? WorldRecord { get; init; }

	public CustomLeaderboardOverviewSelectedPlayerStats? SelectedPlayerStats { get; init; }
}
