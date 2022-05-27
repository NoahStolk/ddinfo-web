using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Utils;
using System.IO.Compression;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Tests;

[TestClass]
public class ModArchiveProcessorTransformTests : ModArchiveProcessorTests
{
	[TestMethod]
	public async Task Transform_Remove1()
	{
		const string modName = "mod";
		const string binaryName1 = "main";
		const string binaryName2 = "binaryToDelete";
		const string assetName = "binding";

		ModBinary binary1 = new(ModBinaryType.Dd);
		binary1.AddAsset(assetName, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));

		ModBinary binary2 = new(ModBinaryType.Dd);
		binary2.AddAsset(assetName, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"egg\""));

		Dictionary<string, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries, new());

		await Processor.TransformBinariesInModArchiveAsync(modName, modName, new() { GetBinaryNameWithPrefix(binary2.ModBinaryType, modName, binaryName2) }, new(), new());

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);
		Assert.AreEqual(GetBinaryNameWithPrefix(binary1.ModBinaryType, modName, binaryName1), archive.Entries[0].Name);
	}

	private static string GetBinaryNameWithPrefix(ModBinaryType modBinaryType, string modName, string binaryName)
	{
		return BinaryFileNameUtils.GetBinaryPrefix(modBinaryType, modName) + binaryName;
	}
}
