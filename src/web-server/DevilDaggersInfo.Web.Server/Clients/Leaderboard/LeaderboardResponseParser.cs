using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

public class LeaderboardResponseParser
{
	public IDdLeaderboardService.LeaderboardResponse ParseGetLeaderboardResponse(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		IDdLeaderboardService.LeaderboardResponse leaderboard = new()
		{
			DateTime = DateTime.UtcNow,
		};

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		leaderboard.DeathsGlobal = br.ReadUInt64();
		leaderboard.KillsGlobal = br.ReadUInt64();
		leaderboard.DaggersFiredGlobal = br.ReadUInt64();
		leaderboard.TimeGlobal = br.ReadUInt64();
		leaderboard.GemsGlobal = br.ReadUInt64();
		leaderboard.DaggersHitGlobal = br.ReadUInt64();
		leaderboard.TotalEntries = br.ReadUInt16();

		br.BaseStream.Seek(14, SeekOrigin.Current);
		leaderboard.TotalPlayers = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);
		for (int i = 0; i < leaderboard.TotalEntries; i++)
		{
			IDdLeaderboardService.EntryResponse entry = new();

			short usernameLength = br.ReadInt16();
			entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			entry.Rank = br.ReadInt32();
			entry.Id = br.ReadInt32();
			entry.Time = br.ReadInt32();
			entry.Kills = br.ReadInt32();
			entry.DaggersFired = br.ReadInt32();
			entry.DaggersHit = br.ReadInt32();
			entry.Gems = br.ReadInt32();
			entry.DeathType = br.ReadInt32();
			entry.DeathsTotal = br.ReadUInt64();
			entry.KillsTotal = br.ReadUInt64();
			entry.DaggersFiredTotal = br.ReadUInt64();
			entry.TimeTotal = br.ReadUInt64();
			entry.GemsTotal = br.ReadUInt64();
			entry.DaggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			leaderboard.Entries.Add(entry);
		}

		return leaderboard;
	}

	public List<IDdLeaderboardService.EntryResponse> ParseGetEntriesByName(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		List<IDdLeaderboardService.EntryResponse> entries = new();

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		short totalResults = br.ReadInt16();

		br.BaseStream.Seek(6, SeekOrigin.Current);
		for (int i = 0; i < Math.Min((short)100, totalResults); i++)
		{
			IDdLeaderboardService.EntryResponse entry = new();

			short usernameLength = br.ReadInt16();
			entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			entry.Rank = br.ReadInt32();
			entry.Id = br.ReadInt32();

			br.BaseStream.Seek(4, SeekOrigin.Current);
			entry.Time = br.ReadInt32();
			entry.Kills = br.ReadInt32();
			entry.DaggersFired = br.ReadInt32();
			entry.DaggersHit = br.ReadInt32();
			entry.Gems = br.ReadInt32();
			entry.DeathType = br.ReadInt32();
			entry.DeathsTotal = br.ReadUInt64();
			entry.KillsTotal = br.ReadUInt64();
			entry.DaggersFiredTotal = br.ReadUInt64();
			entry.TimeTotal = br.ReadUInt64();
			entry.GemsTotal = br.ReadUInt64();
			entry.DaggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			entries.Add(entry);
		}

		return entries;
	}

	public List<IDdLeaderboardService.EntryResponse> ParseGetEntriesByIds(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		List<IDdLeaderboardService.EntryResponse> entries = new();

		br.BaseStream.Seek(19, SeekOrigin.Begin);
		while (br.BaseStream.Position < br.BaseStream.Length)
		{
			entries.Add(ReadEntry(br));
			br.BaseStream.Seek(4, SeekOrigin.Current);
		}

		return entries;
	}

	public IDdLeaderboardService.EntryResponse ParseGetEntryById(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		br.BaseStream.Seek(19, SeekOrigin.Begin);

		return ReadEntry(br);
	}

	private static IDdLeaderboardService.EntryResponse ReadEntry(BinaryReader br)
	{
		IDdLeaderboardService.EntryResponse entry = new();

		short usernameLength = br.ReadInt16();
		entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
		entry.Rank = br.ReadInt32();
		entry.Id = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);
		entry.Time = br.ReadInt32();
		entry.Kills = br.ReadInt32();
		entry.DaggersFired = br.ReadInt32();
		entry.DaggersHit = br.ReadInt32();
		entry.Gems = br.ReadInt32();
		entry.DeathType = br.ReadInt32();
		entry.DeathsTotal = br.ReadUInt64();
		entry.KillsTotal = br.ReadUInt64();
		entry.DaggersFiredTotal = br.ReadUInt64();
		entry.TimeTotal = br.ReadUInt64();
		entry.GemsTotal = br.ReadUInt64();
		entry.DaggersHitTotal = br.ReadUInt64();

		return entry;
	}
}
