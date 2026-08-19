using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

// This lives in its own class because TestDbContext seeds in its constructor, and TUnit constructs the test class once
// per test method, which would seed the same in-memory database twice.
internal sealed class PlayerHistoryRepositoryOrderingTests : IDisposable
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
	[Test]
	public async Task GetPlayerHistoryById_IsNotAffectedByFileEnumerationOrder()
	{
		string[] chronological = _data.TryGetFiles(DataSubDirectory.LeaderboardHistory);
		await Assert.That(chronological.Length).IsGreaterThan(2).Because("At least three history files are needed for the scramble to be meaningful.");

		// A deterministic scramble: every odd index first, then every even one.
		string[] scrambled =
		[
			.. chronological.Where((_, i) => i % 2 == 1),
			.. chronological.Where((_, i) => i % 2 == 0),
		];
		await Assert.That(scrambled).IsNotEquivalentTo(chronological, CollectionOrdering.Matching).Because("The scrambled order must actually differ from the chronological one.");

		foreach (int playerId in new[] { 1, 2, 3, 4 })
		{
			PlayerHistory expected = CreateRepository(chronological).GetPlayerHistoryById(playerId);
			PlayerHistory actual = CreateRepository(scrambled).GetPlayerHistoryById(playerId);

			await AssertDatesAreIncreasingAsync(actual, playerId);

			await Assert.That(actual.BestRank).IsEqualTo(expected.BestRank).Because($"BestRank differs for player {playerId}.");
			await Assert.That(actual.ScoreHistory).IsEquivalentTo(expected.ScoreHistory, CollectionOrdering.Matching).Because($"ScoreHistory differs for player {playerId}.");
			await Assert.That(actual.RankHistory).IsEquivalentTo(expected.RankHistory, CollectionOrdering.Matching).Because($"RankHistory differs for player {playerId}.");
			await Assert.That(actual.ActivityHistory).IsEquivalentTo(expected.ActivityHistory, CollectionOrdering.Matching).Because($"ActivityHistory differs for player {playerId}.");
			await Assert.That(actual.Usernames).IsEquivalentTo(expected.Usernames, CollectionOrdering.Matching).Because($"Usernames differ for player {playerId}.");
		}
	}

	[AssertionMethod]
	private static async Task AssertDatesAreIncreasingAsync(PlayerHistory history, int playerId)
	{
		for (int i = 1; i < history.ScoreHistory.Count; i++)
			await Assert.That(history.ScoreHistory[i].DateTime).IsGreaterThan(history.ScoreHistory[i - 1].DateTime).Because($"ScoreHistory is not in chronological order for player {playerId}.");

		for (int i = 1; i < history.RankHistory.Count; i++)
			await Assert.That(history.RankHistory[i].DateTime).IsGreaterThan(history.RankHistory[i - 1].DateTime).Because($"RankHistory is not in chronological order for player {playerId}.");

		for (int i = 1; i < history.ActivityHistory.Count; i++)
			await Assert.That(history.ActivityHistory[i].DateTime).IsGreaterThan(history.ActivityHistory[i - 1].DateTime).Because($"ActivityHistory is not in chronological order for player {playerId}.");
	}

	public void Dispose()
	{
		_dbContext.Dispose();
	}

	private PlayerHistoryRepository CreateRepository(string[] leaderboardHistoryPaths)
	{
		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Returns(leaderboardHistoryPaths);
		return new PlayerHistoryRepository(_dbContext, fileSystemService, _data);
	}
}
