using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards
{
	public class AddCustomLeaderboard
	{
		public int SpawnsetId { get; init; }

		public CustomLeaderboardCategory Category { get; init; }

		[Range(1, 1500)]
		public double TimeBronze { get; init; }

		[Range(1, 1500)]
		public double TimeSilver { get; init; }

		[Range(1, 1500)]
		public double TimeGolden { get; init; }

		[Range(1, 1500)]
		public double TimeDevil { get; init; }

		[Range(1, 1500)]
		public double TimeLeviathan { get; init; }
	}
}
