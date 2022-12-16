namespace DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;

public class SpawnsetHashCacheData
{
	public required string Name { get; init; }

	public required byte[] Hash { get; init; }
}
