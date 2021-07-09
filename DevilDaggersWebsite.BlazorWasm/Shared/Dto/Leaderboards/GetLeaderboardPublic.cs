using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Leaderboards
{
	public class GetLeaderboardPublic
	{
		public DateTime DateTime { get; init; }

		public int TotalPlayers { get; init; }

		public double TimeGlobal { get; init; }

		public ulong KillsGlobal { get; init; }

		public ulong GemsGlobal { get; init; }

		public ulong DeathsGlobal { get; init; }

		public ulong DaggersHitGlobal { get; init; }

		public ulong DaggersFiredGlobal { get; init; }

		public List<GetEntryPublic> Entries { get; init; } = new();
	}
}
