namespace DevilDaggersWebsite.Models.Spawnset
{
	public class SpawnsetFile
    {
		public string Name { get; set; }
		public string Author { get; set; }
		public string Path { get; set; }

		public SpawnsetFile(string name, string author, string path)
		{
			Name = name;
			Author = author;
			Path = path;
		}
    }
}