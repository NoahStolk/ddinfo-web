namespace DevilDaggersInfo.Web.ApiSpec.App.CustomLeaderboards;

public record GetUploadResponseCriteriaRejection
{
	public required string CriteriaName { get; init; }

	public required CustomLeaderboardCriteriaOperator CriteriaOperator { get; init; }

	public required int ExpectedValue { get; init; }

	public required int ActualValue { get; init; }
}
