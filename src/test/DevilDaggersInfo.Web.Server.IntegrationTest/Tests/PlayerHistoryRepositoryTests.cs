using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using ClubberApi = DevilDaggersInfo.Web.ApiSpec.Clubber.Players;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Players;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

[NotInParallel(nameof(PlayerHistoryRepositoryTests))]
internal sealed class PlayerHistoryRepositoryTests : ApplicationTest
{
	private static DateTime CreateDateTime(int year, int month, int day)
	{
		return LeaderboardHistoryFixtures.Date(year, month, day);
	}

	[Test]
	public async Task GetPlayerHistory_WithCheater()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		await using AsyncServiceScope scope = App.CreateScope();
		PlayerHistoryRepository repository = scope.ServiceProvider.GetRequiredService<PlayerHistoryRepository>();

		PlayerHistory historyPlayer1 = await repository.GetPlayerHistoryByIdAsync(1);

		// Verify that this player always has first place, even if a cheater has technically been first in the history at some point.
		await Assert.That(historyPlayer1.ScoreHistory.Count).IsEqualTo(3);
		await Assert.That(historyPlayer1.ScoreHistory[0].Rank).IsEqualTo(1);
		await Assert.That(historyPlayer1.ScoreHistory[1].Rank).IsEqualTo(1);
		await Assert.That(historyPlayer1.ScoreHistory[2].Rank).IsEqualTo(1);
		await Assert.That(historyPlayer1.ScoreHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 1));
		await Assert.That(historyPlayer1.ScoreHistory[1].DateTime).IsEqualTo(CreateDateTime(2022, 1, 3));
		await Assert.That(historyPlayer1.ScoreHistory[2].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyPlayer1.RankHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer1.RankHistory[0].Rank).IsEqualTo(1);
		await Assert.That(historyPlayer1.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 1));

		await Assert.That(historyPlayer1.BestRank).IsEqualTo(1);

		PlayerHistory historyPlayer2 = await repository.GetPlayerHistoryByIdAsync(2);

		// Verify that this player always has second place, even if a cheater has technically been first in the history at some point.
		await Assert.That(historyPlayer2.ScoreHistory.Count).IsEqualTo(2);
		await Assert.That(historyPlayer2.ScoreHistory[0].Rank).IsEqualTo(2);
		await Assert.That(historyPlayer2.ScoreHistory[1].Rank).IsEqualTo(2);
		await Assert.That(historyPlayer2.ScoreHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 1));
		await Assert.That(historyPlayer2.ScoreHistory[1].DateTime).IsEqualTo(CreateDateTime(2022, 1, 3));

		await Assert.That(historyPlayer2.RankHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer2.RankHistory[0].Rank).IsEqualTo(2);
		await Assert.That(historyPlayer2.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 1));

		await Assert.That(historyPlayer2.BestRank).IsEqualTo(2);

		PlayerHistory historyPlayer3 = await repository.GetPlayerHistoryByIdAsync(3);

		// Verify that this player's best rank is 3rd, even if a cheater has always been above them.
		await Assert.That(historyPlayer3.ScoreHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer3.ScoreHistory[0].Rank).IsEqualTo(3);
		await Assert.That(historyPlayer3.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyPlayer3.RankHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer3.RankHistory[0].Rank).IsEqualTo(3);
		await Assert.That(historyPlayer3.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyPlayer3.BestRank).IsEqualTo(3);

		PlayerHistory historyCheater = await repository.GetPlayerHistoryByIdAsync(LeaderboardHistoryFixtures.CheaterId);

		// A cheater's history is not affected, except if there is another cheater with a better rank (which we don't test here because we don't care about accurate cheater stats).
		await Assert.That(historyCheater.ScoreHistory.Count).IsEqualTo(3);
		await Assert.That(historyCheater.ScoreHistory[0].Rank).IsEqualTo(1);
		await Assert.That(historyCheater.ScoreHistory[1].Rank).IsEqualTo(3);
		await Assert.That(historyCheater.ScoreHistory[2].Rank).IsEqualTo(1);
		await Assert.That(historyCheater.ScoreHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 2));
		await Assert.That(historyCheater.ScoreHistory[1].DateTime).IsEqualTo(CreateDateTime(2022, 1, 3));
		await Assert.That(historyCheater.ScoreHistory[2].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyCheater.RankHistory.Count).IsEqualTo(3);
		await Assert.That(historyCheater.RankHistory[0].Rank).IsEqualTo(1);
		await Assert.That(historyCheater.RankHistory[1].Rank).IsEqualTo(3);
		await Assert.That(historyCheater.RankHistory[2].Rank).IsEqualTo(1);
		await Assert.That(historyCheater.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 2));
		await Assert.That(historyCheater.RankHistory[1].DateTime).IsEqualTo(CreateDateTime(2022, 1, 3));
		await Assert.That(historyCheater.RankHistory[2].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyCheater.BestRank).IsEqualTo(1);
	}

	[Test]
	[Arguments(1, 1, new[] { 1, 1, 1 })]
	[Arguments(3, 3, new[] { 3 })]
	[Arguments(LeaderboardHistoryFixtures.CheaterId, 1, new[] { 1, 3, 1 })]
	public async Task MainEndpoint_ServesHistoryWithCheaterExcludedFromOtherRanks(int playerId, int expectedBestRank, int[] expectedScoreRanks)
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/players/{playerId}/history");
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		MainApi.GetPlayerHistory? history = await response.Content.ReadFromJsonAsync<MainApi.GetPlayerHistory>();
		ArgumentNullException.ThrowIfNull(history);
		await Assert.That(history.BestRank).IsEqualTo(expectedBestRank);
		await Assert.That(history.ScoreHistory.Select(e => e.Rank)).IsEquivalentTo(expectedScoreRanks, CollectionOrdering.Matching);
	}

	[Test]
	[Arguments(1, 1, new[] { 1, 1, 1 })]
	[Arguments(3, 3, new[] { 3 })]
	[Arguments(LeaderboardHistoryFixtures.CheaterId, 1, new[] { 1, 3, 1 })]
	public async Task ClubberEndpoint_ServesHistoryWithCheaterExcludedFromOtherRanks(int playerId, int expectedBestRank, int[] expectedScoreRanks)
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/clubber/players/{playerId}/history");
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		ClubberApi.GetPlayerHistory? history = await response.Content.ReadFromJsonAsync<ClubberApi.GetPlayerHistory>();
		ArgumentNullException.ThrowIfNull(history);
		await Assert.That(history.BestRank).IsEqualTo(expectedBestRank);
		await Assert.That(history.ScoreHistory.Select(e => e.Rank)).IsEquivalentTo(expectedScoreRanks, CollectionOrdering.Matching);
	}
}
