using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

[TestClass]
public class ModArchiveProcessorTransformTests : ModArchiveProcessorTests
{
	[DataTestMethod]
	[DataRow("mod", "mod-renamed")]
	[DataRow("mod", "renamed")]
	[DataRow("mod", "modd")]
	[DataRow("mod", "mmod")]
	[DataRow("mod", "m")]
	[DataRow("mod", "")]
	public async Task Transform_Rename(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main2");
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new(), new());

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(2, archive.Entries.Count);
		AssertBinaryName(binaryName1, archive.Entries[0].Name, newModName);
		AssertBinaryName(binaryName2, archive.Entries[1].Name, newModName);
	}

	[DataTestMethod]
	[DataRow("mod", "mod")]
	[DataRow("mod", "mod-renamed")]
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

		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new() { binaryName2 }, new());

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);
		AssertBinaryName(binaryName1, archive.Entries[0].Name, newModName);
	}

	[DataTestMethod]
	[DataRow("mod", "mod")]
	[DataRow("mod", "mod-renamed")]
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
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new(), new() { { binaryName3, binary3.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(3, archive.Entries.Count);
		AssertBinaryName(binaryName1, archive.Entries[0].Name, newModName);
		AssertBinaryName(binaryName2, archive.Entries[1].Name, newModName);
		AssertBinaryName(binaryName3, archive.Entries[2].Name, newModName);
	}

	[DataTestMethod]
	[DataRow("mod", "mod")]
	[DataRow("mod", "mod-renamed")]
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
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new() { binaryName2 }, new() { { binaryName3, binary3.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(2, archive.Entries.Count);
		AssertBinaryName(binaryName1, archive.Entries[0].Name, newModName);
		AssertBinaryName(binaryName3, archive.Entries[1].Name, newModName);
	}

	[DataTestMethod]
	[DataRow("mod", "mod")]
	[DataRow("mod", "mod-renamed")]
	public async Task Transform_Remove1_Add1_SameName_Fail(string modName, string newModName)
	{
		BinaryName binaryName1 = new(ModBinaryType.Dd, "main1");
		BinaryName binaryName2 = new(ModBinaryType.Dd, "main1"); // Same name, should fail
		const string assetName = "binding";

		ModBinaryBuilder binary1 = CreateWithBinding(assetName);
		Dictionary<BinaryName, byte[]> binaries = new() { [binaryName1] = binary1.Compile() };
		await Processor.ProcessModBinaryUploadAsync(modName, binaries);

		ModBinaryBuilder binary2 = CreateWithBinding(assetName);
		await Assert.ThrowsExceptionAsync<InvalidModArchiveException>(async () => await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new(), new() { { binaryName2, binary2.Compile() } }));

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);
		AssertBinaryName(binaryName1, archive.Entries[0].Name, modName);
	}

	[DataTestMethod]
	[DataRow("mod", "mod")]
	[DataRow("mod", "mod-renamed")]
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
		await Processor.TransformBinariesInModArchiveAsync(modName, newModName, new() { binaryName1 }, new() { { binaryName2, binary2.Compile() } });

		string zipFilePath = Accessor.GetModArchivePath(newModName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);
		AssertBinaryName(binaryName2, archive.Entries[0].Name, newModName);

		// Test if the asset name is actually updated.
		ModBinaryCacheData modBinaryCacheData = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(1, modBinaryCacheData.Chunks.Count);
		Assert.AreEqual(assetName2, modBinaryCacheData.Chunks[0].Name);
	}
}
