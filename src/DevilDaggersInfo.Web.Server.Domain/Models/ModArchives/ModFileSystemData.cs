namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public record ModFileSystemData
{
	public required ModArchiveCacheData? ModArchive { get; init; }

	public required List<string>? ScreenshotFileNames { get; init; }
}
