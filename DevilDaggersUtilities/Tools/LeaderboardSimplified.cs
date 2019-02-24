using System;
using System.Collections.Generic;

namespace DevilDaggersUtilities.Tools
{
	public class LeaderboardSimplified
	{
		public DateTime DateTime = DateTime.UtcNow;

		public int Players;
		public ulong TimeGlobal;
		public ulong KillsGlobal;
		public ulong GemsGlobal;
		public ulong DeathsGlobal;
		public ulong ShotsHitGlobal;
		public ulong ShotsFiredGlobal;

		public List<LeaderboardEntrySimplified> Entries = new List<LeaderboardEntrySimplified>();
	}
}