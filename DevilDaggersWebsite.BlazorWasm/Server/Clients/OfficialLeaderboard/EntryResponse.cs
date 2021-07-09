namespace DevilDaggersWebsite.BlazorWasm.Server.Clients.OfficialLeaderboard
{
	public class EntryResponse
	{
		public int Rank { get; init; }

		public int Id { get; init; }

		public string Username { get; init; } = null!;

		public int Time { get; init; }

		public int Kills { get; init; }

		public int Gems { get; init; }

		public short DeathType { get; init; }

		public int DaggersHit { get; init; }

		public int DaggersFired { get; init; }

		public ulong TimeTotal { get; init; }

		public ulong KillsTotal { get; init; }

		public ulong GemsTotal { get; init; }

		public ulong DeathsTotal { get; init; }

		public ulong DaggersHitTotal { get; init; }

		public ulong DaggersFiredTotal { get; init; }
	}
}
