namespace DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

public record GetToolVersion
{
	public string VersionNumber { get; init; } = string.Empty;

	public DateTime? Date { get; init; }

	public int DownloadCount { get; init; }

	public IReadOnlyList<GetToolVersionChange>? Changes { get; init; } = new List<GetToolVersionChange>();
}
