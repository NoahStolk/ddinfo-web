using System;

namespace DevilDaggersWebsite.Models
{
	public class Entry
	{
		public int Rank { get; set; }
		public int ID { get; set; }
		public string Username { get; set; }
		public int Time { get; set; }
		public int Kills { get; set; }
		public int Gems { get; set; }
		public int DeathType { get; set; }
		public int ShotsHit { get; set; }
		public int ShotsFired { get; set; }
		public UInt64 TimeTotal { get; set; }
		public UInt64 KillsTotal { get; set; }
		public UInt64 GemsTotal { get; set; }
		public UInt64 DeathsTotal { get; set; }
		public UInt64 ShotsHitTotal { get; set; }
		public UInt64 ShotsFiredTotal { get; set; }
	}
}