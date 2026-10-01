using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Utils;

internal sealed class InMemoryFileSystem : IFileSystem
{
	private readonly ConcurrentDictionary<(DataSubDirectory Directory, string Name), byte[]> _files = new();

	/// <summary>
	/// Adds or overwrites a file synchronously, for seeding test data in constructors.
	/// </summary>
	public void Seed(DataSubDirectory directory, string name, byte[] contents)
	{
		_files[(directory, name)] = contents;
	}

	public Task<bool> ExistsAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(_files.ContainsKey((directory, name)));
	}

	public Task<byte[]?> ReadAllBytesAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(_files.TryGetValue((directory, name), out byte[]? contents) ? contents.ToArray() : null);
	}

	public Task WriteAllBytesAsync(DataSubDirectory directory, string name, byte[] contents, CancellationToken cancellationToken = default)
	{
		_files[(directory, name)] = contents.ToArray();
		return Task.CompletedTask;
	}

	public Task<bool> DeleteAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(_files.TryRemove((directory, name), out _));
	}

	public Task<IReadOnlyList<FileEntry>> ListAsync(DataSubDirectory directory, string? prefix = null, CancellationToken cancellationToken = default)
	{
		List<FileEntry> entries =
		[
			.. _files
				.Where(kvp => kvp.Key.Directory == directory && (prefix == null || kvp.Key.Name.StartsWith(prefix, StringComparison.Ordinal)))
				.Select(kvp => new FileEntry(kvp.Key.Name, kvp.Value.Length))
				.OrderBy(e => e.Name, StringComparer.Ordinal),
		];
		return Task.FromResult<IReadOnlyList<FileEntry>>(entries);
	}
}
