using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

// The base class clears and rewrites the shared mod and mod archive cache directories for every test case.
[NotInParallel]
internal sealed class ModArchiveProcessorProcessTests : ModArchiveProcessorTests
{
	// TODO: Add a failing test where the ModBinaryType is incorrect.
	[Test]
	public async Task ProcessNewMod_1Binary_1Asset()
	{
		const string modName = "mod";
		BinaryName binaryName = new(ModBinaryType.Dd, "main");
		const string assetName = "binding";

		DdModBinaryBuilder binary = CreateWithBinding(assetName);
		await Processor.ProcessModBinaryUploadAsync(modName, new Dictionary<BinaryName, byte[]> { [binaryName] = binary.Compile() });

		string zipFilePath = Accessor.GetModArchivePath(modName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(1);

		ModBinaryCacheData processedBinary = await GetProcessedBinaryFromArchiveEntryAsync(archive.Entries[0]);
		await Assert.That(processedBinary.ModBinaryType).IsEqualTo(ModBinaryType.Dd);
		await AssertBinaryNameAsync(binaryName, processedBinary.Name, modName);
		await Assert.That(processedBinary.TocEntries.Count).IsEqualTo(1);
		await Assert.That(processedBinary.TocEntries[0].Name).IsEqualTo(assetName);
		await Assert.That(processedBinary.TocEntries[0].AssetType).IsEqualTo(AssetType.ObjectBinding);
	}

	[Test]
	public async Task ProcessNewMod_1Binary_2Assets()
	{
		const string modName = "mod";
		BinaryName binaryName = new(ModBinaryType.Dd, "main");
		const string assetName1 = "binding";
		const string assetName2 = "texture";

		ModBinaryBuilder binary = CreateWithBindingAndTexture(assetName1, assetName2);
		await Processor.ProcessModBinaryUploadAsync(modName, new Dictionary<BinaryName, byte[]> { [binaryName] = binary.Compile() });

		string zipFilePath = Accessor.GetModArchivePath(modName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(1);

		ModBinaryCacheData processedBinary = await GetProcessedBinaryFromArchiveEntryAsync(archive.Entries[0]);
		await Assert.That(processedBinary.ModBinaryType).IsEqualTo(ModBinaryType.Dd);
		await AssertBinaryNameAsync(binaryName, processedBinary.Name, modName);
		await Assert.That(processedBinary.TocEntries.Count).IsEqualTo(2);
		await Assert.That(processedBinary.TocEntries[0].Name).IsEqualTo(assetName1);
		await Assert.That(processedBinary.TocEntries[0].AssetType).IsEqualTo(AssetType.ObjectBinding);
		await Assert.That(processedBinary.TocEntries[1].Name).IsEqualTo(assetName2);
		await Assert.That(processedBinary.TocEntries[1].AssetType).IsEqualTo(AssetType.Texture);
	}

	[Test]
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
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(2);

		ModBinaryCacheData processedBinary1 = await GetProcessedBinaryFromArchiveEntryAsync(archive.Entries[0]);
		await Assert.That(processedBinary1.ModBinaryType).IsEqualTo(ModBinaryType.Dd);
		await AssertBinaryNameAsync(binaryName1, processedBinary1.Name, modName);
		await Assert.That(processedBinary1.TocEntries.Count).IsEqualTo(2);
		await Assert.That(processedBinary1.TocEntries[0].Name).IsEqualTo(assetName1);
		await Assert.That(processedBinary1.TocEntries[0].AssetType).IsEqualTo(AssetType.ObjectBinding);
		await Assert.That(processedBinary1.TocEntries[1].Name).IsEqualTo(assetName2);
		await Assert.That(processedBinary1.TocEntries[1].AssetType).IsEqualTo(AssetType.Texture);

		ModBinaryCacheData processedBinary2 = await GetProcessedBinaryFromArchiveEntryAsync(archive.Entries[1]);
		await Assert.That(processedBinary2.ModBinaryType).IsEqualTo(ModBinaryType.Dd);
		await AssertBinaryNameAsync(binaryName2, processedBinary2.Name, modName);
		await Assert.That(processedBinary2.TocEntries.Count).IsEqualTo(2);
		await Assert.That(processedBinary2.TocEntries[0].Name).IsEqualTo(assetName1);
		await Assert.That(processedBinary2.TocEntries[0].AssetType).IsEqualTo(AssetType.ObjectBinding);
		await Assert.That(processedBinary2.TocEntries[1].Name).IsEqualTo(assetName2);
		await Assert.That(processedBinary2.TocEntries[1].AssetType).IsEqualTo(AssetType.Texture);
	}
}
