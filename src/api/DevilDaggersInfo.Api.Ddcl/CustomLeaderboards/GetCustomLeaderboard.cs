using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetCustomLeaderboard
{
	public required string SpawnsetName { get; init; }

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }

	public required List<GetCustomEntry> SortedEntries { get; init; }
}
