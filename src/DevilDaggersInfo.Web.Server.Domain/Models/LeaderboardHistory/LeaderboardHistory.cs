namespace DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;

public record LeaderboardHistory
{
	public required DateTime DateTime { get; init; }

	public required int Players { get; init; }

	public required ulong TimeGlobal { get; init; }

	public required ulong KillsGlobal { get; init; }

	public required ulong GemsGlobal { get; init; }

	public required ulong DeathsGlobal { get; init; }

	public required ulong DaggersHitGlobal { get; init; }

	public required ulong DaggersFiredGlobal { get; init; }

	public required List<EntryHistory> Entries { get; init; } = [];

	public static LeaderboardHistory CreateFromFile(byte[] bytes)
	{
		using MemoryStream ms = new(bytes);
		using BinaryReader br = new(ms);
		_ = br.ReadInt32(); // Version

		LeaderboardHistory leaderboardHistory = new()
		{
			DateTime = new DateTime(br.ReadInt64()),
			Players = br.ReadInt32(),
			TimeGlobal = br.ReadUInt64(),
			KillsGlobal = br.ReadUInt64(),
			GemsGlobal = br.ReadUInt64(),
			DeathsGlobal = br.ReadUInt64(),
			DaggersHitGlobal = br.ReadUInt64(),
			DaggersFiredGlobal = br.ReadUInt64(),
			Entries = [],
		};

		int entryCount = br.ReadInt32();
		for (int i = 0; i < entryCount; i++)
		{
			EntryHistory entryHistory = new()
			{
				Rank = br.ReadInt32(),
				Id = br.ReadInt32(),
				Username = br.ReadString(),
				Time = br.ReadInt32(),
				Kills = br.ReadInt32(),
				Gems = br.ReadInt32(),
				DeathType = br.ReadByte(),
				DaggersHit = br.ReadInt32(),
				DaggersFired = br.ReadInt32(),
				TimeTotal = br.ReadUInt64(),
				KillsTotal = br.ReadUInt64(),
				GemsTotal = br.ReadUInt64(),
				DeathsTotal = br.ReadUInt64(),
				DaggersHitTotal = br.ReadUInt64(),
				DaggersFiredTotal = br.ReadUInt64(),
			};

			leaderboardHistory.Entries.Add(entryHistory);
		}

		return leaderboardHistory;
	}

	public byte[] ToBytes()
	{
		const uint version = 0;

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(version);
		bw.Write(DateTime.Ticks);
		bw.Write(Players);
		bw.Write(TimeGlobal);
		bw.Write(KillsGlobal);
		bw.Write(GemsGlobal);
		bw.Write(DeathsGlobal);
		bw.Write(DaggersHitGlobal);
		bw.Write(DaggersFiredGlobal);

		bw.Write(Entries.Count);
		foreach (EntryHistory entryHistory in Entries)
		{
			bw.Write(entryHistory.Rank);
			bw.Write(entryHistory.Id);
			bw.Write(entryHistory.Username);
			bw.Write(entryHistory.Time);
			bw.Write(entryHistory.Kills);
			bw.Write(entryHistory.Gems);
			bw.Write(entryHistory.DeathType);
			bw.Write(entryHistory.DaggersHit);
			bw.Write(entryHistory.DaggersFired);
			bw.Write(entryHistory.TimeTotal);
			bw.Write(entryHistory.KillsTotal);
			bw.Write(entryHistory.GemsTotal);
			bw.Write(entryHistory.DeathsTotal);
			bw.Write(entryHistory.DaggersHitTotal);
			bw.Write(entryHistory.DaggersFiredTotal);
		}

		return ms.ToArray();
	}
}
