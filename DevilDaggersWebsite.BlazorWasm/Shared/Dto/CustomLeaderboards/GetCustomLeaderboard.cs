using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomLeaderboards
{
	public class GetCustomLeaderboard : IGetDto<int>
	{
		public int Id { get; init; }

		[Display(Name = "Name")]
		public string SpawnsetName { get; init; } = null!;

		[Display(Name = "Author")]
		public string SpawnsetAuthorName { get; init; } = null!;

		[Display(Name = "Bronze")]
		public double TimeBronze { get; init; }

		[Display(Name = "Silver")]
		public double TimeSilver { get; init; }

		[Display(Name = "Golden")]
		public double TimeGolden { get; init; }

		[Display(Name = "Devil")]
		public double TimeDevil { get; init; }

		[Display(Name = "Levi")]
		public double TimeLeviathan { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }
	}
}
