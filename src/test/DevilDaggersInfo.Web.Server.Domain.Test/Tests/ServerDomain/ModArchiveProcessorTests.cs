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

internal abstract class ModArchiveProcessorTests
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

		Cache = new ModArchiveCache(fileSystemService);
		Accessor = new ModArchiveAccessor(fileSystemService, Cache);
		Processor = new ModArchiveProcessor(fileSystemService, Cache, Accessor);
	}

	protected ModArchiveCache Cache { get; }
	protected ModArchiveAccessor Accessor { get; }
	protected ModArchiveProcessor Processor { get; }

	[AssertionMethod]
	protected static async Task AssertBinaryNameAsync(BinaryName binaryName, string name, string modName)
	{
		await Assert.That(name).IsEqualTo(binaryName.ToFullName(modName));
		await Assert.That(BinaryName.Parse(name, modName)).IsEqualTo(binaryName);
	}

	[AssertionMethod]
	protected static async Task<ModBinaryCacheData> GetProcessedBinaryFromArchiveEntryAsync(ZipArchiveEntry entry)
	{
		await Assert.That(string.IsNullOrEmpty(entry.Name)).IsFalse();

		byte[] extractedContents = new byte[entry.Length];
		await using (Stream entryStream = await entry.OpenAsync())
		{
			int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
			await Assert.That(readBytes).IsEqualTo(extractedContents.Length).Because("Premature end of stream.");
		}

		return ModBinaryCacheData.CreateFromFile(entry.Name, extractedContents);
	}

	protected static DdModBinaryBuilder CreateWithBinding(string assetName)
	{
		DdModBinaryBuilder binary = new();
		binary.AddObjectBinding(assetName, [.. "shader = \"boid\""u8]);
		return binary;
	}

	protected static DdModBinaryBuilder CreateWithBindingAndTexture(string shaderName, string textureName)
	{
		DdModBinaryBuilder binary = new();
		binary.AddObjectBinding(shaderName, [.. "shader = \"boid\""u8]);
		binary.AddTexture(textureName, File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png")));
		return binary;
	}
}
