namespace DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;

public class SpawnsetHashCacheData
{
	public SpawnsetHashCacheData(string name, byte[] hash)
	{
		Name = name;
		Hash = hash;
	}

	public string Name { get; init; }
	public byte[] Hash { get; init; }
}
