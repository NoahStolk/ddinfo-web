using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

public record ModFileSystemData(ModArchiveCacheData ModArchive, List<string> ScreenshotFileNames);
