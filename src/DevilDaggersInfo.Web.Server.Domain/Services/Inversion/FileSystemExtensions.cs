using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public static class FileSystemExtensions
{
	/// <returns>The UTF-8 decoded file contents, or <see langword="null"/> when the file does not exist.</returns>
	public static async Task<string?> ReadAllTextAsync(this IFileSystem fileSystem, DataSubDirectory directory, string name, CancellationToken cancellationToken = default)
	{
		byte[]? bytes = await fileSystem.ReadAllBytesAsync(directory, name, cancellationToken);
		return bytes == null ? null : Encoding.UTF8.GetString(bytes);
	}

	public static async Task WriteAllTextAsync(this IFileSystem fileSystem, DataSubDirectory directory, string name, string text, CancellationToken cancellationToken = default)
	{
		await fileSystem.WriteAllBytesAsync(directory, name, Encoding.UTF8.GetBytes(text), cancellationToken);
	}
}
