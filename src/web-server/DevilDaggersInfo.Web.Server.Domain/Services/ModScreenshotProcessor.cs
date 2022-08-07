using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ModScreenshotProcessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly IFileSystemLogger _fileSystemLogger;

	public ModScreenshotProcessor(IFileSystemService fileSystemService, IFileSystemLogger fileSystemLogger)
	{
		_fileSystemService = fileSystemService;
		_fileSystemLogger = fileSystemLogger;
	}

	public void ProcessModScreenshotUpload(string modName, Dictionary<string, byte[]> screenshots)
	{
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		Directory.CreateDirectory(modScreenshotsDirectory);
		int i = 0;
		foreach (KeyValuePair<string, byte[]> kvp in screenshots.OrderBy(kvp => kvp.Key))
		{
			if (!PngFileUtils.HasValidPngHeader(kvp.Value))
			{
				_fileSystemLogger.FileNotAddedBecauseInvalid(kvp.Key, "Invalid PNG file.");
				continue;
			}

			string path;
			do
			{
				path = Path.Combine(modScreenshotsDirectory, $"{i:00}.png");
				i++;
			}
			while (File.Exists(path));

			File.WriteAllBytes(path, kvp.Value);
			_fileSystemLogger.FileAdded(path);
		}
	}

	public void DeleteScreenshot(string modName, string screenshotFileName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		string screenshotFilePath = Path.Combine(screenshotsDirectory, screenshotFileName);
		if (File.Exists(screenshotFilePath))
		{
			File.Delete(screenshotFilePath);
			_fileSystemLogger.FileDeleted(screenshotFilePath);
		}
		else
		{
			_fileSystemLogger.FileNotDeletedBecauseNotFound(screenshotFilePath);
		}
	}

	public void DeleteScreenshotsDirectory(string modName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		if (Directory.Exists(screenshotsDirectory))
		{
			Directory.Delete(screenshotsDirectory, true);
			_fileSystemLogger.DirectoryDeleted(screenshotsDirectory);
		}
		else
		{
			_fileSystemLogger.DirectoryNotDeletedBecauseNotFound(screenshotsDirectory);
		}
	}

	public void MoveScreenshotsDirectory(string originalModName, string newModName)
	{
		if (originalModName == newModName)
			return;

		string screenshotsDirectory = _fileSystemService.GetPath(DataSubDirectory.ModScreenshots);
		string oldScreenshotsDirectory = Path.Combine(screenshotsDirectory, originalModName);
		if (Directory.Exists(oldScreenshotsDirectory))
		{
			string newScreenshotsDirectory = Path.Combine(screenshotsDirectory, newModName);
			Directory.Move(oldScreenshotsDirectory, newScreenshotsDirectory);
			_fileSystemLogger.DirectoryMoved(oldScreenshotsDirectory, newScreenshotsDirectory);
		}
		else
		{
			_fileSystemLogger.DirectoryNotMovedBecauseNotFound(oldScreenshotsDirectory);
		}
	}
}
