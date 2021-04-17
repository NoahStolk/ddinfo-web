namespace DevilDaggersWebsite.Caches.SpawnsetHash
{
	public class SpawnsetHashData
	{
		public SpawnsetHashData(string name, byte[] hash)
		{
			Name = name;
			Hash = hash;
		}

		public string Name { get; init; }
		public byte[] Hash { get; init; }
	}
}
