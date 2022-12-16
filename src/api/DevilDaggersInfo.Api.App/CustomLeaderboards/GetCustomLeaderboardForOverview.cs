using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardForOverview
{
	public required int Id { get; init; }

	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required int PlayerCount { get; init; }

	public required int SubmitCount { get; init; }

	public required CustomLeaderboardCategory Category { get; init; }

	public required GetCustomLeaderboardDaggers? Daggers { get; init; }

	public required GetCustomLeaderboardWorldRecord? WorldRecord { get; init; }

	public required GetCustomLeaderboardSelectedPlayerStats? SelectedPlayerStats { get; init; }
}
