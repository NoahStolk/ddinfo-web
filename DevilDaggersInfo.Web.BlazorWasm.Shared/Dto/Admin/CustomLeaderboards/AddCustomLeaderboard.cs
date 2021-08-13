using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards
{
	public class AddCustomLeaderboard
	{
		public int SpawnsetId { get; set; }

		public CustomLeaderboardCategory Category { get; set; }

		[Range(1, 1500)]
		public double TimeBronze { get; set; }

		[Range(1, 1500)]
		public double TimeSilver { get; set; }

		[Range(1, 1500)]
		public double TimeGolden { get; set; }

		[Range(1, 1500)]
		public double TimeDevil { get; set; }

		[Range(1, 1500)]
		public double TimeLeviathan { get; set; }
	}
}
