using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Clients
{
	public class Leaderboard
	{
		/// <summary>
		/// Represents the UTC date and time when the leaderboard was fetched.
		/// </summary>
		public DateTime DateTime { get; set; } = DateTime.UtcNow;

		public int Players { get; set; }

		public ulong TimeGlobal { get; set; }

		public ulong KillsGlobal { get; set; }

		public ulong GemsGlobal { get; set; }

		public ulong DeathsGlobal { get; set; }

		public ulong DaggersHitGlobal { get; set; }

		public ulong DaggersFiredGlobal { get; set; }

		public List<Entry> Entries { get; set; } = new();

		[JsonIgnore]
		public double AccuracyGlobal => DaggersFiredGlobal == 0 ? 0 : DaggersHitGlobal / (double)DaggersFiredGlobal;
	}
}
