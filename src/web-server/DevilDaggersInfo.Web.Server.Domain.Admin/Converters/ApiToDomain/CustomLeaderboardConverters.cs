using DevilDaggersInfo.Core.CriteriaExpression;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class CustomLeaderboardConverters
{
	public static Entities.Enums.CustomLeaderboardRankSorting ToDomain(this AdminApi.CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		AdminApi.CustomLeaderboardRankSorting.TimeDesc => Entities.Enums.CustomLeaderboardRankSorting.TimeDesc,
		AdminApi.CustomLeaderboardRankSorting.TimeAsc => Entities.Enums.CustomLeaderboardRankSorting.TimeAsc,
		AdminApi.CustomLeaderboardRankSorting.GemsDesc => Entities.Enums.CustomLeaderboardRankSorting.GemsDesc,
		AdminApi.CustomLeaderboardRankSorting.KillsDesc => Entities.Enums.CustomLeaderboardRankSorting.KillsDesc,
		AdminApi.CustomLeaderboardRankSorting.HomingDesc => Entities.Enums.CustomLeaderboardRankSorting.HomingDesc,
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
