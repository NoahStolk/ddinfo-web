namespace DevilDaggersInfo.Web.ApiSpec.Tools.Spawnsets;

public record GetSpawnsetByHashCustomLeaderboard
{
	public required int CustomLeaderboardId { get; init; }

	public required List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; } = [];
}
