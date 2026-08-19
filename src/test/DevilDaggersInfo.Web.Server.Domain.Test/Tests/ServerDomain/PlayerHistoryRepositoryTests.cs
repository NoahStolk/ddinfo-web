using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class PlayerHistoryRepositoryTests
{
	private readonly PlayerHistoryRepository _repository;

	public PlayerHistoryRepositoryTests()
	{
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: nameof(PlayerHistoryRepositoryTests))
			.Options;
		TestDbContext dbContext = new(options, Substitute.For<IHttpContextAccessor>(), Substitute.For<ILogContainerService>());
		TestData data = new();
		_repository = new PlayerHistoryRepository(dbContext, data, data);
	}

	private static DateTime CreateDateTime(int year, int month, int day)
	{
		return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
	}

	[Test]
	public async Task GetPlayerHistory_WithCheater()
	{
		PlayerHistory historyPlayer1 = _repository.GetPlayerHistoryById(1);

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

		PlayerHistory historyPlayer2 = _repository.GetPlayerHistoryById(2);

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

		PlayerHistory historyPlayer3 = _repository.GetPlayerHistoryById(3);

		// Verify that this player's best rank is 3rd, even if a cheater has always been above them.
		await Assert.That(historyPlayer3.ScoreHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer3.ScoreHistory[0].Rank).IsEqualTo(3);
		await Assert.That(historyPlayer3.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyPlayer3.RankHistory.Count).IsEqualTo(1);
		await Assert.That(historyPlayer3.RankHistory[0].Rank).IsEqualTo(3);
		await Assert.That(historyPlayer3.RankHistory[0].DateTime).IsEqualTo(CreateDateTime(2022, 1, 4));

		await Assert.That(historyPlayer3.BestRank).IsEqualTo(3);

		PlayerHistory historyCheater = _repository.GetPlayerHistoryById(4);

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
}
