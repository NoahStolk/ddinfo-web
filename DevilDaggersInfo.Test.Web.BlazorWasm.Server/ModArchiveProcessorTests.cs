using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

public abstract class ModArchiveProcessorTests
{
	protected ModArchiveProcessorTests()
	{
		string modsPath = Path.Combine(TestUtils.ResourcePath, "Mods");
		string modArchiveCachePath = Path.Combine(TestUtils.ResourcePath, "ModArchiveCache");

		if (Directory.Exists(modsPath))
			Directory.Delete(modsPath, true);

		if (Directory.Exists(modArchiveCachePath))
			Directory.Delete(modArchiveCachePath, true);

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Mods)).Returns(modsPath);
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.ModArchiveCache)).Returns(modArchiveCachePath);

		Directory.CreateDirectory(modsPath);
		Directory.CreateDirectory(modArchiveCachePath);

		Cache = new(fileSystemService.Object);
		Accessor = new(fileSystemService.Object, Cache);
		Processor = new(fileSystemService.Object, Cache, Accessor);
	}

	protected ModArchiveCache Cache { get; }
	protected ModArchiveAccessor Accessor { get; }
	protected ModArchiveProcessor Processor { get; }
}
