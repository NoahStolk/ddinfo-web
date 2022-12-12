using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
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
		// Create an in-memory database
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: "DevilDaggersInfo")
			.Options;
		TestDbContext dbContext = new(options, new Mock<IHttpContextAccessor>().Object, new Mock<ILogContainerService>().Object);
		TestData data = new();
		_repository = new(dbContext, data, data);
	}

	[TestMethod]
	public void TestGetWorldRecordsData()
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

	private sealed class TestDbContext : ApplicationDbContext
	{
		public TestDbContext(DbContextOptions<TestDbContext> options, IHttpContextAccessor httpContextAccessor, ILogContainerService logContainerService)
			: base(options, httpContextAccessor, logContainerService)
		{
			List<PlayerEntity> players = new()
			{
				new() { Id = 1, BanType = BanType.NotBanned, PlayerName = "Player 1" },
				new() { Id = 2, BanType = BanType.NotBanned, PlayerName = "Player 2" },
				new() { Id = 3, BanType = BanType.NotBanned, PlayerName = "Player 3" },
				new() { Id = 4, BanType = BanType.Cheater, PlayerName = "Cheater" },
			};

			Players.AddRange(players);
			SaveChanges();
		}

		public override DbSet<PlayerEntity> Players => Set<PlayerEntity>();
	}

	private sealed class TestData : ILeaderboardHistoryCache, IFileSystemService
	{
		private readonly IReadOnlyDictionary<string, LeaderboardHistory> _data = new Dictionary<string, LeaderboardHistory>
		{
			["2022-01-01.bin"] = new()
			{
				DateTime = new(2022, 1, 1),
				Entries = new()
				{
					new() { Rank = 1, Id = 1, Time = 90 },
					new() { Rank = 2, Id = 2, Time = 80 },
				},
			},
			["2022-01-02.bin"] = new()
			{
				DateTime = new(2022, 1, 2),
				Entries = new()
				{
					new() { Rank = 1, Id = 4, Time = 100000 }, // Cheater makes it to first place.
					new() { Rank = 2, Id = 1, Time = 90 },
					new() { Rank = 3, Id = 2, Time = 80 },
				},
			},
			["2022-01-03.bin"] = new()
			{
				DateTime = new(2022, 1, 3),
				Entries = new()
				{
					new() { Rank = 1, Id = 1, Time = 95 },
					new() {	Rank = 2, Id = 2, Time = 85 },
					new() { Rank = 3, Id = 4, Time = 0 }, // Cheater is removed from the leaderboard.
				},
			},
			["2022-01-04.bin"] = new()
			{
				DateTime = new(2022, 1, 4),
				Entries = new()
				{
					new() { Rank = 1, Id = 4, Time = 1000000 }, // Cheater makes it to first place again.
					new() { Rank = 2, Id = 1, Time = 98 },
					new() { Rank = 3, Id = 2, Time = 85 },
					new() { Rank = 4, Id = 3, Time = 82 }, // Player 3 joins the leaderboard.
				},
			},
		};

		public string[] TryGetFiles(DataSubDirectory subDirectory)
		{
			if (subDirectory != DataSubDirectory.LeaderboardHistory)
				throw new NotImplementedException();

			return _data.Keys.ToArray();
		}

		public LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
		{
			return _data[filePath];
		}

 #pragma warning disable SA1201
		public string Root => throw new NotImplementedException();
		public string GetLeaderboardHistoryPathFromDate(DateTime dateTime) => throw new NotImplementedException();
		public string GetPath(DataSubDirectory subDirectory) => throw new NotImplementedException();
		public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version) => throw new NotImplementedException();
		public int GetCount() => throw new NotImplementedException();
		public void Clear() => throw new NotImplementedException();
 #pragma warning restore SA1201
	}
}
