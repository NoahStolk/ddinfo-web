using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Server.Clients.OfficialLeaderboard
{
	public class LeaderboardResponse
	{
		public int TotalPlayers { get; set; }

		public ulong TimeGlobal { get; set; }

		public ulong KillsGlobal { get; set; }

		public ulong GemsGlobal { get; set; }

		public ulong DeathsGlobal { get; set; }

		public ulong DaggersHitGlobal { get; set; }

		public ulong DaggersFiredGlobal { get; set; }

		public ushort TotalEntries { get; set; }

		public List<EntryResponse> Entries { get; } = new();
	}
}
