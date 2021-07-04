using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Dto.CustomLeaderboards
{
	public class GetCustomLeaderboard
	{
		public int Id { get; init; }

		public string SpawnsetName { get; init; } = null!;

		public string SpawnsetAuthorName { get; init; } = null!;

		public int TimeBronze { get; init; }

		public int TimeSilver { get; init; }

		public int TimeGolden { get; init; }

		public int TimeDevil { get; init; }

		public int TimeLeviathan { get; init; }

		public DateTime? DateLastPlayed { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }

		public bool IsAscending { get; init; }
	}
}
