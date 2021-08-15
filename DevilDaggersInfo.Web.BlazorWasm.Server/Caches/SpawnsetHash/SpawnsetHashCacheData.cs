namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHash;

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
