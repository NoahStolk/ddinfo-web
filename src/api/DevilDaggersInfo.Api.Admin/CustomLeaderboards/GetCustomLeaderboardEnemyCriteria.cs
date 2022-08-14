using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboardEnemyCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public short Value { get; init; }
}
