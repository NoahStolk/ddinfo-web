using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public int Value { get; init; }

	public string? Expression { get; init; }
}
