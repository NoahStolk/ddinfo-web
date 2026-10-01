using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Utils;

internal sealed class TestData : ILeaderboardHistoryCache
{
	private readonly IReadOnlyDictionary<string, LeaderboardHistory> _leaderboardHistory = new Dictionary<string, LeaderboardHistory>
	{
		["2022-01-01.bin"] = CreateLeaderboardHistory(
			new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			[
				CreateEntryHistory(1, 1, 90, "Player 1"),
				CreateEntryHistory(2, 2, 80, "Player 2"),
			]),
		["2022-01-02.bin"] = CreateLeaderboardHistory(
			new DateTime(2022, 1, 2, 0, 0, 0, DateTimeKind.Utc),
			[
				CreateEntryHistory(1, 4, 100000, "Cheater"), // Cheater makes it to first place.
				CreateEntryHistory(2, 1, 90, "Player 1"),
				CreateEntryHistory(3, 2, 80, "Player 2"),
			]),
		["2022-01-03.bin"] = CreateLeaderboardHistory(
			new DateTime(2022, 1, 3, 0, 0, 0, DateTimeKind.Utc),
			[
				CreateEntryHistory(1, 1, 95, "Player 1"),
				CreateEntryHistory(2, 2, 85, "Player 2"),
				CreateEntryHistory(3, 4, 0, "Cheater"), // Cheater is removed from the leaderboard.
			]),
		["2022-01-04.bin"] = CreateLeaderboardHistory(
			new DateTime(2022, 1, 4, 0, 0, 0, DateTimeKind.Utc),
			[
				CreateEntryHistory(1, 4, 1000000, "Cheater"), // Cheater makes it to first place again.
				CreateEntryHistory(2, 1, 98, "Player 1"),
				CreateEntryHistory(3, 2, 85, "Player 2"),
				CreateEntryHistory(4, 3, 82, "Player 3"), // Player 3 joins the leaderboard.
			]),
	};

	private readonly IReadOnlyDictionary<string, string> _modArchiveCache = new Dictionary<string, string>
	{
		["test"] = """{"FileSize":8400,"FileSizeExtracted":21891,"Binaries":[{"Name":"dd-test-main","Size":21891,"ModBinaryType":1,"Chunks":[{"Name":"dagger6","Size":21855,"AssetType":2,"IsProhibited":false}],"ModifiedLoudnessAssets":null}]}""",
	};

	public TestData()
	{
		// The leaderboard history contents are served by this class as ILeaderboardHistoryCache, so the files only need to exist.
		foreach (string fileName in _leaderboardHistory.Keys)
			FileSystem.Seed(DataSubDirectory.LeaderboardHistory, fileName, []);

		foreach ((string modName, string json) in _modArchiveCache)
			FileSystem.Seed(DataSubDirectory.ModArchiveCache, $"{modName}.json", Encoding.UTF8.GetBytes(json));
	}

	public InMemoryFileSystem FileSystem { get; } = new();

	private static LeaderboardHistory CreateLeaderboardHistory(DateTime dateTime, List<EntryHistory> entries)
	{
		return new LeaderboardHistory
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
		return new EntryHistory
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

	public Task<LeaderboardHistory> GetLeaderboardHistoryAsync(string fileName)
	{
		return Task.FromResult(_leaderboardHistory[fileName]);
	}

	public int GetCount()
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}
}
