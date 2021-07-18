using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomLeaderboards
{
	public class GetCustomLeaderboardOverview
	{
		public string SpawnsetName { get; init; } = null!;

		public string SpawnsetAuthorName { get; init; } = null!;

		public double TimeBronze { get; init; }

		public double TimeSilver { get; init; }

		public double TimeGolden { get; init; }

		public double TimeDevil { get; init; }

		public double TimeLeviathan { get; init; }

		public DateTime? DateLastPlayed { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }
	}
}
