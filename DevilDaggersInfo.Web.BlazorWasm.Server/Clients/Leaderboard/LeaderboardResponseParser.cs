namespace DevilDaggersInfo.Web.BlazorWasm.Server.Clients.Leaderboard;

public static class LeaderboardResponseParser
{
	public static LeaderboardResponse ParseGetLeaderboardResponse(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		LeaderboardResponse leaderboard = new()
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
			EntryResponse entry = new();

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

	public static List<EntryResponse> ParseGetEntriesByName(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		List<EntryResponse> entries = new();

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		short totalResults = br.ReadInt16();
		if (totalResults > 100)
		{
			// TODO: Log warning.
			return entries;
		}

		br.BaseStream.Seek(6, SeekOrigin.Current);
		for (int i = 0; i < totalResults; i++)
		{
			EntryResponse entry = new();

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

	public static List<EntryResponse> ParseGetEntriesByIds(byte[] response, int entryCount)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		List<EntryResponse> entries = new();

		br.BaseStream.Seek(19, SeekOrigin.Begin);
		for (int i = 0; i < entryCount; i++)
		{
			EntryResponse entry = new();

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

	public static EntryResponse ParseGetEntryById(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		EntryResponse entry = new();

		br.BaseStream.Seek(19, SeekOrigin.Begin);

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
