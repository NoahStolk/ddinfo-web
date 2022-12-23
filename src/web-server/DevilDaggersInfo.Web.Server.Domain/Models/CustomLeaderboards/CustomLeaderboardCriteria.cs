using DevilDaggersInfo.Types.Core.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaType Type { get; init; }

	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required byte[] Expression { get; init; }
}
