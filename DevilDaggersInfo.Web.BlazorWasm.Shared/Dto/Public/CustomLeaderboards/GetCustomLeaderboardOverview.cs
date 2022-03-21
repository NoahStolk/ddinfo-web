namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetCustomLeaderboardOverview
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public string SpawnsetAuthorName { get; init; } = null!;

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public bool IsFeatured { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public int PlayerCount { get; init; }

	public string? TopPlayer { get; init; }

	public double? WorldRecord { get; init; }

	public CustomLeaderboardDagger WorldRecordDagger { get; init; }
}
