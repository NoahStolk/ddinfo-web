using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards
{
	public class GetCustomLeaderboard : IGetDto<int>
	{
		public int Id { get; init; }

		[Display(Name = "Name")]
		public string SpawnsetName { get; init; } = null!;

		[Display(Name = "Author")]
		public string SpawnsetAuthorName { get; init; } = null!;

		[Format(FormatUtils.TimeFormat)]
		[Display(Name = "Bronze")]
		public double TimeBronze { get; init; }

		[Format(FormatUtils.TimeFormat)]
		[Display(Name = "Silver")]
		public double TimeSilver { get; init; }

		[Format(FormatUtils.TimeFormat)]
		[Display(Name = "Golden")]
		public double TimeGolden { get; init; }

		[Format(FormatUtils.TimeFormat)]
		[Display(Name = "Devil")]
		public double TimeDevil { get; init; }

		[Format(FormatUtils.TimeFormat)]
		[Display(Name = "Levi")]
		public double TimeLeviathan { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }
	}
}
