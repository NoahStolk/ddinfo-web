using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Enums;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Server.Transients;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.ModScreenshots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/mod-screenshots")]
	[Authorize(Roles = Roles.Mods)]
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
			Io.File.WriteAllBytes(path, addModScreenshot.FileContents);

			await _auditLogger.LogFileSystemInformation(new() { new($"File '{_fileSystemService.GetRelevantDisplayPath(path)}' was added.", FileSystemInformationType.Add) });

			return Ok();
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> DeleteModScreenshot(DeleteModScreenshot deleteModScreenshot)
		{
			if (!_dbContext.Mods.Any(m => m.Name == deleteModScreenshot.ModName))
				return BadRequest($"Mod with name '{deleteModScreenshot.ModName}' does not exist.");

			string directory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), deleteModScreenshot.ModName);
			string path = Path.Combine(directory, deleteModScreenshot.ScreenshotName);
			bool fileExists = Io.File.Exists(path);
			if (!fileExists)
				return BadRequest($"Screenshot with name {deleteModScreenshot.ScreenshotName} does not exist.");

			Io.File.Delete(path);

			await _auditLogger.LogFileSystemInformation(new() { new($"File '{_fileSystemService.GetRelevantDisplayPath(path)}' was deleted.", FileSystemInformationType.Delete) });

			return Ok();
		}
	}
}
