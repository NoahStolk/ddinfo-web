using DevilDaggersInfo.Core.CriteriaExpression;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class CustomLeaderboardConverters
{
	public static Entities.Enums.CustomLeaderboardRankSorting ToDomain(this AdminApi.CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting switch
		{
			AdminApi.CustomLeaderboardRankSorting.TimeDesc => Entities.Enums.CustomLeaderboardRankSorting.TimeDesc,
			AdminApi.CustomLeaderboardRankSorting.TimeAsc => Entities.Enums.CustomLeaderboardRankSorting.TimeAsc,
			AdminApi.CustomLeaderboardRankSorting.GemsCollectedDesc => Entities.Enums.CustomLeaderboardRankSorting.GemsCollectedDesc,
			AdminApi.CustomLeaderboardRankSorting.GemsCollectedAsc => Entities.Enums.CustomLeaderboardRankSorting.GemsCollectedAsc,
			AdminApi.CustomLeaderboardRankSorting.GemsDespawnedDesc => Entities.Enums.CustomLeaderboardRankSorting.GemsDespawnedDesc,
			AdminApi.CustomLeaderboardRankSorting.GemsDespawnedAsc => Entities.Enums.CustomLeaderboardRankSorting.GemsDespawnedAsc,
			AdminApi.CustomLeaderboardRankSorting.GemsEatenDesc => Entities.Enums.CustomLeaderboardRankSorting.GemsEatenDesc,
			AdminApi.CustomLeaderboardRankSorting.GemsEatenAsc => Entities.Enums.CustomLeaderboardRankSorting.GemsEatenAsc,
			AdminApi.CustomLeaderboardRankSorting.EnemiesKilledDesc => Entities.Enums.CustomLeaderboardRankSorting.EnemiesKilledDesc,
			AdminApi.CustomLeaderboardRankSorting.EnemiesKilledAsc => Entities.Enums.CustomLeaderboardRankSorting.EnemiesKilledAsc,
			AdminApi.CustomLeaderboardRankSorting.EnemiesAliveDesc => Entities.Enums.CustomLeaderboardRankSorting.EnemiesAliveDesc,
			AdminApi.CustomLeaderboardRankSorting.EnemiesAliveAsc => Entities.Enums.CustomLeaderboardRankSorting.EnemiesAliveAsc,
			AdminApi.CustomLeaderboardRankSorting.HomingStoredDesc => Entities.Enums.CustomLeaderboardRankSorting.HomingStoredDesc,
			AdminApi.CustomLeaderboardRankSorting.HomingStoredAsc => Entities.Enums.CustomLeaderboardRankSorting.HomingStoredAsc,
			AdminApi.CustomLeaderboardRankSorting.HomingEatenDesc => Entities.Enums.CustomLeaderboardRankSorting.HomingEatenDesc,
			AdminApi.CustomLeaderboardRankSorting.HomingEatenAsc => Entities.Enums.CustomLeaderboardRankSorting.HomingEatenAsc,
			_ => throw new UnreachableException(),
		};
	}

	public static CustomLeaderboardCriteriaOperator ToDomain(this AdminApi.CustomLeaderboardCriteriaOperator @operator)
	{
		return @operator switch
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
}
