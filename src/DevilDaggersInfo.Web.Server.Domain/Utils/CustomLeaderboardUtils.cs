using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class CustomLeaderboardUtils
{
	// TODO: Add unit tests.
	public static CustomLeaderboardDagger GetDaggerFromStat(CustomLeaderboardRankSorting rankSorting, IDaggerStatCustomEntry entry, int leviathan, int devil, int golden, int silver, int bronze)
	{
		int stat = rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => entry.Time,
			CustomLeaderboardRankSorting.GemsCollectedAsc or CustomLeaderboardRankSorting.GemsCollectedDesc => entry.GemsCollected,
			CustomLeaderboardRankSorting.GemsDespawnedAsc or CustomLeaderboardRankSorting.GemsDespawnedDesc => entry.GemsDespawned,
			CustomLeaderboardRankSorting.GemsEatenAsc or CustomLeaderboardRankSorting.GemsEatenDesc => entry.GemsEaten,
			CustomLeaderboardRankSorting.EnemiesKilledAsc or CustomLeaderboardRankSorting.EnemiesKilledDesc => entry.EnemiesKilled,
			CustomLeaderboardRankSorting.EnemiesAliveAsc or CustomLeaderboardRankSorting.EnemiesAliveDesc => entry.EnemiesAlive,
			CustomLeaderboardRankSorting.HomingStoredAsc or CustomLeaderboardRankSorting.HomingStoredDesc => entry.HomingStored,
			CustomLeaderboardRankSorting.HomingEatenAsc or CustomLeaderboardRankSorting.HomingEatenDesc => entry.HomingEaten,
			_ => throw new InvalidOperationException("Unsupported rank sorting for dagger calculation."),
		};

		if (rankSorting.IsAscending())
		{
			if (stat <= leviathan)
				return CustomLeaderboardDagger.Leviathan;
			if (stat <= devil)
				return CustomLeaderboardDagger.Devil;
			if (stat <= golden)
				return CustomLeaderboardDagger.Golden;
			if (stat <= silver)
				return CustomLeaderboardDagger.Silver;
			if (stat <= bronze)
				return CustomLeaderboardDagger.Bronze;

			return CustomLeaderboardDagger.Default;
		}

		if (stat >= leviathan)
			return CustomLeaderboardDagger.Leviathan;
		if (stat >= devil)
			return CustomLeaderboardDagger.Devil;
		if (stat >= golden)
			return CustomLeaderboardDagger.Golden;
		if (stat >= silver)
			return CustomLeaderboardDagger.Silver;
		if (stat >= bronze)
			return CustomLeaderboardDagger.Bronze;

		return CustomLeaderboardDagger.Default;
	}

	public static bool IsGameModeAndRankSortingCombinationAllowed(GameMode gameMode, CustomLeaderboardRankSorting rankSorting)
	{
		// Allow all rank sortings for Survival.
		if (gameMode == GameMode.Survival)
			return true;

		// Allow both time rank sortings for Race.
		if (gameMode == GameMode.Race)
			return rankSorting is CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc;

		// Only allow ascending time sort for TimeAttack for now.
		return rankSorting == CustomLeaderboardRankSorting.TimeAsc;
	}

	public static List<(GameMode GameMode, CustomLeaderboardRankSorting RankSorting)> GetAllowedGameModeAndRankSortingCombinations()
	{
		List<(GameMode GameMode, CustomLeaderboardRankSorting RankSorting)> allowedCombinations = [];
		foreach (GameMode gameMode in Enum.GetValues<GameMode>())
		{
			foreach (CustomLeaderboardRankSorting rankSorting in Enum.GetValues<CustomLeaderboardRankSorting>().Where(rs => IsGameModeAndRankSortingCombinationAllowed(gameMode, rs)))
				allowedCombinations.Add((gameMode, rankSorting));
		}

		return allowedCombinations;
	}
}
