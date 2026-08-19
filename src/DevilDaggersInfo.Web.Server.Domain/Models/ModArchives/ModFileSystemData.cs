namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public sealed record ModFileSystemData
{
	public required ModArchiveCacheData? ModArchive { get; init; }

	public required List<string>? ScreenshotFileNames { get; init; }
}
