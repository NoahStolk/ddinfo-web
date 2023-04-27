using DevilDaggersInfo.Core.CriteriaExpression;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCriteriaOperatorExtensions
{
	public static CustomLeaderboardCriteriaOperator ToCore(this AdminApi.CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		AdminApi.CustomLeaderboardCriteriaOperator.Any => CustomLeaderboardCriteriaOperator.Any,
		AdminApi.CustomLeaderboardCriteriaOperator.Equal => CustomLeaderboardCriteriaOperator.Equal,
		AdminApi.CustomLeaderboardCriteriaOperator.LessThan => CustomLeaderboardCriteriaOperator.LessThan,
		AdminApi.CustomLeaderboardCriteriaOperator.GreaterThan => CustomLeaderboardCriteriaOperator.GreaterThan,
		AdminApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual => CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		AdminApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		AdminApi.CustomLeaderboardCriteriaOperator.Modulo => CustomLeaderboardCriteriaOperator.Modulo,
		AdminApi.CustomLeaderboardCriteriaOperator.NotEqual => CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	public static CustomLeaderboardCriteriaOperator ToCore(this MainApi.CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		MainApi.CustomLeaderboardCriteriaOperator.Any => CustomLeaderboardCriteriaOperator.Any,
		MainApi.CustomLeaderboardCriteriaOperator.Equal => CustomLeaderboardCriteriaOperator.Equal,
		MainApi.CustomLeaderboardCriteriaOperator.LessThan => CustomLeaderboardCriteriaOperator.LessThan,
		MainApi.CustomLeaderboardCriteriaOperator.GreaterThan => CustomLeaderboardCriteriaOperator.GreaterThan,
		MainApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual => CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		MainApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		MainApi.CustomLeaderboardCriteriaOperator.Modulo => CustomLeaderboardCriteriaOperator.Modulo,
		MainApi.CustomLeaderboardCriteriaOperator.NotEqual => CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	public static string GetColor(this CustomLeaderboardCriteriaOperator criteriaOperator) => criteriaOperator switch
	{
		CustomLeaderboardCriteriaOperator.Any => "#fff",
		CustomLeaderboardCriteriaOperator.LessThan or CustomLeaderboardCriteriaOperator.LessThanOrEqual => "#ff0",
		CustomLeaderboardCriteriaOperator.GreaterThan or CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => "#0ff",
		CustomLeaderboardCriteriaOperator.Equal => "#4f4",
		CustomLeaderboardCriteriaOperator.Modulo => "#80f",
		CustomLeaderboardCriteriaOperator.NotEqual => "#f44",
		_ => throw new UnreachableException(),
	};
}
