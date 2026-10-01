using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/mod-screenshots")]
[ApiController]
public sealed class ModScreenshotsController(IFileSystemService fileSystemService) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetScreenshotByFilePath(string modName, string fileName)
	{
		// These come from the query string, so they must not be able to point outside the mod screenshots directory.
		if (!IsValidName(modName) || !IsValidName(fileName))
			return NotFound();

		string screenshotsDirectory = Path.GetFullPath(fileSystemService.GetPath(DataSubDirectory.ModScreenshots));
		string path = Path.GetFullPath(Path.Combine(screenshotsDirectory, modName, fileName));
		if (!path.StartsWith(screenshotsDirectory + Path.DirectorySeparatorChar, StringComparison.Ordinal))
			return NotFound();

		if (!IoFile.Exists(path))
			return NotFound();

		return File(await IoFile.ReadAllBytesAsync(path), "image/png");

		static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && name != "." && name != ".." && name.IndexOfAny(['/', '\\', ':']) == -1;
	}
}
