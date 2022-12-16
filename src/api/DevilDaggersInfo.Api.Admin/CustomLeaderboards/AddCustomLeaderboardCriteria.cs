using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required string? Expression { get; init; }
}
