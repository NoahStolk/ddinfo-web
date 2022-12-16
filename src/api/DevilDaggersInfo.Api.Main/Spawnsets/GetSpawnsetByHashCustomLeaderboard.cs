namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetByHashCustomLeaderboard
{
	public required int CustomLeaderboardId { get; init; }

	public required List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; }
}
