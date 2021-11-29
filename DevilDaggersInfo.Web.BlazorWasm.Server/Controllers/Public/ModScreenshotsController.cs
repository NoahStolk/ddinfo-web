namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/mod-screenshots")]
[ApiController]
public class ModScreenshotsController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;

	public ModScreenshotsController(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	[HttpGet]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult GetScreenshotByFilePath(string modName, string fileName)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName, fileName);
		if (!IoFile.Exists(path))
			return NotFound();

		return File(IoFile.ReadAllBytes(path), "image/png");
	}
}
