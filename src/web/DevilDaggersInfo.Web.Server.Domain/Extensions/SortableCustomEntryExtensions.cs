using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class SortableCustomEntryExtensions
{
	public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> customEntries, CustomLeaderboardRankSorting rankSorting)
		where T : ISortableCustomEntry
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc => customEntries.OrderBy(ce => ce.Time).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsCollectedAsc => customEntries.OrderBy(ce => ce.GemsCollected).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsDespawnedAsc => customEntries.OrderBy(ce => ce.GemsDespawned).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsEatenAsc => customEntries.OrderBy(ce => ce.GemsEaten).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.EnemiesKilledAsc => customEntries.OrderBy(ce => ce.EnemiesKilled).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.EnemiesAliveAsc => customEntries.OrderBy(ce => ce.EnemiesAlive).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.HomingStoredAsc => customEntries.OrderBy(ce => ce.HomingStored).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.HomingEatenAsc => customEntries.OrderBy(ce => ce.HomingEaten).ThenBy(ce => ce.SubmitDate),

			CustomLeaderboardRankSorting.TimeDesc => customEntries.OrderByDescending(ce => ce.Time).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsCollectedDesc => customEntries.OrderByDescending(ce => ce.GemsCollected).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsDespawnedDesc => customEntries.OrderByDescending(ce => ce.GemsDespawned).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsEatenDesc => customEntries.OrderByDescending(ce => ce.GemsEaten).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.EnemiesKilledDesc => customEntries.OrderByDescending(ce => ce.EnemiesKilled).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.EnemiesAliveDesc => customEntries.OrderByDescending(ce => ce.EnemiesAlive).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.HomingStoredDesc => customEntries.OrderByDescending(ce => ce.HomingStored).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.HomingEatenDesc => customEntries.OrderByDescending(ce => ce.HomingEaten).ThenBy(ce => ce.SubmitDate),

			_ => throw new InvalidOperationException($"Rank sorting '{rankSorting}' not supported."),
		};
	}
}
