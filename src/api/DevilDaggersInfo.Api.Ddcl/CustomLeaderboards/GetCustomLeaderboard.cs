namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboard
{
	public string SpawnsetName { get; init; } = null!;

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }

	public List<GetCustomEntry> SortedEntries { get; init; } = new();
}
