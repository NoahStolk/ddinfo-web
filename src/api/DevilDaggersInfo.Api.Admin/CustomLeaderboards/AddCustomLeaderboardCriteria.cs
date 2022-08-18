using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public string? Expression { get; set; }
}
