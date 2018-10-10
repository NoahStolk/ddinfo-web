using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LeaderboardJsonCreator
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Leaderboard
	{
		[JsonProperty]
		public DateTime DateTime = DateTime.UtcNow;

		[JsonProperty]
		public int Players;
		[JsonProperty]
		public ulong TimeGlobal;
		[JsonProperty]
		public ulong KillsGlobal;
		[JsonProperty]
		public ulong GemsGlobal;
		[JsonProperty]
		public ulong DeathsGlobal;
		[JsonProperty]
		public ulong ShotsHitGlobal;
		[JsonProperty]
		public ulong ShotsFiredGlobal;

		[JsonProperty]
		public List<Entry> Entries = new List<Entry>();
	}
}