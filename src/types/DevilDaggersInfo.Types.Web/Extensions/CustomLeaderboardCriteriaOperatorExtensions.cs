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
		_ => string.Empty,
	};
}
