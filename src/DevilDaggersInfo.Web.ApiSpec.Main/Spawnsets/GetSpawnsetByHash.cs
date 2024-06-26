namespace DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

public record GetSpawnsetByHash
{
	public required int SpawnsetId { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public required GetSpawnsetByHashCustomLeaderboard? CustomLeaderboard { get; init; }
}
