using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.DdLive.CustomLeaderboards;

public record GetCustomLeaderboardOverviewDdLive
{
	public required int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required int SpawnsetAuthorId { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public required GetCustomLeaderboardDaggersDdLive? Daggers { get; init; }

	public required DateTime? DateLastPlayed { get; init; }

	public required DateTime DateCreated { get; init; }

	public required int SubmitCount { get; init; }

	public required int PlayerCount { get; init; }

	public required CustomLeaderboardCategory Category { get; init; }

	public required int? TopPlayerId { get; init; }

	public required string? TopPlayerName { get; init; }

	public required double? WorldRecord { get; init; }

	public required CustomLeaderboardDagger? WorldRecordDagger { get; init; }
}
