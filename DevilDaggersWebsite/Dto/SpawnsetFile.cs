using DevilDaggersCore.Spawnsets;
using System;

namespace DevilDaggersWebsite.Dto
{
	public class SpawnsetFile
	{
		public int? MaxDisplayWaves { get; set; }

		public string? HtmlDescription { get; set; }

		public DateTime LastUpdated { get; set; }

		public SpawnsetData SpawnsetData { get; set; } = null!;

		public string Name { get; set; } = null!;

		public string AuthorName { get; set; } = null!;

		public bool HasCustomLeaderboard { get; set; }

		public bool IsPractice { get; set; }
	}
}
