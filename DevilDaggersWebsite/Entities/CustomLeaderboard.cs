using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomLeaderboard
	{
		[Key]
		public int Id { get; set; }

		public CustomLeaderboardCategory Category { get; set; }

		public int SpawnsetFileId { get; set; }

		[ForeignKey(nameof(SpawnsetFileId))]
		public SpawnsetFile SpawnsetFile { get; set; } = null!;

		public int TimeBronze { get; set; }

		public int TimeSilver { get; set; }

		public int TimeGolden { get; set; }

		public int TimeDevil { get; set; }

		public int TimeLeviathan { get; set; }

		public DateTime? DateLastPlayed { get; set; }

		public DateTime? DateCreated { get; set; }

		public int TotalRunsSubmitted { get; set; }

		public bool IsAscending()
			=> Category == CustomLeaderboardCategory.Challenge || Category == CustomLeaderboardCategory.Speedrun;
	}
}
