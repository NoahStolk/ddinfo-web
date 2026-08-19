namespace DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

public sealed record AddCustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required string? Expression { get; init; }
}
