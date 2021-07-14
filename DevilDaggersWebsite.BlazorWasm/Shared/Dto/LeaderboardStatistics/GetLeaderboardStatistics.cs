using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardStatistics
{
	public class GetLeaderboardStatistics
	{
		public DateTime DateTime { get; init; }
		public bool IsFetched { get; init; }
		public int TotalEntries { get; init; }

		public Dictionary<string, int> DaggerStatistics { get; init; } = null!;
		public Dictionary<string, int> DeathStatistics { get; init; } = null!;
		public Dictionary<string, int> EnemyStatistics { get; init; } = null!;

		public Dictionary<int, int> TimeStatistics { get; init; } = null!;
		public Dictionary<int, int> KillStatistics { get; init; } = null!;
		public Dictionary<int, int> GemStatistics { get; init; } = null!;

		public ArrayData Time { get; init; }
		public ArrayData Kills { get; init; }
		public ArrayData Gems { get; init; }
	}
}
