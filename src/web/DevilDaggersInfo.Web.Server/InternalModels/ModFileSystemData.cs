using DevilDaggersInfo.Web.Server.Caches.ModArchives;

namespace DevilDaggersInfo.Web.Server.InternalModels;

public record ModFileSystemData(ModArchiveCacheData? ModArchive, List<string>? ScreenshotFileNames);
