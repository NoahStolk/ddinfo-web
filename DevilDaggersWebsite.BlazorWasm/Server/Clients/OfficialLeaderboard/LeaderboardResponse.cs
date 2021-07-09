using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Server.Clients.OfficialLeaderboard
{
	public class LeaderboardResponse
	{
		public int Players { get; init; }

		public ulong TimeGlobal { get; init; }

		public ulong KillsGlobal { get; init; }

		public ulong GemsGlobal { get; init; }

		public ulong DeathsGlobal { get; init; }

		public ulong DaggersHitGlobal { get; init; }

		public ulong DaggersFiredGlobal { get; init; }

		public List<EntryResponse> Entries { get; init; } = new();
	}
}
