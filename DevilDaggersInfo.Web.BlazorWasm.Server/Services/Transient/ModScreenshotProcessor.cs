using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Transient;

public class ModScreenshotProcessor
{
	private readonly IFileSystemService _fileSystemService;

	public ModScreenshotProcessor(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public void ProcessModScreenshotUpload(string modName, Dictionary<string, byte[]> screenshots, List<FileSystemInformation> fileSystemInformation)
	{
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		Directory.CreateDirectory(modScreenshotsDirectory);
		int i = 0;
		foreach (KeyValuePair<string, byte[]> kvp in screenshots.OrderBy(kvp => kvp.Key))
		{
			if (!PngFileUtils.HasValidPngHeader(kvp.Value))
			{
				fileSystemInformation.Add(new($"File {kvp.Key} was skipped because it is not a valid PNG file.", FileSystemInformationType.Skip));
				continue;
			}

			string path;
			do
			{
				path = Path.Combine(modScreenshotsDirectory, $"{i:00}.png");
				i++;
			}
			while (File.Exists(path));

			IoFile.WriteAllBytes(path, kvp.Value);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add));
		}
	}

	public void DeleteScreenshot(string modName, string screenshotFileName, List<FileSystemInformation> fileSystemInformation)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		string screenshotFilePath = Path.Combine(screenshotsDirectory, screenshotFileName);
		if (File.Exists(screenshotFilePath))
		{
			File.Delete(screenshotFilePath);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotFilePath)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotFilePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}
	}

	public void DeleteScreenshotsDirectory(string modName, List<FileSystemInformation> fileSystemInformation)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		if (Directory.Exists(screenshotsDirectory))
		{
			Directory.Delete(screenshotsDirectory, true);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}
	}

	public void MoveScreenshotsDirectory(string originalModName, string newModName, List<FileSystemInformation> fileSystemInformation)
	{
		if (originalModName == newModName)
			return;

		string screenshotsDirectory = _fileSystemService.GetPath(DataSubDirectory.ModScreenshots);
		string oldScreenshotsDirectory = Path.Combine(screenshotsDirectory, originalModName);
		if (Directory.Exists(oldScreenshotsDirectory))
		{
			string newScreenshotsDirectory = Path.Combine(screenshotsDirectory, newModName);
			Directory.Move(oldScreenshotsDirectory, newScreenshotsDirectory);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was moved to {_fileSystemService.FormatPath(newScreenshotsDirectory)}.", FileSystemInformationType.Move));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}
	}
}
