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
		Directory.CreateDirectory(modScreenshotsDirectory);
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
			while (File.Exists(path));

			File.WriteAllBytes(path, screenshotContents);
		}
	}

	public void DeleteScreenshot(string modName, string screenshotFileName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		string screenshotFilePath = Path.Combine(screenshotsDirectory, screenshotFileName);
		if (File.Exists(screenshotFilePath))
			File.Delete(screenshotFilePath);
	}

	public void DeleteScreenshotsDirectory(string modName)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		if (Directory.Exists(screenshotsDirectory))
			Directory.Delete(screenshotsDirectory, true);
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
		}
	}
}
