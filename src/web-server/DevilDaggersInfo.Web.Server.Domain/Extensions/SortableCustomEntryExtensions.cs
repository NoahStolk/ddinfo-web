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
			CustomLeaderboardRankSorting.TimeDesc => customEntries.OrderByDescending(ce => ce.Time).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.GemsDesc => customEntries.OrderByDescending(ce => ce.GemsCollected).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.KillsDesc => customEntries.OrderByDescending(ce => ce.EnemiesKilled).ThenBy(ce => ce.SubmitDate),
			CustomLeaderboardRankSorting.HomingDesc => customEntries.OrderByDescending(ce => ce.HomingStored).ThenBy(ce => ce.SubmitDate),
			_ => throw new InvalidOperationException($"Rank sorting '{rankSorting}' not supported."),
		};
	}
}
