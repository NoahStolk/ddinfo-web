using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

public class LeaderboardResponseParser
{
	public IDdLeaderboardService.LeaderboardResponse ParseGetLeaderboardResponse(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		ulong deathsGlobal = br.ReadUInt64();
		ulong killsGlobal = br.ReadUInt64();
		ulong daggersFiredGlobal = br.ReadUInt64();
		ulong timeGlobal = br.ReadUInt64();
		ulong gemsGlobal = br.ReadUInt64();
		ulong daggersHitGlobal = br.ReadUInt64();
		ushort totalEntries = br.ReadUInt16();

		br.BaseStream.Seek(14, SeekOrigin.Current);
		int totalPlayers = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);

		List<IDdLeaderboardService.EntryResponse> entries = new();
		for (int i = 0; i < totalEntries; i++)
		{
			short usernameLength = br.ReadInt16();
			string username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			int rank = br.ReadInt32();
			int id = br.ReadInt32();
			int time = br.ReadInt32();
			int kills = br.ReadInt32();
			int daggersFired = br.ReadInt32();
			int daggersHit = br.ReadInt32();
			int gems = br.ReadInt32();
			int deathType = br.ReadInt32();
			ulong deathsTotal = br.ReadUInt64();
			ulong killsTotal = br.ReadUInt64();
			ulong daggersFiredTotal = br.ReadUInt64();
			ulong timeTotal = br.ReadUInt64();
			ulong gemsTotal = br.ReadUInt64();
			ulong daggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			IDdLeaderboardService.EntryResponse entry = new()
			{
				Username = username,
				Rank = rank,
				Id = id,
				Time = time,
				Kills = kills,
				DaggersFired = daggersFired,
				DaggersHit = daggersHit,
				Gems = gems,
				DeathType = deathType,
				DeathsTotal = deathsTotal,
				KillsTotal = killsTotal,
				DaggersFiredTotal = daggersFiredTotal,
				TimeTotal = timeTotal,
				GemsTotal = gemsTotal,
				DaggersHitTotal = daggersHitTotal,
			};

			entries.Add(entry);
		}

		return new()
		{
			DateTime = DateTime.UtcNow,
			DeathsGlobal = deathsGlobal,
			KillsGlobal = killsGlobal,
			DaggersFiredGlobal = daggersFiredGlobal,
			TimeGlobal = timeGlobal,
			GemsGlobal = gemsGlobal,
			DaggersHitGlobal = daggersHitGlobal,
			TotalEntries = totalEntries,
			TotalPlayers = totalPlayers,
			Entries = entries,
		};
	}

	public List<IDdLeaderboardService.EntryResponse> ParseGetEntriesByName(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		short totalResults = br.ReadInt16();

		br.BaseStream.Seek(6, SeekOrigin.Current);

		List<IDdLeaderboardService.EntryResponse> entries = new();
		for (int i = 0; i < Math.Min((short)100, totalResults); i++)
		{
			short usernameLength = br.ReadInt16();
			string username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			int rank = br.ReadInt32();
			int id = br.ReadInt32();

			br.BaseStream.Seek(4, SeekOrigin.Current);
			int time = br.ReadInt32();
			int kills = br.ReadInt32();
			int daggersFired = br.ReadInt32();
			int daggersHit = br.ReadInt32();
			int gems = br.ReadInt32();
			int deathType = br.ReadInt32();
			ulong deathsTotal = br.ReadUInt64();
			ulong killsTotal = br.ReadUInt64();
			ulong daggersFiredTotal = br.ReadUInt64();
			ulong timeTotal = br.ReadUInt64();
			ulong gemsTotal = br.ReadUInt64();
			ulong daggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			IDdLeaderboardService.EntryResponse entry = new()
			{
				Username = username,
				Rank = rank,
				Id = id,
				Time = time,
				Kills = kills,
				DaggersFired = daggersFired,
				DaggersHit = daggersHit,
				Gems = gems,
				DeathType = deathType,
				DeathsTotal = deathsTotal,
				KillsTotal = killsTotal,
				DaggersFiredTotal = daggersFiredTotal,
				TimeTotal = timeTotal,
				GemsTotal = gemsTotal,
				DaggersHitTotal = daggersHitTotal,
			};

			entries.Add(entry);
		}

		return entries;
	}

	public List<IDdLeaderboardService.EntryResponse> ParseGetEntriesByIds(byte[] response)
	{
		using MemoryStream ms = new(response);
		using BinaryReader br = new(ms);

		br.BaseStream.Seek(19, SeekOrigin.Begin);

		List<IDdLeaderboardService.EntryResponse> entries = new();
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
		short usernameLength = br.ReadInt16();
		string username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
		int rank = br.ReadInt32();
		int id = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);
		int time = br.ReadInt32();
		int kills = br.ReadInt32();
		int daggersFired = br.ReadInt32();
		int daggersHit = br.ReadInt32();
		int gems = br.ReadInt32();
		int deathType = br.ReadInt32();
		ulong deathsTotal = br.ReadUInt64();
		ulong killsTotal = br.ReadUInt64();
		ulong daggersFiredTotal = br.ReadUInt64();
		ulong timeTotal = br.ReadUInt64();
		ulong gemsTotal = br.ReadUInt64();
		ulong daggersHitTotal = br.ReadUInt64();

		return new()
		{
			Username = username,
			Rank = rank,
			Id = id,
			Time = time,
			Kills = kills,
			DaggersFired = daggersFired,
			DaggersHit = daggersHit,
			Gems = gems,
			DeathType = deathType,
			DeathsTotal = deathsTotal,
			KillsTotal = killsTotal,
			DaggersFiredTotal = daggersFiredTotal,
			TimeTotal = timeTotal,
			GemsTotal = gemsTotal,
			DaggersHitTotal = daggersHitTotal,
		};
	}
}
