using DevilDaggersWebsite.Utils;

namespace DevilDaggersWebsite.Models.Spawnset
{
	public class SpawnsetFile
    {
		public string Path { get; set; }

		public string Name { get { return Path.Substring(0, Path.LastIndexOf('_')); } }
		public string Author { get { return Path.Substring(Path.LastIndexOf('_') + 1); } }

		public Spawnset Spawnset { get { return SpawnsetParser.ParseFile(Path); } }

		public SpawnsetFile(string path)
		{
			Path = path;
		}
    }
}