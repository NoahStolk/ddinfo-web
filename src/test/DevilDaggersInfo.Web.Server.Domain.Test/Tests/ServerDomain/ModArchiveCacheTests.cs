using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class ModArchiveCacheTests
{
	[Test]
	public async Task GetModArchiveCacheData()
	{
		InMemoryFileSystem fileSystem = new();
		fileSystem.Seed(
			DataSubDirectory.ModArchiveCache,
			"test.json",
			Encoding.UTF8.GetBytes(
				"""
				{
					"FileSize": 8400,
					"FileSizeExtracted": 21891,
					"Binaries": [
						{
							"Name": "dd-test-main",
							"Size": 21891,
							"ModBinaryType": 1,
							"Chunks": [{ "Name": "dagger6", "Size": 21855, "AssetType": 2, "IsProhibited": false }],
							"ModifiedLoudnessAssets": null
						}
					]
				}
				"""));

		ModArchiveCache cache = new(fileSystem);
		ModArchiveCacheData data = await cache.GetArchiveDataByModNameAsync("test");
		await Assert.That(data.FileSize).IsEqualTo(8400);
		await Assert.That(data.FileSizeExtracted).IsEqualTo(21891);
		await Assert.That(data.Binaries.Count).IsEqualTo(1);

		ModBinaryCacheData binary = data.Binaries[0];
		await Assert.That(binary.Name).IsEqualTo("dd-test-main");
		await Assert.That(binary.Size).IsEqualTo(21891);
		await Assert.That(binary.ModBinaryType).IsEqualTo(ModBinaryType.Dd);
		await Assert.That(binary.TocEntries.Count).IsEqualTo(1);

		ModTocEntryCacheData tocEntry = binary.TocEntries[0];
		await Assert.That(tocEntry.Name).IsEqualTo("dagger6");
		await Assert.That(tocEntry.Size).IsEqualTo(21855);
		await Assert.That(tocEntry.AssetType).IsEqualTo(AssetType.Texture);
		await Assert.That(tocEntry.IsProhibited).IsFalse();
		await Assert.That(binary.ModifiedLoudnessAssets).IsNull();
	}
}
