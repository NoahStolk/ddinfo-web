using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboardOverview
{
	public int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required string SpawnsetAuthorName { get; init; }

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public bool IsFeatured { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public int PlayerCount { get; init; }

	public string? TopPlayer { get; init; }

	public double? WorldRecord { get; init; }

	public CustomLeaderboardDagger? WorldRecordDagger { get; init; }
}
