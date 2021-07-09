using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.CustomLeaderboards
{
	public class GetCustomLeaderboard : IGetDto<int>
	{
		public int Id { get; init; }

		[Display(Name = "Name")]
		public string SpawnsetName { get; init; } = null!;

		[Display(Name = "Author")]
		public string SpawnsetAuthorName { get; init; } = null!;

		[Display(Name = "Bronze")]
		public float TimeBronze { get; init; }

		[Display(Name = "Silver")]
		public float TimeSilver { get; init; }

		[Display(Name = "Golden")]
		public float TimeGolden { get; init; }

		[Display(Name = "Devil")]
		public float TimeDevil { get; init; }

		[Display(Name = "Levi")]
		public float TimeLeviathan { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }
	}
}
