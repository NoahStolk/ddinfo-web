using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardForOverview
{
	public int SpawnsetId { get; init; }

	public string SpawnsetName { get; init; } = string.Empty;

	public int PlayerCount { get; init; }

	public int SubmitCount { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public GetCustomLeaderboardWorldRecord? WorldRecord { get; init; }

	public GetCustomLeaderboardSelectedPlayerStats? SelectedPlayerStats { get; init; }
}
