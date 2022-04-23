namespace DevilDaggersInfo.Common.Extensions;

public static class IEnumerableExtensions
{
	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> query, Func<T, TKey> keySelector, bool isAscending)
		=> isAscending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
}
