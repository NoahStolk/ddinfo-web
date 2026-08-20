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
		string path = Path.Combine(fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName, fileName);
		if (!IoFile.Exists(path))
			return NotFound();

		return File(await IoFile.ReadAllBytesAsync(path), "image/png");
	}
}
