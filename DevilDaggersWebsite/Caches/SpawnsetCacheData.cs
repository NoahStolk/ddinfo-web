namespace DevilDaggersWebsite.Caches
{
	public class SpawnsetCacheData
	{
		public SpawnsetCacheData(string name, byte[] hash)
		{
			Name = name;
			Hash = hash;
		}

		public string Name { get; init; }
		public byte[] Hash { get; init; }
	}
}
