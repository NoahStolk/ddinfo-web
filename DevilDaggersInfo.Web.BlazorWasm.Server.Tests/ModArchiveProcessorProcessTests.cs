using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using System.IO.Compression;
using System.Text;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Tests;

[TestClass]
public class ModArchiveProcessorProcessTests : ModArchiveProcessorTests
{
	[TestMethod]
	public async Task ProcessNewMod_1Binary_1Asset()
	{
		const string modName = "mod";
		const string binaryName = "main";
		const string assetName = "binding";

		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));

		await Processor.ProcessModBinaryUploadAsync(modName, new Dictionary<string, byte[]> { [binaryName] = binary.Compile() }, new());

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName, modName), processedBinary.Name);
		Assert.AreEqual(1, processedBinary.Chunks.Count);
		Assert.AreEqual(assetName, processedBinary.Chunks[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary.Chunks[0].AssetType);
	}

	[TestMethod]
	public async Task ProcessNewMod_1Binary_2Assets()
	{
		const string modName = "mod";
		const string binaryName = "main";
		const string assetName1 = "binding";
		const string assetName2 = "texture";

		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName1, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));
		binary.AddAsset(assetName2, AssetType.Texture, File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Textures", "green.png")));

		await Processor.ProcessModBinaryUploadAsync(modName, new Dictionary<string, byte[]> { [binaryName] = binary.Compile() }, new());

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName, modName), processedBinary.Name);
		Assert.AreEqual(2, processedBinary.Chunks.Count);
		Assert.AreEqual(assetName1, processedBinary.Chunks[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary.Chunks[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary.Chunks[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary.Chunks[1].AssetType);
	}

	[TestMethod]
	public async Task ProcessNewMod_2Binaries_2Assets()
	{
		const string modName = "mod";
		const string binaryName1 = "main";
		const string binaryName2 = "test";
		const string assetName1 = "binding";
		const string assetName2 = "texture";

		ModBinary binary1 = new(ModBinaryType.Dd);
		binary1.AddAsset(assetName1, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));
		binary1.AddAsset(assetName2, AssetType.Texture, File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Textures", "green.png")));

		ModBinary binary2 = new(ModBinaryType.Dd);
		binary2.AddAsset(assetName1, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"egg\""));
		binary2.AddAsset(assetName2, AssetType.Texture, File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Textures", "blue.png")));

		Dictionary<string, byte[]> binaries = new()
		{
			[binaryName1] = binary1.Compile(),
			[binaryName2] = binary2.Compile(),
		};
		await Processor.ProcessModBinaryUploadAsync(modName, binaries, new());

		string zipFilePath = Accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(2, archive.Entries.Count);

		ModBinaryCacheData processedBinary1 = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary1.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName1, modName), processedBinary1.Name);
		Assert.AreEqual(2, processedBinary1.Chunks.Count);
		Assert.AreEqual(assetName1, processedBinary1.Chunks[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary1.Chunks[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary1.Chunks[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary1.Chunks[1].AssetType);

		ModBinaryCacheData processedBinary2 = GetProcessedBinaryFromArchiveEntry(archive.Entries[1]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary2.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName2, modName), processedBinary2.Name);
		Assert.AreEqual(2, processedBinary2.Chunks.Count);
		Assert.AreEqual(assetName1, processedBinary2.Chunks[0].Name);
		Assert.AreEqual(AssetType.ObjectBinding, processedBinary2.Chunks[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary2.Chunks[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary2.Chunks[1].AssetType);
	}
}
