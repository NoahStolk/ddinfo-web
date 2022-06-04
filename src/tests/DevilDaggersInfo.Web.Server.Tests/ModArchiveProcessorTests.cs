using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Web.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.Server.Enums;
using System.IO.Compression;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Tests;

public abstract class ModArchiveProcessorTests
{
	protected ModArchiveProcessorTests()
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

		Cache = new(fileSystemService.Object);
		Accessor = new(fileSystemService.Object, Cache);
		Processor = new(fileSystemService.Object, Cache, Accessor);
	}

	protected ModArchiveCache Cache { get; }
	protected ModArchiveAccessor Accessor { get; }
	protected ModArchiveProcessor Processor { get; }

	[AssertionMethod]
	protected static void AssertBinaryName(BinaryName binaryName, string name, string modName)
	{
		Assert.AreEqual(binaryName.ToFullName(modName), name);
		Assert.AreEqual(binaryName, BinaryName.Parse(name, modName));
	}

	[AssertionMethod]
	protected static ModBinaryCacheData GetProcessedBinaryFromArchiveEntry(ZipArchiveEntry entry)
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

	protected static ModBinary CreateWithBinding(string assetName)
	{
		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));
		return binary;
	}

	protected static ModBinary CreateWithBindingAndTexture(string shaderName, string textureName)
	{
		ModBinary binary = new(ModBinaryType.Dd);
		binary.AddAsset(shaderName, AssetType.ObjectBinding, Encoding.Default.GetBytes("shader = \"boid\""));
		binary.AddAsset(textureName, AssetType.Texture, File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Textures", "green.png")));
		return binary;
	}
}
