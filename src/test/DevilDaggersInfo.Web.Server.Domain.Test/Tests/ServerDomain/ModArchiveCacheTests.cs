using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Services;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

[TestClass]
public class ModArchiveCacheTests
{
	[TestMethod]
	public async Task GetModArchiveCacheData()
	{
		const string binaryName = "maken";

		string directory = Path.Combine("Data", "ModArchives");
		string filePath = Path.Combine(directory, $"{binaryName}.json");
		const string fileContents = """
			{"FileSize":8400,"FileSizeExtracted":21891,"Binaries":[{"Name":"dd-maken-main","Size":21891,"ModBinaryType":1,"Chunks":[{"Name":"dagger6","Size":21855,"AssetType":2,"IsProhibited":false}],"ModifiedLoudnessAssets":null}]}
			""";
		Directory.CreateDirectory(directory);
		await File.WriteAllTextAsync(filePath, fileContents);

		ModArchiveCache cache = new(new FileSystemService());
		ModArchiveCacheData data = await cache.GetArchiveDataByFilePathAsync($"{binaryName}.json");
		Assert.AreEqual(8400, data.FileSize);
		Assert.AreEqual(21891, data.FileSizeExtracted);
		Assert.AreEqual(1, data.Binaries.Count);
		Assert.AreEqual("dd-maken-main", data.Binaries[0].Name);
		Assert.AreEqual(21891, data.Binaries[0].Size);
		Assert.AreEqual(ModBinaryType.Dd, data.Binaries[0].ModBinaryType);
		Assert.AreEqual(1, data.Binaries[0].TocEntries.Count);
		Assert.AreEqual("dagger6", data.Binaries[0].TocEntries[0].Name);
		Assert.AreEqual(21855, data.Binaries[0].TocEntries[0].Size);
		Assert.AreEqual(AssetType.Texture, data.Binaries[0].TocEntries[0].AssetType);
		Assert.IsFalse(data.Binaries[0].TocEntries[0].IsProhibited);
		Assert.IsNull(data.Binaries[0].ModifiedLoudnessAssets);
	}
}
