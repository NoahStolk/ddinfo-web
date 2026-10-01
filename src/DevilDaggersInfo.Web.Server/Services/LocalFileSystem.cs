using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

// This is the one class that is allowed to access the disk directly.
#pragma warning disable RS0030

namespace DevilDaggersInfo.Web.Server.Services;

/// <summary>
/// Stores files on the local disk under <c>{rootDirectory}/{DataSubDirectory}/{name}</c>.
/// </summary>
internal sealed class LocalFileSystem : IFileSystem
{
	private readonly string _rootDirectory;

	public LocalFileSystem(string rootDirectory)
	{
		_rootDirectory = Path.GetFullPath(rootDirectory);

		foreach (DataSubDirectory e in Enum.GetValues<DataSubDirectory>())
			Directory.CreateDirectory(GetDirectoryPath(e));
	}

	public Task<bool> ExistsAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(File.Exists(GetFilePath(directory, name)));
	}

	public async Task<byte[]?> ReadAllBytesAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		try
		{
			return await File.ReadAllBytesAsync(GetFilePath(directory, name), cancellationToken);
		}
		catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
		{
			return null;
		}
	}

	public async Task WriteAllBytesAsync(DataSubDirectory directory, string name, byte[] contents, CancellationToken cancellationToken = default)
	{
		string path = GetFilePath(directory, name);
		string? parentDirectoryPath = Path.GetDirectoryName(path);
		if (parentDirectoryPath != null)
			Directory.CreateDirectory(parentDirectoryPath);

		await File.WriteAllBytesAsync(path, contents, cancellationToken);
	}

	public Task<bool> DeleteAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		// File.Delete does not throw when the file does not exist, so check first to report whether it did.
		string path = GetFilePath(directory, name);
		if (!File.Exists(path))
			return Task.FromResult(false);

		File.Delete(path);
		return Task.FromResult(true);
	}

	public Task<IReadOnlyList<FileEntry>> ListAsync(DataSubDirectory directory, string? prefix = null, CancellationToken cancellationToken = default)
	{
		string directoryPath = GetDirectoryPath(directory);
		if (!Directory.Exists(directoryPath))
			return Task.FromResult<IReadOnlyList<FileEntry>>([]);

		// Directory.EnumerateFiles does not guarantee any order. NTFS happens to return entries sorted by name, but ext4 returns them in hash order, so sort explicitly.
		List<FileEntry> entries = [];
		foreach (FileInfo fileInfo in new DirectoryInfo(directoryPath).EnumerateFiles("*", SearchOption.AllDirectories))
		{
			string name = Path.GetRelativePath(directoryPath, fileInfo.FullName).Replace(Path.DirectorySeparatorChar, '/');
			if (prefix == null || name.StartsWith(prefix, StringComparison.Ordinal))
				entries.Add(new FileEntry(name, fileInfo.Length));
		}

		entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
		return Task.FromResult<IReadOnlyList<FileEntry>>(entries);
	}

	private string GetDirectoryPath(DataSubDirectory directory)
	{
		return Path.Combine(_rootDirectory, directory.ToString());
	}

	private string GetFilePath(DataSubDirectory directory, string name)
	{
		if (string.IsNullOrWhiteSpace(name) || name.Contains('\\', StringComparison.Ordinal) || Path.IsPathRooted(name))
			throw new ArgumentException($"Invalid file name '{name}'.", nameof(name));

		string directoryPath = GetDirectoryPath(directory);
		string path = Path.GetFullPath(Path.Combine([directoryPath, .. name.Split('/')]));
		if (!path.StartsWith(directoryPath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
			throw new ArgumentException($"File name '{name}' resolves outside of directory '{directory}'.", nameof(name));

		return path;
	}
}
