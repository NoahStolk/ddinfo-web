namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboardOverview
{
	public required int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	public required bool IsFeatured { get; init; }

	public required DateTime? DateLastPlayed { get; init; }

	public required DateTime DateCreated { get; init; }

	public required int SubmitCount { get; init; }

	public required int PlayerCount { get; init; }

	public required string? TopPlayer { get; init; }

	public required double? WorldRecord { get; init; }

	public required CustomLeaderboardDagger? WorldRecordDagger { get; init; }
}
