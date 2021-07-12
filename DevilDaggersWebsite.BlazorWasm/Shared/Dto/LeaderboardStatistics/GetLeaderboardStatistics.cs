using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardStatistics
{
	public class GetLeaderboardStatistics
	{
		public Dictionary<string, int> DaggerStatistics { get; init; } = null!;
		public Dictionary<string, int> DeathStatistics { get; init; } = null!;
		public Dictionary<string, int> EnemyStatistics { get; init; } = null!;
		public Dictionary<int, int> TimeStatistics { get; init; } = null!;

		public DateTime DateTime { get; init; }
		public bool IsFetched { get; init; }
		public int TotalEntries { get; init; }

		public int AverageTimeInTenthsOfMilliseconds { get; init; }
		public float AverageKills { get; init; }
		public float AverageGems { get; init; }
	}
}
