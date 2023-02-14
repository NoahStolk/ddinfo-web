using DevilDaggersInfo.Types.Core.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record UploadCriteriaRejection
{
	public required string CriteriaName { get; init; }

	public CustomLeaderboardCriteriaOperator CriteriaOperator { get; init; }

	public int ExpectedValue { get; init; }

	public int ActualValue { get; init; }
}
