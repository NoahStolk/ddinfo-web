namespace DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

public record GetCustomLeaderboardCriteria
{
	public required CustomLeaderboardCriteriaType Type { get; init; }

	public required CustomLeaderboardCriteriaOperator Operator { get; init; }

	public required byte[] Expression { get; init; }
}
