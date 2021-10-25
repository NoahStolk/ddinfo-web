namespace DevilDaggersWebsite.Razor.Models
{
	public class SpawnsPartialModel
	{
		public SpawnsPartialModel(string spawnsetName, bool isWikiPage)
		{
			SpawnsetName = spawnsetName;
			IsWikiPage = isWikiPage;
		}

		public string SpawnsetName { get; }

		public bool IsWikiPage { get; }
	}
}
