using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics
{
	public class GetLeaderboardHistoryStatistics
	{
		public DateTime DateTime { get; init; }

		public int TotalPlayers { get; init; }
		public double TimeGlobal { get; init; }
		public ulong KillsGlobal { get; init; }
		public ulong GemsGlobal { get; init; }
		public ulong DeathsGlobal { get; init; }
		public ulong DaggersHitGlobal { get; init; }
		public ulong DaggersFiredGlobal { get; init; }
		public double Top10Entrance { get; init; }
		public double Top100Entrance { get; init; }

		public bool TotalPlayersUpdated { get; init; }
		public bool TimeGlobalUpdated { get; init; }
		public bool KillsGlobalUpdated { get; init; }
		public bool GemsGlobalUpdated { get; init; }
		public bool DeathsGlobalUpdated { get; init; }
		public bool DaggersHitGlobalUpdated { get; init; }
		public bool DaggersFiredGlobalUpdated { get; init; }
		public bool Top10EntranceUpdated { get; init; }
		public bool Top100EntranceUpdated { get; init; }
	}
}
