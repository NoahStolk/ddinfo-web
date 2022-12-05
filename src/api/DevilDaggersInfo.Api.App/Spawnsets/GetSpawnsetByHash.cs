namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnsetByHash
{
	public int SpawnsetId { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public GetSpawnsetByHashCustomLeaderboard? CustomLeaderboard { get; init; }
}
