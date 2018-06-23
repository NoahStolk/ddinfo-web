using System.Collections.Generic;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	public class Leaderboard
	{
		public int Offset { get; set; } = 1;
		public int OffsetPrevious { get; set; } = 1;

		public List<Entry> Entries { get; set; } = new List<Entry>();

		public int Players { get; set; }
		public ulong TimeGlobal { get; set; }
		public ulong KillsGlobal { get; set; }
		public ulong GemsGlobal { get; set; }
		public ulong DeathsGlobal { get; set; }
		public ulong ShotsHitGlobal { get; set; }
		public ulong ShotsFiredGlobal { get; set; }

		public double AccuracyGlobal => ShotsFiredGlobal == 0 ? 0 : ShotsHitGlobal / (double)ShotsFiredGlobal * 100;
	}
}