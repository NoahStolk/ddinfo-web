namespace DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

public record GetCustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required string? Expression { get; init; }
}
