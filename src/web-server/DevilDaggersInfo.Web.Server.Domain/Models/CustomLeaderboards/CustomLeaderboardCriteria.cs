using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaType Type { get; init; }

	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	public int Value { get; init; }

	public bool IsDefault() => Operator == CustomLeaderboardCriteriaOperator.Any && Value == 0;
}
