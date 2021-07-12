using DevilDaggersCore.Spawnsets;
using System;

namespace DevilDaggersWebsite.Dto.Spawnsets
{
	public class GetSpawnsetPublic
	{
		public int? MaxDisplayWaves { get; init; }

		public string? HtmlDescription { get; init; }

		public DateTime LastUpdated { get; init; }

		public SpawnsetData SpawnsetData { get; init; } = null!;

		public string Name { get; init; } = null!;

		public string AuthorName { get; init; } = null!;

		public bool HasCustomLeaderboard { get; init; }

		public bool IsPractice { get; init; }
	}
}
