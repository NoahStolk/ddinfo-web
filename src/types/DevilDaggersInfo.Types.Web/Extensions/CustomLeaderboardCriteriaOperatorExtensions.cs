namespace DevilDaggersInfo.Types.Web.Extensions;

public static class CustomLeaderboardCriteriaOperatorExtensions
{
	public static string Display(this CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		CustomLeaderboardCriteriaOperator.Equal => "equal to",
		CustomLeaderboardCriteriaOperator.LessThan => "less than",
		CustomLeaderboardCriteriaOperator.GreaterThan => "greater than",
		_ => string.Empty,
	};
}
