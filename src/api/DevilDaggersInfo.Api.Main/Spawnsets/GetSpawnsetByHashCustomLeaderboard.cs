namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetByHashCustomLeaderboard
{
	public int CustomLeaderboardId { get; init; }

	public List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; } = new();
}
