using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

/// <summary>
/// Storage for all files the server reads and writes at runtime.
/// A file is identified by its <see cref="DataSubDirectory"/> and a name relative to it. Names use <c>/</c> as separator for nested files (for example <c>mod-name/00.png</c>) and never contain OS paths.
/// There are no directories; a directory exists only as a common prefix of file names.
/// </summary>
public interface IFileSystem
{
	Task<bool> ExistsAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default);

	/// <summary>
	/// Reads the entire file.
	/// </summary>
	/// <returns>The file contents, or <see langword="null"/> when the file does not exist.</returns>
	Task<byte[]?> ReadAllBytesAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates or overwrites the file.
	/// </summary>
	Task WriteAllBytesAsync(DataSubDirectory directory, string name, byte[] contents, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes the file if it exists.
	/// </summary>
	/// <returns>Whether the file existed.</returns>
	Task<bool> DeleteAsync(DataSubDirectory directory, string name, CancellationToken cancellationToken = default);

	/// <summary>
	/// Lists all files (including nested files) whose name starts with <paramref name="prefix"/>, ordered by name using ordinal comparison.
	/// </summary>
	Task<IReadOnlyList<FileEntry>> ListAsync(DataSubDirectory directory, string? prefix = null, CancellationToken cancellationToken = default);
}
