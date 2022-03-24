namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public record GetCustomLeaderboardDdLive
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public int SpawnsetAuthorId { get; init; }

	public string SpawnsetAuthorName { get; init; } = null!;

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public int PlayerCount { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public int? TopPlayerId { get; init; }

	public string? TopPlayerName { get; init; }

	public double? WorldRecord { get; init; }

	public CustomLeaderboardDagger WorldRecordDagger { get; init; }
}
