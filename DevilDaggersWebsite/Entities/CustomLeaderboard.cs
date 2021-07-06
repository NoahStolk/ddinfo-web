using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomLeaderboard : IEntity
	{
		[Key]
		public int Id { get; init; }

		public int SpawnsetFileId { get; set; }

		[ForeignKey(nameof(SpawnsetFileId))]
		public SpawnsetFile SpawnsetFile { get; set; } = null!;

		public CustomLeaderboardCategory Category { get; set; }

		public int TimeBronze { get; set; }

		public int TimeSilver { get; set; }

		public int TimeGolden { get; set; }

		public int TimeDevil { get; set; }

		public int TimeLeviathan { get; set; }

		public DateTime? DateLastPlayed { get; set; }

		public DateTime? DateCreated { get; set; }

		public int TotalRunsSubmitted { get; set; }

		public bool IsArchived { get; set; }

		public CustomLeaderboardDagger GetDaggerFromTime(int time)
		{
			if (Category.IsAscending())
			{
				if (time <= TimeLeviathan)
					return CustomLeaderboardDagger.Leviathan;
				if (time <= TimeDevil)
					return CustomLeaderboardDagger.Devil;
				if (time <= TimeGolden)
					return CustomLeaderboardDagger.Golden;
				if (time <= TimeSilver)
					return CustomLeaderboardDagger.Silver;
				if (time <= TimeBronze)
					return CustomLeaderboardDagger.Bronze;

				return CustomLeaderboardDagger.Default;
			}

			if (time >= TimeLeviathan)
				return CustomLeaderboardDagger.Leviathan;
			if (time >= TimeDevil)
				return CustomLeaderboardDagger.Devil;
			if (time >= TimeGolden)
				return CustomLeaderboardDagger.Golden;
			if (time >= TimeSilver)
				return CustomLeaderboardDagger.Silver;
			if (time >= TimeBronze)
				return CustomLeaderboardDagger.Bronze;

			return CustomLeaderboardDagger.Default;
		}
	}
}
