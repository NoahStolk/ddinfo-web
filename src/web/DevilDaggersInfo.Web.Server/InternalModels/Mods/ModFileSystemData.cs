using DevilDaggersInfo.Web.Server.Caches.ModArchives;

namespace DevilDaggersInfo.Web.Server.InternalModels.Mods;

public record ModFileSystemData(ModArchiveCacheData? ModArchive, List<string>? ScreenshotFileNames);
