using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardForOverview
{
	public int SpawnsetId { get; init; }

	public string SpawnsetName { get; init; } = string.Empty;

	public int PlayerCount { get; init; }

	public int SubmitCount { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public GetCustomLeaderboardWorldRecord? WorldRecord { get; init; }

	public GetCustomLeaderboardSelectedPlayerStats? SelectedPlayerStats { get; init; }
}
