namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

public record GetArrayStatistic
{
	public double Average { get; init; }
	public double Median { get; init; }
	public double Mode { get; init; }
}
