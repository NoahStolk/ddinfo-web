namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface IDdLeaderboardService
{
	Task<LeaderboardResponse> GetLeaderboard(int rankStart);

	Task<List<EntryResponse>> GetEntriesByName(string name);

	Task<List<EntryResponse>> GetEntriesByIds(IEnumerable<int> ids);

	Task<EntryResponse> GetEntryById(int id);

	public record LeaderboardResponse
	{
		public required DateTime DateTime { get; init; }

		public required int TotalPlayers { get; init; }

		public required ulong TimeGlobal { get; init; }

		public required ulong KillsGlobal { get; init; }

		public required ulong GemsGlobal { get; init; }

		public required ulong DeathsGlobal { get; init; }

		public required ulong DaggersHitGlobal { get; init; }

		public required ulong DaggersFiredGlobal { get; init; }

		public required ushort TotalEntries { get; init; }

		public required List<EntryResponse> Entries { get; init; }
	}

	public record EntryResponse
	{
		public required int Rank { get; init; }

		public required int Id { get; init; }

		public required string Username { get; init; }

		public required int Time { get; init; }

		public required int Kills { get; init; }

		public required int Gems { get; init; }

		public required int DeathType { get; init; }

		public required int DaggersHit { get; init; }

		public required int DaggersFired { get; init; }

		public required ulong TimeTotal { get; init; }

		public required ulong KillsTotal { get; init; }

		public required ulong GemsTotal { get; init; }

		public required ulong DeathsTotal { get; init; }

		public required ulong DaggersHitTotal { get; init; }

		public required ulong DaggersFiredTotal { get; init; }
	}
}
