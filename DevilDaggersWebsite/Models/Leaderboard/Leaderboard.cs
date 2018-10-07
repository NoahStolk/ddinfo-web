using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Leaderboard
	{
		[JsonProperty]
		public DateTime DateTime { get; set; } = DateTime.UtcNow;

		public int Offset { get; set; } = 1;
		public int OffsetPrevious { get; set; } = 1;

		[JsonProperty]
		public int Players { get; set; }
		[JsonProperty]
		public ulong TimeGlobal { get; set; }
		[JsonProperty]
		public ulong KillsGlobal { get; set; }
		[JsonProperty]
		public ulong GemsGlobal { get; set; }
		[JsonProperty]
		public ulong DeathsGlobal { get; set; }
		[JsonProperty]
		public ulong ShotsHitGlobal { get; set; }
		[JsonProperty]
		public ulong ShotsFiredGlobal { get; set; }

		[JsonProperty]
		public List<Entry> Entries { get; set; } = new List<Entry>();

		public double AccuracyGlobal => ShotsFiredGlobal == 0 ? 0 : ShotsHitGlobal / (double)ShotsFiredGlobal * 100;
	}
}