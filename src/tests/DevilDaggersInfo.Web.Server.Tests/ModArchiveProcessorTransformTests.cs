using DevilDaggersInfo.Core.Mod;
using System.IO.Compression;

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

		ModBinary binary1 = CreateWithBinding(assetName);
		ModBinary binary2 = CreateWithBinding(assetName);
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

	[TestMethod]
	public async Task Transform_Add1()
	{
		const string modName = "mod";
		const string binaryName1 = "main1";
		const string binaryName2 = "main2";
		const string binaryName3 = "new";
		const string assetName = "binding";

		ModBinary binary1 = CreateWithBinding(assetName);
		ModBinary binary2 = CreateWithBinding(assetName);
		Dictionary<string, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries, new());

		ModBinary binary3 = CreateWithBinding(assetName);
		await Processor.TransformBinariesInModArchiveAsync(modName, modName, new(), new() { { GetBinaryNameWithPrefix(binary3.ModBinaryType, modName, binaryName3), binary3.Compile() } }, new());

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(3, archive.Entries.Count);
		Assert.AreEqual(GetBinaryNameWithPrefix(binary1.ModBinaryType, modName, binaryName1), archive.Entries[0].Name);
		Assert.AreEqual(GetBinaryNameWithPrefix(binary2.ModBinaryType, modName, binaryName2), archive.Entries[1].Name);
		Assert.AreEqual(GetBinaryNameWithPrefix(binary3.ModBinaryType, modName, binaryName3), archive.Entries[2].Name);
	}
}
