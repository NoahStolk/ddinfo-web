using DevilDaggersCore.Spawnsets;
using System;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class SpawnsetFile
	{
		public int? MaxDisplayWaves { get; set; }

		public string? HtmlDescription { get; set; }

		public DateTime LastUpdated { get; set; }

		public SpawnsetData SpawnsetData { get; set; }

		public string Name { get; set; }

		public string AuthorName { get; set; }

		public bool HasCustomLeaderboard { get; set; }
	}
}