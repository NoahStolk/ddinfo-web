namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnsetByHashCustomLeaderboard
{
	public required int CustomLeaderboardId { get; init; }

	public required List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; } = new();
}
