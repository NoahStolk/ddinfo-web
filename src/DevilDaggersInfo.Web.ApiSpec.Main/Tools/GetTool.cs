namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

public record GetTool
{
	public required string Name { get; init; }

	public required string DisplayName { get; init; }

	public required string VersionNumber { get; init; }

	public required string VersionNumberRequired { get; init; }

	// TODO: This is only used as a fallback for DDSE/DDAE changelog windows. Remove this when DDSE/DDAE are no longer supported.
	[Obsolete("This data is no longer available.")]
	public IReadOnlyList<GetToolVersion>? Changelog { get; set; }

	public required IReadOnlyList<GetToolDownloadCount> DownloadCounts { get; init; }
}
