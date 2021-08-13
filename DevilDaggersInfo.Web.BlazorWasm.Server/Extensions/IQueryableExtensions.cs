using System;
using System.Linq;
using System.Linq.Expressions;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Extensions
{
	public static class IQueryableExtensions
	{
		public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool isAscending)
			=> isAscending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

		public static IQueryable<T> OrderByMember<T>(this IQueryable<T> query, string memberName, bool ascending)
			=> query.ByMember(memberName, ascending, "OrderBy", "OrderByDescending");

		public static IQueryable<T> ThenByMember<T>(this IQueryable<T> query, string memberName, bool ascending)
			=> query.ByMember(memberName, ascending, "ThenBy", "ThenByDescending");

		private static IQueryable<T> ByMember<T>(this IQueryable<T> query, string memberName, bool ascending, string methodNameAsc, string methodNameDesc)
		{
			ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
			MemberExpression property = Expression.Property(parameter, memberName);
			LambdaExpression expression = Expression.Lambda(property, parameter);
			Type[] types = new[] { query.ElementType, expression.Body.Type };

			string method = ascending ? methodNameAsc : methodNameDesc;
			MethodCallExpression mce = Expression.Call(typeof(Queryable), method, types, query.Expression, expression);

			return query.Provider.CreateQuery<T>(mce);
		}
	}
}
