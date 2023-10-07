namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

public record GetToolVersion
{
	public required string VersionNumber { get; init; }

	public required DateTime Date { get; init; }

	public required int DownloadCount { get; init; }

	public required IReadOnlyList<GetToolVersionChange> Changes { get; init; } = new List<GetToolVersionChange>();
}
