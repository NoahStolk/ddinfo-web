using DevilDaggersInfo.Core.CriteriaExpression;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public sealed record CustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaType Type { get; init; }

	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required byte[] Expression { get; init; }
}
