using System;

namespace DevilDaggersWebsite.Code.Database
{
	public class SpawnsetFile
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int PlayerId { get; set; }
		public int? MaxDisplayWaves { get; set; }
		public string? HtmlDescription { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}