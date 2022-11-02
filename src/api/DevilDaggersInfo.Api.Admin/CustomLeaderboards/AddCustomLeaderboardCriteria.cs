using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public string? Expression { get; init; }
}
