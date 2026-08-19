using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class ModArchiveCacheTests
{
	[Test]
	public async Task GetModArchiveCacheData()
	{
		ModArchiveCache cache = new(new TestData());
		ModArchiveCacheData data = await cache.GetArchiveDataByFilePathAsync("test.json");
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
