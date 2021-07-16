namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory
{
	public class GetEntryHistoryPublic
	{
		public int Rank { get; set; }

		public int Id { get; set; }

		public string Username { get; set; } = null!;

		// TODO: Translate all history Time to double.
		public int Time { get; set; }

		public int Kills { get; set; }

		public int Gems { get; set; }

		public int DeathType { get; set; }

		public int DaggersHit { get; set; }

		public int DaggersFired { get; set; }

		// TODO: Translate all history TimeTotal to double.
		public ulong TimeTotal { get; set; }

		public ulong KillsTotal { get; set; }

		public ulong GemsTotal { get; set; }

		public ulong DeathsTotal { get; set; }

		public ulong DaggersHitTotal { get; set; }

		public ulong DaggersFiredTotal { get; set; }
	}
}
