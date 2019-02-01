using System;
using System.Collections.Generic;

namespace DevilDaggersUtilities.Tools
{
	public class Leaderboard
	{
		public DateTime DateTime = DateTime.UtcNow;

		public int Players;
		public ulong TimeGlobal;
		public ulong KillsGlobal;
		public ulong GemsGlobal;
		public ulong DeathsGlobal;
		public ulong ShotsHitGlobal;
		public ulong ShotsFiredGlobal;

		public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();
	}
}