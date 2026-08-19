namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public sealed record GetCustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaType Type { get; init; }

	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required byte[] Expression { get; init; }
}
