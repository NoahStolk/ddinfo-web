namespace DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

public record GetTool
{
	public string Name { get; init; } = string.Empty;

	public string DisplayName { get; init; } = string.Empty;

	public string LatestVersionNumber { get; init; } = string.Empty;

	public string LatestCompatibleVersionNumber { get; init; } = string.Empty;

	public IReadOnlyList<GetToolVersion>? Changelog { get; init; }

	public List<GetToolDistribution> Distributions { get; init; } = new();
}
