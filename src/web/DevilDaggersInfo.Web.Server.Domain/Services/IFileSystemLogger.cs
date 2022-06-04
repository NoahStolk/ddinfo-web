using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public interface IFileSystemLogger
{
	void FileAdded(string filePath);

	void FileNotAddedBecauseInvalid(string filePath, string reason);

	void FileDeleted(string filePath);

	void FileNotDeletedBecauseNotFound(string filePath);

	void DirectoryDeleted(string directoryPath);

	void DirectoryNotDeletedBecauseNotFound(string directoryPath);

	void DirectoryMoved(string directoryPath, string newDirectoryPath);

	void DirectoryNotMovedBecauseNotFound(string directoryPath);

	void ModArchiveAdded(string filePath, int fileSize, string modName, Dictionary<BinaryName, byte[]> binaries);

	List<FileSystemInformation> Flush();
}
