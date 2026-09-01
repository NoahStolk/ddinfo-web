using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class CustomLeaderboardUtilsTests
{
	// The thresholds used throughout, in the order the production code checks them.
	private const int _leviathan = 1000;
	private const int _devil = 800;
	private const int _golden = 600;
	private const int _silver = 400;
	private const int _bronze = 200;

	private sealed record StatEntry : IDaggerStatCustomEntry
	{
		public int Time { get; init; }

		public int GemsCollected { get; init; }

		public int GemsDespawned { get; init; }

		public int GemsEaten { get; init; }

		public int EnemiesKilled { get; init; }

		public int EnemiesAlive { get; init; }

		public int HomingStored { get; init; }

		public int HomingEaten { get; init; }
	}

	[Test]
	[Arguments(1000, CustomLeaderboardDagger.Leviathan)]
	[Arguments(1200, CustomLeaderboardDagger.Leviathan)]
	[Arguments(999, CustomLeaderboardDagger.Devil)]
	[Arguments(800, CustomLeaderboardDagger.Devil)]
	[Arguments(799, CustomLeaderboardDagger.Golden)]
	[Arguments(600, CustomLeaderboardDagger.Golden)]
	[Arguments(599, CustomLeaderboardDagger.Silver)]
	[Arguments(400, CustomLeaderboardDagger.Silver)]
	[Arguments(399, CustomLeaderboardDagger.Bronze)]
	[Arguments(200, CustomLeaderboardDagger.Bronze)]
	[Arguments(199, CustomLeaderboardDagger.Default)]
	[Arguments(0, CustomLeaderboardDagger.Default)]
	public async Task GetDaggerFromStat_Descending_AwardsTheHighestThresholdReached(int time, CustomLeaderboardDagger expected)
	{
		// TimeDesc: a higher time is better, so each threshold is a lower bound and the boundary itself qualifies.
		CustomLeaderboardDagger actual = CustomLeaderboardUtils.GetDaggerFromStat(
			CustomLeaderboardRankSorting.TimeDesc, new StatEntry { Time = time }, _leviathan, _devil, _golden, _silver, _bronze);

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	[Arguments(1000, CustomLeaderboardDagger.Leviathan)]
	[Arguments(800, CustomLeaderboardDagger.Leviathan)]
	[Arguments(0, CustomLeaderboardDagger.Leviathan)]
	[Arguments(1001, CustomLeaderboardDagger.Default)]
	public async Task GetDaggerFromStat_Ascending_TreatsThresholdsAsUpperBounds(int time, CustomLeaderboardDagger expected)
	{
		// TimeAsc: a lower time is better. Anything at or under the leviathan threshold is a leviathan, and only a
		// value above every threshold falls through to Default.
		CustomLeaderboardDagger actual = CustomLeaderboardUtils.GetDaggerFromStat(
			CustomLeaderboardRankSorting.TimeAsc, new StatEntry { Time = time }, _leviathan, _devil, _golden, _silver, _bronze);

		await Assert.That(actual).IsEqualTo(expected);
	}

	[Test]
	public async Task GetDaggerFromStat_ReadsTheStatNamedByTheRankSorting()
	{
		// Only GemsCollected is above the leviathan threshold, so any sorting that reads a different stat must not
		// award a leviathan. This pins the switch that maps sorting to stat.
		StatEntry entry = new()
		{
			Time = 0,
			GemsCollected = 1000,
			GemsDespawned = 0,
			GemsEaten = 0,
			EnemiesKilled = 0,
			EnemiesAlive = 0,
			HomingStored = 0,
			HomingEaten = 0,
		};

		CustomLeaderboardDagger gems = CustomLeaderboardUtils.GetDaggerFromStat(
			CustomLeaderboardRankSorting.GemsCollectedDesc, entry, _leviathan, _devil, _golden, _silver, _bronze);
		CustomLeaderboardDagger kills = CustomLeaderboardUtils.GetDaggerFromStat(
			CustomLeaderboardRankSorting.EnemiesKilledDesc, entry, _leviathan, _devil, _golden, _silver, _bronze);

		await Assert.That(gems).IsEqualTo(CustomLeaderboardDagger.Leviathan);
		await Assert.That(kills).IsEqualTo(CustomLeaderboardDagger.Default);
	}

	[Test]
	[Arguments(CustomLeaderboardRankSorting.GemsDespawnedDesc)]
	[Arguments(CustomLeaderboardRankSorting.GemsEatenDesc)]
	[Arguments(CustomLeaderboardRankSorting.EnemiesAliveDesc)]
	[Arguments(CustomLeaderboardRankSorting.HomingStoredDesc)]
	[Arguments(CustomLeaderboardRankSorting.HomingEatenDesc)]
	public async Task GetDaggerFromStat_SupportsEveryRankSorting(CustomLeaderboardRankSorting rankSorting)
	{
		// Every enum member must map to a stat; the production switch throws on anything it does not handle.
		CustomLeaderboardDagger actual = CustomLeaderboardUtils.GetDaggerFromStat(
			rankSorting, new StatEntry(), _leviathan, _devil, _golden, _silver, _bronze);

		await Assert.That(actual).IsEqualTo(CustomLeaderboardDagger.Default);
	}

	[Test]
	public async Task IsGameModeAndRankSortingCombinationAllowed_SurvivalAllowsEverything()
	{
		foreach (CustomLeaderboardRankSorting rankSorting in Enum.GetValues<CustomLeaderboardRankSorting>())
			await Assert.That(CustomLeaderboardUtils.IsGameModeAndRankSortingCombinationAllowed(SpawnsetGameMode.Survival, rankSorting)).IsTrue();
	}

	[Test]
	[Arguments(CustomLeaderboardRankSorting.TimeAsc, true)]
	[Arguments(CustomLeaderboardRankSorting.TimeDesc, true)]
	[Arguments(CustomLeaderboardRankSorting.GemsCollectedAsc, false)]
	[Arguments(CustomLeaderboardRankSorting.EnemiesKilledDesc, false)]
	public async Task IsGameModeAndRankSortingCombinationAllowed_RaceAllowsOnlyTime(CustomLeaderboardRankSorting rankSorting, bool expected)
	{
		await Assert.That(CustomLeaderboardUtils.IsGameModeAndRankSortingCombinationAllowed(SpawnsetGameMode.Race, rankSorting)).IsEqualTo(expected);
	}

	[Test]
	[Arguments(CustomLeaderboardRankSorting.TimeAsc, true)]
	[Arguments(CustomLeaderboardRankSorting.TimeDesc, false)]
	[Arguments(CustomLeaderboardRankSorting.GemsCollectedAsc, false)]
	public async Task IsGameModeAndRankSortingCombinationAllowed_TimeAttackAllowsOnlyAscendingTime(CustomLeaderboardRankSorting rankSorting, bool expected)
	{
		await Assert.That(CustomLeaderboardUtils.IsGameModeAndRankSortingCombinationAllowed(SpawnsetGameMode.TimeAttack, rankSorting)).IsEqualTo(expected);
	}

	[Test]
	public async Task GetAllowedGameModeAndRankSortingCombinations_MatchesThePredicate()
	{
		List<(SpawnsetGameMode GameMode, CustomLeaderboardRankSorting RankSorting)> combinations = CustomLeaderboardUtils.GetAllowedGameModeAndRankSortingCombinations();

		// 16 sortings for Survival, 2 for Race, 1 for TimeAttack.
		await Assert.That(combinations.Count).IsEqualTo(19);
		await Assert.That(combinations.All(c => CustomLeaderboardUtils.IsGameModeAndRankSortingCombinationAllowed(c.GameMode, c.RankSorting))).IsTrue();
		await Assert.That(combinations.Distinct().Count()).IsEqualTo(combinations.Count);
	}
}
