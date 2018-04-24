using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Models
{
	public class Leaderboard
	{
		public int Offset { get; set; } = 1;
		public int OffsetPrevious { get; set; } = 1;

		public List<Entry> Entries { get; set; } = new List<Entry>();

		public int Players { get; set; }
		public UInt64 TimeGlobal { get; set; }
		public UInt64 KillsGlobal { get; set; }
		public UInt64 GemsGlobal { get; set; }
		public UInt64 DeathsGlobal { get; set; }
		public UInt64 ShotsHitGlobal { get; set; }
		public UInt64 ShotsFiredGlobal { get; set; }
	}
}