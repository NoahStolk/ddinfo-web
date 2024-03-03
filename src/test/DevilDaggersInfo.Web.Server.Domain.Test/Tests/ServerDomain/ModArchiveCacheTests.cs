using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

[TestClass]
public class ModArchiveCacheTests
{
	[TestMethod]
	public async Task GetModArchiveCacheData()
	{
		ModArchiveCache cache = new(new TestData());
		ModArchiveCacheData data = await cache.GetArchiveDataByFilePathAsync("test.json");
		Assert.AreEqual(8400, data.FileSize);
		Assert.AreEqual(21891, data.FileSizeExtracted);
		Assert.AreEqual(1, data.Binaries.Count);

		ModBinaryCacheData binary = data.Binaries[0];
		Assert.AreEqual("dd-test-main", binary.Name);
		Assert.AreEqual(21891, binary.Size);
		Assert.AreEqual(ModBinaryType.Dd, binary.ModBinaryType);
		Assert.AreEqual(1, binary.TocEntries.Count);

		ModTocEntryCacheData tocEntry = binary.TocEntries[0];
		Assert.AreEqual("dagger6", tocEntry.Name);
		Assert.AreEqual(21855, tocEntry.Size);
		Assert.AreEqual(AssetType.Texture, tocEntry.AssetType);
		Assert.IsFalse(tocEntry.IsProhibited);
		Assert.IsNull(binary.ModifiedLoudnessAssets);
	}
}
