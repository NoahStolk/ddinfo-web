namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public class Tool
{
	public string Name { get; init; } = string.Empty;

	public string DisplayName { get; init; } = string.Empty;

	public string VersionNumber { get; init; } = string.Empty;

	public string VersionNumberRequired { get; init; } = string.Empty;

	public IReadOnlyList<ToolVersion>? Changelog { get; init; }
}
