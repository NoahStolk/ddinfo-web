namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface IDdLeaderboardService
{
	Task<LeaderboardResponse> GetLeaderboard(int rankStart);

	Task<List<EntryResponse>> GetEntriesByName(string name);

	Task<List<EntryResponse>> GetEntriesByIds(IEnumerable<int> ids);

	Task<EntryResponse> GetEntryById(int id);

	public class LeaderboardResponse
	{
		public DateTime DateTime { get; init; }

		public int TotalPlayers { get; set; }

		public ulong TimeGlobal { get; set; }

		public ulong KillsGlobal { get; set; }

		public ulong GemsGlobal { get; set; }

		public ulong DeathsGlobal { get; set; }

		public ulong DaggersHitGlobal { get; set; }

		public ulong DaggersFiredGlobal { get; set; }

		public ushort TotalEntries { get; set; }

		public List<EntryResponse> Entries { get; } = new();
	}

	public class EntryResponse
	{
		public int Rank { get; set; }

		public int Id { get; set; }

		public string Username { get; set; } = null!;

		public int Time { get; set; }

		public int Kills { get; set; }

		public int Gems { get; set; }

		public int DeathType { get; set; }

		public int DaggersHit { get; set; }

		public int DaggersFired { get; set; }

		public ulong TimeTotal { get; set; }

		public ulong KillsTotal { get; set; }

		public ulong GemsTotal { get; set; }

		public ulong DeathsTotal { get; set; }

		public ulong DaggersHitTotal { get; set; }

		public ulong DaggersFiredTotal { get; set; }
	}
}
