using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

// The base class clears and rewrites the shared mod and mod archive cache directories for every test case.
[NotInParallel]
internal sealed class ModArchiveProcessorTransformTests : ModArchiveProcessorTests
{
	[Test]
	[Arguments("mod", "mod-renamed")]
	[Arguments("mod", "renamed")]
	[Arguments("mod", "modd")]
	[Arguments("mod", "mmod")]
	[Arguments("mod", "m")]
	[Arguments("mod", "")]
	public async Task Transform_Rename(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main2");
		const string assetName = "binding";

		DdModBinaryBuilder binary1 = CreateWithBinding(assetName);
		DdModBinaryBuilder binary2 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [], new Dictionary<BinaryName, byte[]>());

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(2);
		await AssertBinaryNameAsync(binaryName1, archive.Entries[0].Name, newModName);
		await AssertBinaryNameAsync(binaryName2, archive.Entries[1].Name, newModName);
	}

	[Test]
	[Arguments("mod", "mod")]
	[Arguments("mod", "mod-renamed")]
	public async Task Transform_Remove1(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "binaryToDelete");
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [binaryName2], new Dictionary<BinaryName, byte[]>());

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(1);
		await AssertBinaryNameAsync(binaryName1, archive.Entries[0].Name, newModName);
	}

	[Test]
	[Arguments("mod", "mod")]
	[Arguments("mod", "mod-renamed")]
	public async Task Transform_Add1(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main2");
		BinaryName binaryName3 = new(ModBinaryType.Dd, "new");
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		ModBinaryBuilder binary3 = CreateWithBinding(assetName);
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [], new Dictionary<BinaryName, byte[]> { { binaryName3, binary3.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(3);
		await AssertBinaryNameAsync(binaryName1, archive.Entries[0].Name, newModName);
		await AssertBinaryNameAsync(binaryName2, archive.Entries[1].Name, newModName);
		await AssertBinaryNameAsync(binaryName3, archive.Entries[2].Name, newModName);
	}

	[Test]
	[Arguments("mod", "mod")]
	[Arguments("mod", "mod-renamed")]
	public async Task Transform_Remove1_Add1(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "binaryToDelete");
		BinaryName binaryName3 = new(ModBinaryType.Dd, "new");
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		ModBinaryBuilder binary3 = CreateWithBinding(assetName);
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [binaryName2], new Dictionary<BinaryName, byte[]> { { binaryName3, binary3.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(2);
		await AssertBinaryNameAsync(binaryName1, archive.Entries[0].Name, newModName);
		await AssertBinaryNameAsync(binaryName3, archive.Entries[1].Name, newModName);
	}

	[Test]
	[Arguments("mod", "mod")]
	[Arguments("mod", "mod-renamed")]
	public async Task Transform_Remove1_Add1_SameName_Fail(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main1"); // Same name, should fail
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new() { [binaryName1] = binary1.Compile() };
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		await Assert.That(async () => await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [], new Dictionary<BinaryName, byte[]> { { binaryName2, binary2.Compile() } })).Throws<InvalidModArchiveException>();

		string zipFilePath = Accessor.GetModArchivePath(modName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(1);
		await AssertBinaryNameAsync(binaryName1, archive.Entries[0].Name, modName);
	}

	[Test]
	[Arguments("mod", "mod")]
	[Arguments("mod", "mod-renamed")]
	public async Task Transform_Replace1(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main1"); // Same name, but original is removed first, so should succeed
		const string assetName1 = "binding";
		const string assetName2 = "new-binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName1);
		Dictionary<BinaryName, byte[]> binaries = new() { [binaryName1] = binary1.Compile() };
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		ModBinaryBuilder binary2 = CreateWithBinding(assetName2);
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, [binaryName1], new Dictionary<BinaryName, byte[]> { { binaryName2, binary2.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		await using ZipArchive archive = await ZipFile.OpenAsync(zipFilePath, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Count).IsEqualTo(1);
		await AssertBinaryNameAsync(binaryName2, archive.Entries[0].Name, newModName);

		// Test if the asset name is actually updated.
		ModBinaryCacheData modBinaryCacheData = await GetProcessedBinaryFromArchiveEntryAsync(archive.Entries[0]);
		await Assert.That(modBinaryCacheData.TocEntries.Count).IsEqualTo(1);
		await Assert.That(modBinaryCacheData.TocEntries[0].Name).IsEqualTo(assetName2);
	}
}
