using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

[TestClass]
public class ModArchiveProcessorProcessTests : ModArchiveProcessorTests
{
	// TODO: Add a failing test where the ModBinaryType is incorrect.
	[TestMethod]
	public async Task ProcessNewMod_1Binary_1Asset()
	{
		const string modName = "mod";
		BinaryName binaryName = new(ModBinaryType.Dd, "main");
		const string assetName = "binding";

		DdModBinaryBuilder binary = CreateWithBinding(assetName);
		await Processor.ProcessModBinaryUploadAsync(modName, new() { [binaryName] = binary.Compile() });

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		AssertBinaryName(binaryName, processedBinary.Name, modName);
		Assert.AreEqual(1, processedBinary.Entries.Count);
		Assert.AreEqual(assetName, processedBinary.Entries[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary.Entries[0].AssetType);
	}

	[TestMethod]
	public async Task ProcessNewMod_1Binary_2Assets()
	{
		const string modName = "mod";
		BinaryName binaryName = new(ModBinaryType.Dd, "main");
		const string assetName1 = "binding";
		const string assetName2 = "texture";

		ModBinaryBuilder binary = CreateWithBindingAndTexture(assetName1, assetName2);
		await Processor.ProcessModBinaryUploadAsync(modName, new() { [binaryName] = binary.Compile() });

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		AssertBinaryName(binaryName, processedBinary.Name, modName);
		Assert.AreEqual(2, processedBinary.Entries.Count);
		Assert.AreEqual(assetName1, processedBinary.Entries[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary.Entries[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary.Entries[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary.Entries[1].AssetType);
	}

	[TestMethod]
	public async Task ProcessNewMod_2Binaries_2Assets()
	{
		const string modName = "mod";
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "test");
		const string assetName1 = "binding";
		const string assetName2 = "texture";

		ModBinaryBuilder binary1 = CreateWithBindingAndTexture(assetName1, assetName2);
		ModBinaryBuilder binary2 = CreateWithBindingAndTexture(assetName1, assetName2);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(2, archive.Entries.Count);

		ModBinaryCacheData processedBinary1 = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary1.ModBinaryType);
		AssertBinaryName(binaryName1, processedBinary1.Name, modName);
		Assert.AreEqual(2, processedBinary1.Entries.Count);
		Assert.AreEqual(assetName1, processedBinary1.Entries[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary1.Entries[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary1.Entries[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary1.Entries[1].AssetType);

		ModBinaryCacheData processedBinary2 = GetProcessedBinaryFromArchiveEntry(archive.Entries[1]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary2.ModBinaryType);
		AssertBinaryName(binaryName2, processedBinary2.Name, modName);
		Assert.AreEqual(2, processedBinary2.Entries.Count);
		Assert.AreEqual(assetName1, processedBinary2.Entries[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary2.Entries[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary2.Entries[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary2.Entries[1].AssetType);
	}
}
