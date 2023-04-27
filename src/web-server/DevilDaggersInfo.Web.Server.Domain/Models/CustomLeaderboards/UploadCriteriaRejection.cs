using DevilDaggersInfo.Core.CriteriaExpression;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record UploadCriteriaRejection
{
	public required string CriteriaName { get; init; }

	public required CustomLeaderboardCriteriaOperator CriteriaOperator { get; init; }

	public required int ExpectedValue { get; init; }

	public required int ActualValue { get; init; }
}
