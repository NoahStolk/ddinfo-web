using DevilDaggersInfo.Types.Core.CustomLeaderboards;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetUploadResponseCriteriaRejection
{
	public required string CriteriaName { get; init; }

	public CustomLeaderboardCriteriaOperator CriteriaOperator { get; init; }

	public int ExpectedValue { get; init; }

	public int ActualValue { get; init; }
}
