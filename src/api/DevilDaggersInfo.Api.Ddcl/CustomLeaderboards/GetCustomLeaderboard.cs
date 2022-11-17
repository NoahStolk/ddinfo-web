using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public required string SpawnsetName { get; init; }

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }

	public required List<GetCustomEntry> SortedEntries { get; init; }
}
