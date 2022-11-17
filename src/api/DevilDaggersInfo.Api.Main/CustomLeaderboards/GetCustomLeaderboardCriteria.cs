using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaType Type { get; init; }

	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required byte[] Expression { get; init; }
}
