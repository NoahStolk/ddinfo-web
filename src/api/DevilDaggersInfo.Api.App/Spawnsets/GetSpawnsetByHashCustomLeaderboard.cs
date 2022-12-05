namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnsetByHashCustomLeaderboard
{
	public int CustomLeaderboardId { get; init; }

	public List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; } = new();
}
