using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistoryStatistics
{
	public class GetLeaderboardHistoryStatisticsPublic
	{
		public DateTime DateTime { get; init; }

		public int? TotalPlayers { get; init; }

		public double? TimeGlobal { get; init; }

		public ulong? KillsGlobal { get; init; }

		public ulong? GemsGlobal { get; init; }

		public ulong? DeathsGlobal { get; init; }

		public ulong? DaggersHitGlobal { get; init; }

		public ulong? DaggersFiredGlobal { get; init; }

		public double? Top10Entrance { get; init; }

		public double? Top100Entrance { get; init; }
	}
}
