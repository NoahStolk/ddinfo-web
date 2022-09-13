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
		CustomLeaderboardCriteriaOperator.Equal => "#4f4",
		CustomLeaderboardCriteriaOperator.Modulo => "#80f",
		CustomLeaderboardCriteriaOperator.NotEqual => "#f44",
		_ => throw new InvalidEnumConversionException(criteriaOperator),
	};
}
