namespace DevilDaggersInfo.Api.Main.Tools;

public record GetTool
{
	public string Name { get; init; } = string.Empty;

	public string DisplayName { get; init; } = string.Empty;

	public string VersionNumber { get; init; } = string.Empty;

	public string VersionNumberRequired { get; init; } = string.Empty;

	public IReadOnlyList<GetToolVersion>? Changelog { get; init; }
}
