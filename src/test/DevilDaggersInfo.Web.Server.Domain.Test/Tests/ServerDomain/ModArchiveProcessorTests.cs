using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal abstract class ModArchiveProcessorTests
{
	protected ModArchiveProcessorTests()
	{
		Cache = new ModArchiveCache(FileSystem);
		Processor = new ModArchiveProcessor(FileSystem, Cache, Substitute.For<ILogger<ModArchiveProcessor>>());
	}

	protected InMemoryFileSystem FileSystem { get; } = new();
	protected ModArchiveCache Cache { get; }
	protected ModArchiveProcessor Processor { get; }

	protected async Task<ZipArchive> OpenArchiveAsync(string modName)
	{
		byte[]? bytes = await FileSystem.ReadAllBytesAsync(DataSubDirectory.Mods, ModArchiveAccessor.GetModArchiveFileName(modName));
		await Assert.That(bytes).IsNotNull().Because($"Mod archive for '{modName}' was not stored.");

		// ! Asserted above.
		return new ZipArchive(new MemoryStream(bytes!), ZipArchiveMode.Read);
	}

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
