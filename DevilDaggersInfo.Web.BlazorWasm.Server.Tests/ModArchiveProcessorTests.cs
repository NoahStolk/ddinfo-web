using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using System.IO.Compression;

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

	[AssertionMethod]
	protected static ModBinaryCacheData GetProcessedBinaryFromArchiveEntry(ZipArchiveEntry entry)
	{
		Assert.IsFalse(string.IsNullOrEmpty(entry.Name));

		byte[] extractedContents = new byte[entry.Length];
		using (Stream entryStream = entry.Open())
		{
			int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
			Assert.AreEqual(extractedContents.Length, readBytes, "Premature end of stream.");
		}

		return ModBinaryCacheData.CreateFromFile(entry.Name, extractedContents);
	}
}
