using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

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
