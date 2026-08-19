namespace DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

public sealed record GetSpawnsetByHashCustomLeaderboard
{
	public required int CustomLeaderboardId { get; init; }

	public required List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; }
}
