using DevilDaggersInfo.Web.Server.Domain.Main.Tests.TestImplementations;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Tests.Utils;

public static class LeaderboardHistoryData
{
	private static readonly IReadOnlyDictionary<string, LeaderboardHistory> _data = new Dictionary<string, LeaderboardHistory>
	{
		["2022-01-01.bin"] = CreateLeaderboardHistory(new(2022, 1, 1), new()
		{
			CreateEntryHistory(1, 1, 90, "Player 1"),
			CreateEntryHistory(2, 2, 80, "Player 2"),
		}),
		["2022-01-02.bin"] = CreateLeaderboardHistory(new(2022, 1, 2), new()
		{
			CreateEntryHistory(1, 4, 100000, "Cheater"), // Cheater makes it to first place.
			CreateEntryHistory(2, 1, 90, "Player 1"),
			CreateEntryHistory(3, 2, 80, "Player 2"),
		}),
		["2022-01-03.bin"] = CreateLeaderboardHistory(new(2022, 1, 3), new()
		{
			CreateEntryHistory(1, 1, 95, "Player 1"),
			CreateEntryHistory(2, 2, 85, "Player 2"),
			CreateEntryHistory(3, 4, 0, "Cheater"), // Cheater is removed from the leaderboard.
		}),
		["2022-01-04.bin"] = CreateLeaderboardHistory(new(2022, 1, 4), new()
		{
			CreateEntryHistory(1, 4, 1000000, "Cheater"), // Cheater makes it to first place again.
			CreateEntryHistory(2, 1, 98, "Player 1"),
			CreateEntryHistory(3, 2, 85, "Player 2"),
			CreateEntryHistory(4, 3, 82, "Player 3"), // Player 3 joins the leaderboard.
		}),
	};

	public static IFileSystemService GetFileSystemService()
	{
		IFileSystemService fileSystemService = new TestFileSystemService();
		foreach (KeyValuePair<string, LeaderboardHistory> kvp in _data)
			fileSystemService.WriteAllBytes(Path.Combine(nameof(DataSubDirectory.LeaderboardHistory), kvp.Key), kvp.Value.ToBytes());

		return fileSystemService;
	}

	private static LeaderboardHistory CreateLeaderboardHistory(DateTime dateTime, List<EntryHistory> entries)
	{
		return new()
		{
			DateTime = dateTime,
			Entries = entries,
			Players = entries.Count,
			DeathsGlobal = 0,
			GemsGlobal = 0,
			KillsGlobal = 0,
			TimeGlobal = 0,
			DaggersFiredGlobal = 0,
			DaggersHitGlobal = 0,
		};
	}

	private static EntryHistory CreateEntryHistory(int rank, int id, int time, string username)
	{
		return new()
		{
			Id = id,
			Rank = rank,
			Time = time,
			Username = username,
			Gems = 0,
			Kills = 0,
			DaggersFired = 0,
			DaggersHit = 0,
			DeathsTotal = 0,
			DeathType = 0,
			GemsTotal = 0,
			KillsTotal = 0,
			TimeTotal = 0,
			DaggersFiredTotal = 0,
			DaggersHitTotal = 0,
		};
	}
}
