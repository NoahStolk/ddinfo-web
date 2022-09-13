using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Types.Web.Extensions;

public static class CustomLeaderboardCriteriaOperatorExtensions
{
	public static string Display(this CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		CustomLeaderboardCriteriaOperator.Equal => "equal to",
		CustomLeaderboardCriteriaOperator.LessThan => "less than",
		CustomLeaderboardCriteriaOperator.GreaterThan => "greater than",
		CustomLeaderboardCriteriaOperator.LessThanOrEqual => "less than or equal to",
		CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => "greater than or equal to",
		CustomLeaderboardCriteriaOperator.Modulo => "divisible by",
		CustomLeaderboardCriteriaOperator.NotEqual => "not equal to",
		CustomLeaderboardCriteriaOperator.Any => string.Empty,
		_ => throw new InvalidEnumConversionException(criteriaOperator),
	};

	public static string Description(this CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		CustomLeaderboardCriteriaOperator.NotEqual => "must not be equal to",
		_ => $"must be {criteriaOperator.Display()}",
	};
}
