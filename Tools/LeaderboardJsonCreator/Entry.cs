using Newtonsoft.Json;

namespace LeaderboardJsonCreator
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Entry
	{
		[JsonProperty]
		public int Rank;
		[JsonProperty]
		public int ID;
		[JsonProperty]
		public string Username;
		[JsonProperty]
		public int Time;
		[JsonProperty]
		public int Kills;
		[JsonProperty]
		public int Gems;
		[JsonProperty]
		public int DeathType;
		[JsonProperty]
		public int ShotsHit;
		[JsonProperty]
		public int ShotsFired;
		[JsonProperty]
		public ulong TimeTotal;
		[JsonProperty]
		public ulong KillsTotal;
		[JsonProperty]
		public ulong GemsTotal;
		[JsonProperty]
		public ulong DeathsTotal;
		[JsonProperty]
		public ulong ShotsHitTotal;
		[JsonProperty]
		public ulong ShotsFiredTotal;
	}
}