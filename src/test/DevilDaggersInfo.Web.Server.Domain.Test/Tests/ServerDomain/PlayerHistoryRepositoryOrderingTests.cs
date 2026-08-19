using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

// This lives in its own class because TestDbContext seeds in its constructor, and MSTest constructs the test class once
// per test method, which would seed the same in-memory database twice.
[TestClass]
public class PlayerHistoryRepositoryOrderingTests
{
	private readonly TestData _data = new();
	private readonly TestDbContext _dbContext;

	public PlayerHistoryRepositoryOrderingTests()
	{
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: nameof(PlayerHistoryRepositoryOrderingTests))
			.Options;
		_dbContext = new TestDbContext(options, Substitute.For<IHttpContextAccessor>(), Substitute.For<ILogContainerService>());
	}

	/// <summary>
	/// The score, rank and activity histories are built in a single pass that carries running state between iterations,
	/// so the result depends on the leaderboard history files being processed chronologically. Directory.GetFiles gives
	/// no ordering guarantee -- NTFS happens to return entries sorted by name while ext4 does not -- so the repository
	/// must order them itself rather than inheriting an accident of the file system.
	/// </summary>
	[TestMethod]
	public void GetPlayerHistoryById_IsNotAffectedByFileEnumerationOrder()
	{
		string[] chronological = _data.TryGetFiles(DataSubDirectory.LeaderboardHistory);
		Assert.IsTrue(chronological.Length > 2, "At least three history files are needed for the scramble to be meaningful.");

		// A deterministic scramble: every odd index first, then every even one.
		string[] scrambled =
		[
			.. chronological.Where((_, i) => i % 2 == 1),
			.. chronological.Where((_, i) => i % 2 == 0),
		];
		CollectionAssert.AreNotEqual(chronological, scrambled, "The scrambled order must actually differ from the chronological one.");

		foreach (int playerId in new[] { 1, 2, 3, 4 })
		{
			PlayerHistory expected = CreateRepository(chronological).GetPlayerHistoryById(playerId);
			PlayerHistory actual = CreateRepository(scrambled).GetPlayerHistoryById(playerId);

			AssertDatesAreIncreasing(actual, playerId);

			Assert.AreEqual(expected.BestRank, actual.BestRank, $"BestRank differs for player {playerId}.");
			CollectionAssert.AreEqual(expected.ScoreHistory, actual.ScoreHistory, $"ScoreHistory differs for player {playerId}.");
			CollectionAssert.AreEqual(expected.RankHistory, actual.RankHistory, $"RankHistory differs for player {playerId}.");
			CollectionAssert.AreEqual(expected.ActivityHistory, actual.ActivityHistory, $"ActivityHistory differs for player {playerId}.");
			CollectionAssert.AreEqual(expected.Usernames, actual.Usernames, $"Usernames differ for player {playerId}.");
		}
	}

	private static void AssertDatesAreIncreasing(PlayerHistory history, int playerId)
	{
		for (int i = 1; i < history.ScoreHistory.Count; i++)
			Assert.IsTrue(history.ScoreHistory[i].DateTime > history.ScoreHistory[i - 1].DateTime, $"ScoreHistory is not in chronological order for player {playerId}.");

		for (int i = 1; i < history.RankHistory.Count; i++)
			Assert.IsTrue(history.RankHistory[i].DateTime > history.RankHistory[i - 1].DateTime, $"RankHistory is not in chronological order for player {playerId}.");

		for (int i = 1; i < history.ActivityHistory.Count; i++)
			Assert.IsTrue(history.ActivityHistory[i].DateTime > history.ActivityHistory[i - 1].DateTime, $"ActivityHistory is not in chronological order for player {playerId}.");
	}

	private PlayerHistoryRepository CreateRepository(string[] leaderboardHistoryPaths)
	{
		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Returns(leaderboardHistoryPaths);
		return new PlayerHistoryRepository(_dbContext, fileSystemService, _data);
	}
}
