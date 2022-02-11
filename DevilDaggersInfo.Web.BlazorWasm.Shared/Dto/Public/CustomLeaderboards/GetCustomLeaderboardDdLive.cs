namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetCustomLeaderboardDdLive
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public int SpawnsetAuthorId { get; init; }

	public string SpawnsetAuthorName { get; init; } = null!;

	public double TimeBronze { get; init; }

	public double TimeSilver { get; init; }

	public double TimeGolden { get; init; }

	public double TimeDevil { get; init; }

	public double TimeLeviathan { get; init; }

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
