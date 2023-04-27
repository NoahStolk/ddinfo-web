using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

public abstract class ModArchiveProcessorTests
{
	protected ModArchiveProcessorTests()
	{
		string modsPath = Path.Combine("Resources", "Mods");
		string modArchiveCachePath = Path.Combine("Resources", "ModArchiveCache");

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Mods)).Returns(modsPath);
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.ModArchiveCache)).Returns(modArchiveCachePath);

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

	protected static ModBinaryBuilder CreateWithBinding(string assetName)
	{
		ModBinaryBuilder binary = new(ModBinaryType.Dd);
		binary.AddAsset(assetName, AssetType.ObjectBinding, "shader = \"boid\""u8.ToArray());
		return binary;
	}

	protected static ModBinaryBuilder CreateWithBindingAndTexture(string shaderName, string textureName)
	{
		ModBinaryBuilder binary = new(ModBinaryType.Dd);
		binary.AddAsset(shaderName, AssetType.ObjectBinding, "shader = \"boid\""u8.ToArray());
		binary.AddAsset(textureName, AssetType.Texture, File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png")));
		return binary;
	}
}
