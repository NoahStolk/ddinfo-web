using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.ModScreenshots;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/mod-screenshots")]
[ApiController]
public class ModScreenshotsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly AuditLogger _auditLogger;

	public ModScreenshotsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_auditLogger = auditLogger;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddModScreenshot(AddModScreenshot addModScreenshot)
	{
		if (!_dbContext.Mods.Any(m => m.Name == addModScreenshot.ModName))
			return BadRequest($"Mod with name '{addModScreenshot.ModName}' does not exist.");

		string directory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), addModScreenshot.ModName);
		Directory.CreateDirectory(directory);
		DirectoryInfo directoryInfo = new(directory);
		int screenshots = directoryInfo.EnumerateFiles().Count();
		if (screenshots >= ModScreenshotConstants.MaxScreenshots)
			return BadRequest($"This mod already contains {screenshots} screenshots.");

		string path = Path.Combine(directory, $"{screenshots:00}.png");
		IoFile.WriteAllBytes(path, addModScreenshot.FileContents);

		await _auditLogger.LogFileSystemInformation(new() { new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add) });

		return Ok();
	}

	[HttpPost] // Not actually DELETE since that needs a route parameter.
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> DeleteModScreenshot(DeleteModScreenshot deleteModScreenshot)
	{
		if (!_dbContext.Mods.Any(m => m.Name == deleteModScreenshot.ModName))
			return BadRequest($"Mod with name '{deleteModScreenshot.ModName}' does not exist.");

		string directory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), deleteModScreenshot.ModName);
		string path = Path.Combine(directory, deleteModScreenshot.ScreenshotName);
		bool fileExists = IoFile.Exists(path);
		if (!fileExists)
			return BadRequest($"Screenshot with name {deleteModScreenshot.ScreenshotName} does not exist.");

		IoFile.Delete(path);

		await _auditLogger.LogFileSystemInformation(new() { new($"File {_fileSystemService.FormatPath(path)} was deleted.", FileSystemInformationType.Delete) });

		return Ok();
	}
}
