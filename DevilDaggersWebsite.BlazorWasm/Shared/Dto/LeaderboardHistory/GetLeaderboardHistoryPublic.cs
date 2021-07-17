using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory
{
	// This class must correspond to what's stored in the leaderboard history JSON.
	public class GetLeaderboardHistoryPublic
	{
		public DateTime DateTime { get; init; }

		public int Players { get; set; }

		// TODO: Translate all history TimeGlobal to double.
		public ulong TimeGlobal { get; set; }

		public ulong KillsGlobal { get; set; }

		public ulong GemsGlobal { get; set; }

		public ulong DeathsGlobal { get; set; }

		public ulong DaggersHitGlobal { get; set; }

		public ulong DaggersFiredGlobal { get; set; }

		public List<GetEntryHistoryPublic> Entries { get; } = new();
	}
}
