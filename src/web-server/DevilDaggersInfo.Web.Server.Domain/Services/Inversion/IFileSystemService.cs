using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface IFileSystemService
{
	string[] TryGetFiles(DataSubDirectory subDirectory);

	string GetLeaderboardHistoryPathFromDate(DateTime dateTime);

	string GetPath(DataSubDirectory subDirectory);

	long GetDirectorySize(string path);

	string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	bool DeleteFileIfExists(string path);

	bool FileExists(string path);

	bool DeleteDirectoryIfExists(string path, bool recursive);

	bool DirectoryExists(string path);

	void MoveDirectory(string sourcePath, string destinationPath);

	void CreateDirectory(string path);

	string[] GetFiles(string path);

	string[] GetFiles(string path, string searchPattern);

	byte[] ReadAllBytes(string path);

	Task<byte[]> ReadAllBytesAsync(string path);

	Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);

	void WriteAllBytes(string path, byte[] bytes);

	Task WriteAllBytesAsync(string path, byte[] bytes);

	Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken);

	void MoveFile(string sourcePath, string destinationPath);

	string ReadAllText(string path);

	Task<string> ReadAllTextAsync(string path);

	Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);

	void WriteAllText(string path, string text);

	Task WriteAllTextAsync(string path, string text);

	Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken);

	Task CreateZipFileAsync(string zipFilePath, Func<ZipArchive, Task> func);

	// TODO: Add ZipFile methods.

	// TODO: Add FileInfo methods.
}
