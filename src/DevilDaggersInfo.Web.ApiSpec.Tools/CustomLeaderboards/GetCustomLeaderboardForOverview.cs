namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetCustomLeaderboardForOverview
{
	public required int Id { get; init; }

	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required int SpawnsetAuthorId { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required SpawnsetGameMode SpawnsetGameMode { get; init; }

	public required int PlayerCount { get; init; }

	public required int SubmitCount { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	public required GetCustomLeaderboardWorldRecord? WorldRecord { get; init; }

	public required GetCustomLeaderboardSelectedPlayerStats? SelectedPlayerStats { get; init; }

	public required List<GetCustomLeaderboardCriteria> Criteria { get; init; }

	public required DateTime DateCreated { get; init; }

	public required DateTime? DateLastPlayed { get; init; }
}
