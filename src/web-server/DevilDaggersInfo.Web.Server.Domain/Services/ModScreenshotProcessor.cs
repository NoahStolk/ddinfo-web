using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ModScreenshotProcessor
{
	private readonly IFileSystemService _fileSystemService;

	public ModScreenshotProcessor(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public void ProcessModScreenshotUpload(string modName, Dictionary<string, byte[]> screenshots)
	{
		if (screenshots.Count == 0)
			return;

		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		_fileSystemService.CreateDirectory(modScreenshotsDirectory);
		int i = 0;
		foreach (byte[] screenshotContents in screenshots.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
		{
			if (!PngFileUtils.HasValidPngHeader(screenshotContents))
				continue;

			string path;
			do
			{
				path = Path.Combine(modScreenshotsDirectory, $"{i:00}.png");
				i++;
			}
			while (_fileSystemService.FileExists(path));

			_fileSystemService.WriteAllBytes(path, screenshotContents);
		}
	}

	public void DeleteScreenshot(string modName, string screenshotFileName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		string screenshotFilePath = Path.Combine(screenshotsDirectory, screenshotFileName);
		_fileSystemService.DeleteFileIfExists(screenshotFilePath);
	}

	public void DeleteScreenshotsDirectory(string modName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		_fileSystemService.DeleteDirectoryIfExists(screenshotsDirectory, true);
	}

	public void MoveScreenshotsDirectory(string originalModName, string newModName)
	{
		if (originalModName == newModName)
			return;

		string screenshotsDirectory = _fileSystemService.GetPath(DataSubDirectory.ModScreenshots);
		string oldScreenshotsDirectory = Path.Combine(screenshotsDirectory, originalModName);
		if (_fileSystemService.DirectoryExists(oldScreenshotsDirectory))
		{
			string newScreenshotsDirectory = Path.Combine(screenshotsDirectory, newModName);
			_fileSystemService.MoveDirectory(oldScreenshotsDirectory, newScreenshotsDirectory);
		}
	}
}
