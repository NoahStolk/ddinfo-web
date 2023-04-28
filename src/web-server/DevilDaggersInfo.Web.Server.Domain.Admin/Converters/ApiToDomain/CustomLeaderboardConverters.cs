using DevilDaggersInfo.Core.CriteriaExpression;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class CustomLeaderboardConverters
{
	public static Entities.Enums.CustomLeaderboardCategory ToDomain(this AdminApi.CustomLeaderboardCategory category) => category switch
	{
		AdminApi.CustomLeaderboardCategory.Survival => Entities.Enums.CustomLeaderboardCategory.Survival,
		AdminApi.CustomLeaderboardCategory.TimeAttack => Entities.Enums.CustomLeaderboardCategory.TimeAttack,
		AdminApi.CustomLeaderboardCategory.Speedrun => Entities.Enums.CustomLeaderboardCategory.Speedrun,
		AdminApi.CustomLeaderboardCategory.Race => Entities.Enums.CustomLeaderboardCategory.Race,
		_ => throw new UnreachableException(),
	};

	public static CustomLeaderboardCriteriaOperator ToDomain(this AdminApi.CustomLeaderboardCriteriaOperator @operator) => @operator switch
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
}
