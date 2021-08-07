using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomLeaderboards
{
	public class GetCustomLeaderboardOverview
	{
		public int Id { get; init; }

		public string SpawnsetName { get; init; } = null!;

		public string SpawnsetAuthorName { get; init; } = null!;

		public double TimeBronze { get; init; }

		public double TimeSilver { get; init; }

		public double TimeGolden { get; init; }

		public double TimeDevil { get; init; }

		public double TimeLeviathan { get; init; }

		public DateTime? DateLastPlayed { get; init; }

		public DateTime? DateCreated { get; init; }

		public string? TopPlayer { get; init; }

		public double? WorldRecord { get; init; }
	}
}
