using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.CustomLeaderboards
{
	public class EditCustomLeaderboard
	{
		public CustomLeaderboardCategory Category { get; init; }

		[Range(10000, 15000000)]
		public int TimeBronze { get; init; }

		[Range(10000, 15000000)]
		public int TimeSilver { get; init; }

		[Range(10000, 15000000)]
		public int TimeGolden { get; init; }

		[Range(10000, 15000000)]
		public int TimeDevil { get; init; }

		[Range(10000, 15000000)]
		public int TimeLeviathan { get; init; }

		public bool IsArchived { get; init; }
	}
}
