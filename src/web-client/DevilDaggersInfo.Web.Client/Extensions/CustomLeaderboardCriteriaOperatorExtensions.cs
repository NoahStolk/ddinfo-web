using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCriteriaOperatorExtensions
{
	public static string GetColor(this CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		CustomLeaderboardCriteriaOperator.Any => "#fff",
		CustomLeaderboardCriteriaOperator.LessThan or CustomLeaderboardCriteriaOperator.LessThanOrEqual => "#ff0",
		CustomLeaderboardCriteriaOperator.GreaterThan or CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => "#0ff",
		CustomLeaderboardCriteriaOperator.Equal => "#f44",
		CustomLeaderboardCriteriaOperator.Modulo => "#80f",
		_ => throw new InvalidEnumConversionException(criteriaOperator),
	};
}