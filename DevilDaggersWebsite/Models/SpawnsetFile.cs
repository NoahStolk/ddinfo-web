namespace DevilDaggersWebsite.Models
{
	/// <summary>
	/// This class will be encoded to JSON.
	/// </summary>
	public class SpawnsetFile
	{
		public string DownloadLink { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }

		public SpawnsetFile(string downloadLink, string name, string author, string description)
		{
			DownloadLink = downloadLink;
			Name = name;
			Author = author;
			Description = description;
		}
	}
}