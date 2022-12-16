namespace DevilDaggersInfo.Api.Main.Tools;

public record GetTool
{
	public required string Name { get; init; }

	public required string DisplayName { get; init; }

	public required string VersionNumber { get; init; }

	public required string VersionNumberRequired { get; init; }

	public required IReadOnlyList<GetToolVersion>? Changelog { get; init; }
}
