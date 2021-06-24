using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class CustomLeaderboard
	{
		public CustomLeaderboardCategory Category { get; init; }

		public string SpawnsetName { get; init; } = null!;
		public string AuthorName { get; init; } = null!;

		public int TimeBronze { get; init; }
		public int TimeSilver { get; init; }
		public int TimeGolden { get; init; }
		public int TimeDevil { get; init; }
		public int TimeLeviathan { get; init; }
		public CustomEntry? WorldRecord { get; init; }
		public string? WorldRecordDaggerName { get; init; }

		public DateTime? DateLastPlayed { get; init; }
		public DateTime? DateCreated { get; init; }
		public int TotalRunsSubmitted { get; init; }
		public int TotalPlayers { get; init; }
	}
}
