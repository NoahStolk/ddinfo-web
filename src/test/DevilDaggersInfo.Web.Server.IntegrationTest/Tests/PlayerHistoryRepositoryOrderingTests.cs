using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

[NotInParallel(nameof(PlayerHistoryRepositoryOrderingTests))]
internal sealed class PlayerHistoryRepositoryOrderingTests : ApplicationTest
{
	/// <summary>
	/// The score, rank and activity histories are built in a single pass that carries running state between iterations,
	/// so the result depends on the leaderboard history files being processed chronologically. The repository must order
	/// them by date itself rather than relying on the order in which the file system lists them. LocalFileSystem always
	/// lists in name order, so only the listing is substituted; the database and the history cache are real.
	/// </summary>
	[Test]
	public async Task GetPlayerHistoryById_IsNotAffectedByFileEnumerationOrder()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		await using AsyncServiceScope scope = App.CreateScope();
		IFileSystem realFileSystem = scope.ServiceProvider.GetRequiredService<IFileSystem>();

		IReadOnlyList<FileEntry> chronological = await realFileSystem.ListAsync(DataSubDirectory.LeaderboardHistory);
		await Assert.That(chronological.Count).IsGreaterThan(2).Because("At least three history files are needed for the scramble to be meaningful.");

		// A deterministic scramble: every odd index first, then every even one.
		FileEntry[] scrambled =
		[
			.. chronological.Where((_, i) => i % 2 == 1),
			.. chronological.Where((_, i) => i % 2 == 0),
		];
		await Assert.That(scrambled).IsNotEquivalentTo(chronological, CollectionOrdering.Matching).Because("The scrambled order must actually differ from the chronological one.");

		foreach (int playerId in new[] { 1, 2, 3, LeaderboardHistoryFixtures.CheaterId })
		{
			PlayerHistory expected = await CreateRepository(scope, chronological).GetPlayerHistoryByIdAsync(playerId);
			PlayerHistory actual = await CreateRepository(scope, scrambled).GetPlayerHistoryByIdAsync(playerId);

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

	private static PlayerHistoryRepository CreateRepository(AsyncServiceScope scope, IReadOnlyList<FileEntry> leaderboardHistoryFiles)
	{
		IFileSystem fileSystem = Substitute.For<IFileSystem>();
		fileSystem.ListAsync(DataSubDirectory.LeaderboardHistory).Returns(leaderboardHistoryFiles);

		return new PlayerHistoryRepository(
			scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(),
			fileSystem,
			scope.ServiceProvider.GetRequiredService<ILeaderboardHistoryCache>());
	}
}
