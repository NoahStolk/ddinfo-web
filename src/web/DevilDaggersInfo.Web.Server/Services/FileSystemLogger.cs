using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Shared.Utils;

namespace DevilDaggersInfo.Web.Server.Services;

public class FileSystemLogger : IFileSystemLogger
{
	private readonly List<FileSystemInformation> _logs = new();

	private readonly IFileSystemService _fileSystemService;

	public FileSystemLogger(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public void DirectoryDeleted(string directoryPath)
	{
		_logs.Add(new($"Directory {_fileSystemService.FormatPath(directoryPath)} was deleted because removal was requested.", FileSystemInformationType.Delete));
	}

	public void DirectoryMoved(string directoryPath, string newDirectoryPath)
	{
		_logs.Add(new($"Directory {_fileSystemService.FormatPath(directoryPath)} was moved to {_fileSystemService.FormatPath(newDirectoryPath)}.", FileSystemInformationType.Move));
	}

	public void DirectoryNotDeletedBecauseNotFound(string directoryPath)
	{
		_logs.Add(new($"Directory {_fileSystemService.FormatPath(directoryPath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
	}

	public void DirectoryNotMovedBecauseNotFound(string directoryPath)
	{
		_logs.Add(new($"Directory {_fileSystemService.FormatPath(directoryPath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
	}

	public void FileAdded(string filePath)
	{
		_logs.Add(new($"File {_fileSystemService.FormatPath(filePath)} was added.", FileSystemInformationType.Add));
	}

	public void FileDeleted(string filePath)
	{
		_logs.Add(new($"File {_fileSystemService.FormatPath(filePath)} was deleted because removal was requested.", FileSystemInformationType.Delete));
	}

	public void FileNotAddedBecauseInvalid(string filePath, string reason)
	{
		_logs.Add(new($"File {_fileSystemService.FormatPath(filePath)} was skipped because: {reason}", FileSystemInformationType.Skip));
	}

	public void FileNotDeletedBecauseNotFound(string filePath)
	{
		_logs.Add(new($"File {_fileSystemService.FormatPath(filePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
	}

	public void ModArchiveAdded(string filePath, int fileSize, string modName, Dictionary<BinaryName, byte[]> binaries)
{
		_logs.Add(new($"File {_fileSystemService.FormatPath(filePath)} (`{FileSizeUtils.Format(fileSize)}`) with {(binaries.Count == 1 ? "binary" : "binaries")} was added. Binaries:\n{string.Join("\n", binaries.Keys.Select(bn => $"- `{bn.ToFullName(modName)}`"))}", FileSystemInformationType.Add));
	}

	public List<FileSystemInformation> Flush()
	{
		return _logs;
	}
}
