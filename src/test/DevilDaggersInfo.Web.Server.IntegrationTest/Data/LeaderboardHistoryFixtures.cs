using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

/// <summary>
/// Four daily leaderboard history snapshots in which a cheater (player 4) reaches first place, disappears, and returns,
/// while player 3 only joins on the last day. Seeded as real history files and real player rows, so the repositories
/// read them through the production <c>LeaderboardHistoryCache</c> and database.
/// </summary>
internal static class LeaderboardHistoryFixtures
{
	internal const int CheaterId = 4;

	/// <summary>
	/// Keyed by file name, in the <c>yyyyMMddHHmm.bin</c> format <c>LeaderboardHistoryBackgroundService</c> writes.
	/// </summary>
	internal static IReadOnlyDictionary<string, LeaderboardHistory> Snapshots { get; } = new Dictionary<string, LeaderboardHistory>(StringComparer.Ordinal)
	{
		["202201010000.bin"] = CreateLeaderboardHistory(
			Date(2022, 1, 1),
			[
				CreateEntryHistory(1, 1, 90, "Player 1"),
				CreateEntryHistory(2, 2, 80, "Player 2"),
			]),
		["202201020000.bin"] = CreateLeaderboardHistory(
			Date(2022, 1, 2),
			[
				CreateEntryHistory(1, CheaterId, 100000, "Cheater"), // Cheater makes it to first place.
				CreateEntryHistory(2, 1, 90, "Player 1"),
				CreateEntryHistory(3, 2, 80, "Player 2"),
			]),
		["202201030000.bin"] = CreateLeaderboardHistory(
			Date(2022, 1, 3),
			[
				CreateEntryHistory(1, 1, 95, "Player 1"),
				CreateEntryHistory(2, 2, 85, "Player 2"),
				CreateEntryHistory(3, CheaterId, 0, "Cheater"), // Cheater is removed from the leaderboard.
			]),
		["202201040000.bin"] = CreateLeaderboardHistory(
			Date(2022, 1, 4),
			[
				CreateEntryHistory(1, CheaterId, 1000000, "Cheater"), // Cheater makes it to first place again.
				CreateEntryHistory(2, 1, 98, "Player 1"),
				CreateEntryHistory(3, 2, 85, "Player 2"),
				CreateEntryHistory(4, 3, 82, "Player 3"), // Player 3 joins the leaderboard.
			]),
	};

	internal static DateTime Date(int year, int month, int day)
	{
		return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
	}

	/// <summary>
	/// Inserts players 1 to 3 and the cheater, and writes every snapshot to the application's data root. The cheater is
	/// banned unless a test passes <see cref="BanType.NotBanned"/> to ban them itself.
	/// </summary>
	internal static async Task SeedAsync(TestApplication app, BanType cheaterBanType = BanType.Cheater)
	{
		await app.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Player 1"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Player 2"));
			dbContext.Players.Add(EntityFixtures.Player(3, "Player 3"));

			PlayerEntity cheater = EntityFixtures.Player(CheaterId, "Cheater");
			cheater.BanType = cheaterBanType;
			dbContext.Players.Add(cheater);
		});

		await using AsyncServiceScope scope = app.CreateScope();
		IFileSystem fileSystem = scope.ServiceProvider.GetRequiredService<IFileSystem>();
		foreach ((string fileName, LeaderboardHistory history) in Snapshots)
			await fileSystem.WriteAllBytesAsync(DataSubDirectory.LeaderboardHistory, fileName, history.ToBytes());
	}

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
}
