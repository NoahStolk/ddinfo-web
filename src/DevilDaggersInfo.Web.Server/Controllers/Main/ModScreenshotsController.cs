using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/mod-screenshots")]
[ApiController]
public sealed class ModScreenshotsController(IFileSystem fileSystem) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetScreenshotByFilePath(string modName, string fileName)
	{
		// These come from the query string, so they must not be able to point outside the mod's screenshots.
		if (!IsValidName(modName) || !IsValidName(fileName))
			return NotFound();

		byte[]? contents = await fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, $"{modName}/{fileName}");
		if (contents == null)
			return NotFound();

		return File(contents, "image/png");

		static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && name != "." && name != ".." && name.IndexOfAny(['/', '\\', ':']) == -1;
	}
}
