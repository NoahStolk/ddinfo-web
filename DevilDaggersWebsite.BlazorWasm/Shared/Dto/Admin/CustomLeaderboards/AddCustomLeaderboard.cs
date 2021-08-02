using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards
{
	public class AddCustomLeaderboard
	{
		public int SpawnsetId { get; init; }

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
