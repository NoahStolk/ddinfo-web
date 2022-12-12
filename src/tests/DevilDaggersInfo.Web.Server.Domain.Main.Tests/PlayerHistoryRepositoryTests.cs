using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Main.Tests.Utils;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Tests;

[TestClass]
public class PlayerHistoryRepositoryTests
{
	private readonly PlayerHistoryRepository _repository;

	public PlayerHistoryRepositoryTests()
	{
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: nameof(PlayerHistoryRepositoryTests))
			.Options;
		TestDbContext dbContext = new(options, new Mock<IHttpContextAccessor>().Object, new Mock<ILogContainerService>().Object);
		TestData data = new();
		_repository = new(dbContext, data, data);
	}

	[TestMethod]
	public void GetPlayerHistory_WithCheater()
	{
		GetPlayerHistory historyPlayer1 = _repository.GetPlayerHistoryById(1);

		// Verify that this player always has first place, even if a cheater has technically been first in the history at some point.
		Assert.AreEqual(3, historyPlayer1.ScoreHistory.Count);
		Assert.AreEqual(1, historyPlayer1.ScoreHistory[0].Rank);
		Assert.AreEqual(1, historyPlayer1.ScoreHistory[1].Rank);
		Assert.AreEqual(1, historyPlayer1.ScoreHistory[2].Rank);
		Assert.AreEqual(new(2022, 1, 1), historyPlayer1.ScoreHistory[0].DateTime);
		Assert.AreEqual(new(2022, 1, 3), historyPlayer1.ScoreHistory[1].DateTime);
		Assert.AreEqual(new(2022, 1, 4), historyPlayer1.ScoreHistory[2].DateTime);

		Assert.AreEqual(1, historyPlayer1.RankHistory.Count);
		Assert.AreEqual(1, historyPlayer1.RankHistory[0].Rank);
		Assert.AreEqual(new(2022, 1, 1), historyPlayer1.RankHistory[0].DateTime);

		Assert.AreEqual(1, historyPlayer1.BestRank);

		GetPlayerHistory historyPlayer2 = _repository.GetPlayerHistoryById(2);

		// Verify that this player always has second place, even if a cheater has technically been first in the history at some point.
		Assert.AreEqual(2, historyPlayer2.ScoreHistory.Count);
		Assert.AreEqual(2, historyPlayer2.ScoreHistory[0].Rank);
		Assert.AreEqual(2, historyPlayer2.ScoreHistory[1].Rank);
		Assert.AreEqual(new(2022, 1, 1), historyPlayer2.ScoreHistory[0].DateTime);
		Assert.AreEqual(new(2022, 1, 3), historyPlayer2.ScoreHistory[1].DateTime);

		Assert.AreEqual(1, historyPlayer2.RankHistory.Count);
		Assert.AreEqual(2, historyPlayer2.RankHistory[0].Rank);
		Assert.AreEqual(new(2022, 1, 1), historyPlayer2.RankHistory[0].DateTime);

		Assert.AreEqual(2, historyPlayer2.BestRank);

		GetPlayerHistory historyPlayer3 = _repository.GetPlayerHistoryById(3);

		// Verify that this player's best rank is 3rd, even if a cheater has always been above them.
		Assert.AreEqual(1, historyPlayer3.ScoreHistory.Count);
		Assert.AreEqual(3, historyPlayer3.ScoreHistory[0].Rank);
		Assert.AreEqual(new(2022, 1, 4), historyPlayer3.RankHistory[0].DateTime);

		Assert.AreEqual(1, historyPlayer3.RankHistory.Count);
		Assert.AreEqual(3, historyPlayer3.RankHistory[0].Rank);
		Assert.AreEqual(new(2022, 1, 4), historyPlayer3.RankHistory[0].DateTime);

		Assert.AreEqual(3, historyPlayer3.BestRank);

		GetPlayerHistory historyCheater = _repository.GetPlayerHistoryById(4);

		// A cheater's history is not affected, except if there is another cheater with a better rank (which we don't test here because we don't care about accurate cheater stats).
		Assert.AreEqual(3, historyCheater.ScoreHistory.Count);
		Assert.AreEqual(1, historyCheater.ScoreHistory[0].Rank);
		Assert.AreEqual(3, historyCheater.ScoreHistory[1].Rank);
		Assert.AreEqual(1, historyCheater.ScoreHistory[2].Rank);
		Assert.AreEqual(new(2022, 1, 2), historyCheater.ScoreHistory[0].DateTime);
		Assert.AreEqual(new(2022, 1, 3), historyCheater.ScoreHistory[1].DateTime);
		Assert.AreEqual(new(2022, 1, 4), historyCheater.ScoreHistory[2].DateTime);

		Assert.AreEqual(3, historyCheater.RankHistory.Count);
		Assert.AreEqual(1, historyCheater.RankHistory[0].Rank);
		Assert.AreEqual(3, historyCheater.RankHistory[1].Rank);
		Assert.AreEqual(1, historyCheater.RankHistory[2].Rank);
		Assert.AreEqual(new(2022, 1, 2), historyCheater.RankHistory[0].DateTime);
		Assert.AreEqual(new(2022, 1, 3), historyCheater.RankHistory[1].DateTime);
		Assert.AreEqual(new(2022, 1, 4), historyCheater.RankHistory[2].DateTime);

		Assert.AreEqual(1, historyCheater.BestRank);
	}
}
