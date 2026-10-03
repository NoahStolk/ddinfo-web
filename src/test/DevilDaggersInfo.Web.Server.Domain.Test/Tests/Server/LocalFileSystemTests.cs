using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Services;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

internal sealed class LocalFileSystemTests : IDisposable
{
	private readonly string _rootDirectory = Path.Combine(Path.GetTempPath(), $"ddinfo-{Guid.NewGuid():N}");
	private readonly LocalFileSystem _fileSystem;

	public LocalFileSystemTests()
	{
		_fileSystem = new LocalFileSystem(_rootDirectory);
	}

	public void Dispose()
	{
		Directory.Delete(_rootDirectory, true);
	}

	[Test]
	public async Task CreatesAllSubDirectories()
	{
		foreach (DataSubDirectory subDirectory in Enum.GetValues<DataSubDirectory>())
			await Assert.That(Directory.Exists(Path.Combine(_rootDirectory, subDirectory.ToString()))).IsTrue();
	}

	[Test]
	public async Task WriteReadDelete()
	{
		byte[] contents = [1, 2, 3];
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay", contents);

		await Assert.That(await _fileSystem.ExistsAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay")).IsTrue();
		await Assert.That(await _fileSystem.ReadAllBytesAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay")).IsEquivalentTo(contents, CollectionOrdering.Matching);
		await Assert.That(File.Exists(Path.Combine(_rootDirectory, "CustomEntryReplays", "1.ddreplay"))).IsTrue();

		await Assert.That(await _fileSystem.DeleteAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay")).IsTrue();
		await Assert.That(await _fileSystem.ExistsAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay")).IsFalse();
	}

	[Test]
	public async Task MissingFile()
	{
		await Assert.That(await _fileSystem.ExistsAsync(DataSubDirectory.Mods, "missing.zip")).IsFalse();
		await Assert.That(await _fileSystem.ReadAllBytesAsync(DataSubDirectory.Mods, "missing.zip")).IsNull();
		await Assert.That(await _fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, "missing/00.png")).IsNull();
		await Assert.That(await _fileSystem.DeleteAsync(DataSubDirectory.Mods, "missing.zip")).IsFalse();
	}

	[Test]
	public async Task NestedFiles()
	{
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, "mod/00.png", [1]);

		await Assert.That(File.Exists(Path.Combine(_rootDirectory, "ModScreenshots", "mod", "00.png"))).IsTrue();
		await Assert.That(await _fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, "mod/00.png")).IsEquivalentTo(new byte[] { 1 }, CollectionOrdering.Matching);
	}

	[Test]
	public async Task ListIsOrderedAndFilteredByPrefix()
	{
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, "b/01.png", [1, 2]);
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, "a/00.png", [1]);
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, "b/00.png", [1, 2, 3]);
		await _fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, "ab/00.png", [1]);

		IReadOnlyList<FileEntry> all = await _fileSystem.ListAsync(DataSubDirectory.ModScreenshots);
		await Assert.That(all.Select(e => e.Name)).IsEquivalentTo(["a/00.png", "ab/00.png", "b/00.png", "b/01.png"], CollectionOrdering.Matching);

		IReadOnlyList<FileEntry> b = await _fileSystem.ListAsync(DataSubDirectory.ModScreenshots, "b/");
		await Assert.That(b).IsEquivalentTo([new FileEntry("b/00.png", 3), new FileEntry("b/01.png", 2)], CollectionOrdering.Matching);

		await Assert.That(await _fileSystem.ListAsync(DataSubDirectory.Mods)).IsEmpty();
	}

	[Test]
	[Arguments("../Mods/x.zip")]
	[Arguments("mod/../../Mods/x.zip")]
	[Arguments("..")]
	[Arguments("a\\b")]
	[Arguments("/etc/passwd")]
	[Arguments("")]
	public async Task RejectsInvalidNames(string name)
	{
		await Assert.That(async () => await _fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, name)).Throws<ArgumentException>();
	}
}
