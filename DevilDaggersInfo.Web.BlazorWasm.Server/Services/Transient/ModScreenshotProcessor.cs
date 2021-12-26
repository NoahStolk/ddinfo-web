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

			string path = Path.Combine(modScreenshotsDirectory, $"{i:00}.png");
			IoFile.WriteAllBytes(path, kvp.Value);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add));

			i++;
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
}
