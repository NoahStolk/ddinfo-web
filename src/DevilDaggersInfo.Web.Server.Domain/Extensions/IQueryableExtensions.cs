using System.Linq.Expressions;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class IQueryableExtensions
{
	public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool isAscending)
	{
		return isAscending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
	}
}
