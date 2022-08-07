namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public class ToolVersion
{
	public string VersionNumber { get; init; } = string.Empty;

	public DateTime Date { get; init; }

	public int DownloadCount { get; init; }

	public IReadOnlyList<ToolVersionChange> Changes { get; init; } = new List<ToolVersionChange>();
}
