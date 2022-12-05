using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public required string SpawnsetName { get; init; }

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }

	public required List<GetCustomEntry> SortedEntries { get; init; }
}
