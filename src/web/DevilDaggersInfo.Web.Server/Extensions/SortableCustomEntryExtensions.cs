using DevilDaggersInfo.Web.Server.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class SortableCustomEntryExtensions
{
	public static IOrderedEnumerable<CustomEntryEntity> Sort(this IEnumerable<CustomEntryEntity> customEntries, CustomLeaderboardCategory category)
	{
		if (category.IsAscending())
			return customEntries.OrderBy(wr => wr.Time).ThenBy(wr => wr.SubmitDate);

		return customEntries.OrderByDescending(wr => wr.Time).ThenBy(wr => wr.SubmitDate);
	}
}
