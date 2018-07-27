using Newtonsoft.Json;

namespace DevilDaggersWebsite.Models.Spawnset
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SpawnsetFile
	{
		public string Path { get; set; }

		public string FileName { get { return System.IO.Path.GetFileName(Path); } }

		[JsonProperty]
		public string Name { get { return FileName.Substring(0, FileName.LastIndexOf('_')); } }
		[JsonProperty]
		public string Author { get { return FileName.Substring(FileName.LastIndexOf('_') + 1); } }

		public SpawnsetFile(string path)
		{
			Path = path;
		}
	}
}