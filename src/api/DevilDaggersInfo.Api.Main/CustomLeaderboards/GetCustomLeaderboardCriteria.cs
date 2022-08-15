using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaType Type { get; init; }

	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public int Value { get; init; }
}
