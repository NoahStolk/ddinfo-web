using DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;

public static class SortableCustomEntryExtensions
{
	public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> customEntries, CustomLeaderboardCategory category)
		where T : ISortableCustomEntry
	{
		if (category.IsAscending())
			return customEntries.OrderBy(wr => wr.Time).ThenBy(wr => wr.SubmitDate);

		return customEntries.OrderByDescending(wr => wr.Time).ThenBy(wr => wr.SubmitDate);
	}
}
