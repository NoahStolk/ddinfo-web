using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using NSubstitute;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

public abstract class ModArchiveProcessorTests
{
	protected ModArchiveProcessorTests()
	{
		string modsPath = Path.Combine("Resources", "Mods");
		string modArchiveCachePath = Path.Combine("Resources", "ModArchiveCache");

		if (Directory.Exists(modsPath))
			Directory.Delete(modsPath, true);

		if (Directory.Exists(modArchiveCachePath))
			Directory.Delete(modArchiveCachePath, true);

		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		fileSystemService.GetPath(DataSubDirectory.Mods).Returns(modsPath);
		fileSystemService.GetPath(DataSubDirectory.ModArchiveCache).Returns(modArchiveCachePath);

		Directory.CreateDirectory(modsPath);
		Directory.CreateDirectory(modArchiveCachePath);

		Cache = new(fileSystemService);
		Accessor = new(fileSystemService, Cache);
		Processor = new(fileSystemService, Cache, Accessor);
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

	protected static DdModBinaryBuilder CreateWithBinding(string assetName)
	{
		DdModBinaryBuilder binary = new();
		binary.AddObjectBinding(assetName, "shader = \"boid\""u8.ToArray());
		return binary;
	}

	protected static DdModBinaryBuilder CreateWithBindingAndTexture(string shaderName, string textureName)
	{
		DdModBinaryBuilder binary = new();
		binary.AddObjectBinding(shaderName, "shader = \"boid\""u8.ToArray());
		binary.AddTexture(textureName, File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png")));
		return binary;
	}
}
