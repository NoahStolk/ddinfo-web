using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class ModScreenshotProcessorTests
{
	private static readonly byte[] _png = File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png"));

	private readonly InMemoryFileSystem _fileSystem = new();
	private readonly ModScreenshotProcessor _processor;

	public ModScreenshotProcessorTests()
	{
		_processor = new ModScreenshotProcessor(_fileSystem);
	}

	[Test]
	public async Task Upload_SkipsExistingIndicesAndInvalidPngs()
	{
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod/01.png", _png);

		await _processor.ProcessModScreenshotUploadAsync("mod", new Dictionary<string, byte[]>
		{
			["a"] = _png,
			["b"] = [1, 2, 3],
			["c"] = _png,
		});

		await Assert.That(await GetNamesAsync()).IsEquivalentTo(["mod/00.png", "mod/01.png", "mod/02.png"], CollectionOrdering.Matching);
	}

	[Test]
	public async Task Move_RenamesAllScreenshotsOfOnlyThatMod()
	{
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod/00.png", _png);
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod/01.png", _png);
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod-other/00.png", _png);

		await _processor.MoveScreenshotsAsync("mod", "renamed");

		await Assert.That(await GetNamesAsync()).IsEquivalentTo(["mod-other/00.png", "renamed/00.png", "renamed/01.png"], CollectionOrdering.Matching);
		await Assert.That(await _fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, "renamed/00.png")).IsEquivalentTo(_png, CollectionOrdering.Matching);
	}

	[Test]
	public async Task Delete_RemovesOnlyThatMod()
	{
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod/00.png", _png);
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod/01.png", _png);
		_fileSystem.Seed(DataSubDirectory.ModScreenshots, "mod-other/00.png", _png);

		await _processor.DeleteScreenshotAsync("mod", "01.png");
		await Assert.That(await GetNamesAsync()).IsEquivalentTo(["mod-other/00.png", "mod/00.png"], CollectionOrdering.Matching);

		await _processor.DeleteScreenshotsAsync("mod");
		await Assert.That(await GetNamesAsync()).IsEquivalentTo(["mod-other/00.png"], CollectionOrdering.Matching);
	}

	private async Task<List<string>> GetNamesAsync()
	{
		return [.. (await _fileSystem.ListAsync(DataSubDirectory.ModScreenshots)).Select(f => f.Name)];
	}
}
