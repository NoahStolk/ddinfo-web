using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using System.IO.Compression;
using System.Text;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

[TestClass]
public class ModArchiveProcessorTests
{
	private readonly ModArchiveCache _cache;
	private readonly ModArchiveAccessor _accessor;
	private readonly ModArchiveProcessor _processor;

	public ModArchiveProcessorTests()
	{
		string modsPath = Path.Combine(TestUtils.ResourcePath, "Mods");
		string modArchiveCachePath = Path.Combine(TestUtils.ResourcePath, "ModArchiveCache");

		if (Directory.Exists(modsPath))
			Directory.Delete(modsPath, true);

		if (Directory.Exists(modArchiveCachePath))
			Directory.Delete(modArchiveCachePath, true);

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Mods)).Returns(modsPath);
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.ModArchiveCache)).Returns(modArchiveCachePath);

		Directory.CreateDirectory(modsPath);
		Directory.CreateDirectory(modArchiveCachePath);

		_cache = new(fileSystemService.Object);
		_accessor = new(fileSystemService.Object, _cache);
		_processor = new(fileSystemService.Object, _cache, _accessor);
	}

	[TestMethod]
	public async Task ProcessNewMod_1Binary_1Asset()
	{
		const string modName = "mod";
		const string binaryName = "main";
		const string assetName = "binding";

		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName, AssetType.ModelBinding, Encoding.Default.GetBytes("shader = \"boid\""));

		await _processor.ProcessModBinaryUploadAsync(modName, new Dictionary<string, byte[]> { [binaryName] = binary.Compile() }, new());

		string zipFilePath = _accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName, modName), processedBinary.Name);
		Assert.AreEqual(1, processedBinary.Chunks.Count);
		Assert.AreEqual(assetName, processedBinary.Chunks[0].Name);
		Assert.AreEqual(AssetType.ModelBinding, processedBinary.Chunks[0].AssetType);
	}

	[TestMethod]
	public async Task ProcessNewMod_1Binary_2Assets()
	{
		const string modName = "mod";
		const string binaryName = "main";
		const string assetName1 = "binding";
		const string assetName2 = "shader";

		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName1, AssetType.ModelBinding, Encoding.Default.GetBytes("shader = \"boid\""));
		binary.AddAsset(assetName2, AssetType.Texture, File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Textures", "pixel.png")));

		await _processor.ProcessModBinaryUploadAsync(modName, new Dictionary<string, byte[]> { [binaryName] = binary.Compile() }, new());

		string zipFilePath = _accessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
		Assert.AreEqual(1, archive.Entries.Count);

		ModBinaryCacheData processedBinary = GetProcessedBinaryFromArchiveEntry(archive.Entries[0]);
		Assert.AreEqual(ModBinaryType.Dd, processedBinary.ModBinaryType);
		Assert.AreEqual(BinaryFileNameUtils.SanitizeModBinaryFileName(binaryName, modName), processedBinary.Name);
		Assert.AreEqual(2, processedBinary.Chunks.Count);
		Assert.AreEqual(assetName1, processedBinary.Chunks[0].Name);
		Assert.AreEqual(AssetType.ModelBinding, processedBinary.Chunks[0].AssetType);
		Assert.AreEqual(assetName2, processedBinary.Chunks[1].Name);
		Assert.AreEqual(AssetType.Texture, processedBinary.Chunks[1].AssetType);
	}

	[AssertionMethod]
	public static ModBinaryCacheData GetProcessedBinaryFromArchiveEntry(ZipArchiveEntry entry)
	{
		Assert.IsFalse(string.IsNullOrEmpty(entry.Name));

		byte[] extractedContents = new byte[entry.Length];
		using (Stream entryStream = entry.Open())
		{
			int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
			Assert.AreEqual(extractedContents.Length, readBytes, "Premature end of stream.");
		}

		return ModBinaryCacheData.CreateFromFile(entry.Name, extractedContents);
	}
}
