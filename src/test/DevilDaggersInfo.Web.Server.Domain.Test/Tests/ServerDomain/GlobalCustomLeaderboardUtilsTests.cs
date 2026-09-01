using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class GlobalCustomLeaderboardUtilsTests
{
	[Test]
	public async Task GetPoints_IsZeroForAnEmptyEntry()
	{
		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(new GlobalCustomLeaderboardEntryData())).IsEqualTo(0);
	}

	[Test]
	public async Task GetPoints_AwardsTwoPerPlayerBeatenPlusOneself()
	{
		// Rank 1 of 10 beats 9 players, and the formula counts the player's own slot too: (10 - 0) * 2.
		GlobalCustomLeaderboardEntryData data = new();
		data.Rankings.Add(new CustomLeaderboardRanking { Rank = 1, TotalPlayers = 10 });

		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(data)).IsEqualTo(20);
	}

	[Test]
	public async Task GetPoints_AwardsLeastForLastPlace()
	{
		GlobalCustomLeaderboardEntryData data = new();
		data.Rankings.Add(new CustomLeaderboardRanking { Rank = 10, TotalPlayers = 10 });

		// (10 - 9) * 2.
		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(data)).IsEqualTo(2);
	}

	[Test]
	public async Task GetPoints_SumsEveryRanking()
	{
		GlobalCustomLeaderboardEntryData data = new();
		data.Rankings.Add(new CustomLeaderboardRanking { Rank = 1, TotalPlayers = 10 });
		data.Rankings.Add(new CustomLeaderboardRanking { Rank = 5, TotalPlayers = 20 });

		// (10 - 0) * 2 + (20 - 4) * 2.
		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(data)).IsEqualTo(52);
	}

	[Test]
	public async Task GetPoints_AddsTheDaggerBonuses()
	{
		GlobalCustomLeaderboardEntryData data = new()
		{
			LeviathanCount = 1,
			DevilCount = 1,
			GoldenCount = 1,
			SilverCount = 1,
			BronzeCount = 1,
			DefaultCount = 1,
		};

		// 10 + 6 + 4 + 2 + 1 + 0. A default dagger is worth nothing.
		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(data)).IsEqualTo(23);
	}

	[Test]
	public async Task GetPoints_DefaultDaggersDoNotAffectTheTotal()
	{
		GlobalCustomLeaderboardEntryData withDefaults = new() { GoldenCount = 2, DefaultCount = 50 };
		GlobalCustomLeaderboardEntryData without = new() { GoldenCount = 2 };

		await Assert.That(GlobalCustomLeaderboardUtils.GetPoints(withDefaults)).IsEqualTo(GlobalCustomLeaderboardUtils.GetPoints(without));
	}

	[Test]
	public async Task TotalPlayed_CountsEveryDagger()
	{
		GlobalCustomLeaderboardEntryData data = new()
		{
			LeviathanCount = 1,
			DevilCount = 2,
			GoldenCount = 3,
			SilverCount = 4,
			BronzeCount = 5,
			DefaultCount = 6,
		};

		await Assert.That(data.TotalPlayed).IsEqualTo(21);
	}
}
