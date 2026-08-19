namespace DevilDaggersInfo.Web.Core.Utils;

public static class EnumerableExtensions
{
	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> query, Func<T, TKey> keySelector, bool isAscending)
	{
		return isAscending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
	}
}
