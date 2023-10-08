namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

[Obsolete("This data is no longer available.")]
public record GetToolVersion
{
	public string VersionNumber { get; init; } = string.Empty;

	public DateTime Date { get; init; }

	public int DownloadCount { get; init; }

	public IReadOnlyList<GetToolVersionChange> Changes { get; init; } = new List<GetToolVersionChange>();
}
