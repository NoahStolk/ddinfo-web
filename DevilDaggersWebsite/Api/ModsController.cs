using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/mods")]
	[ApiController]
	public class ModsController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		public ModsController(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;
		}

		[HttpGet("{modName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetModFile([Required] string modName)
		{
			AssetMod? assetMod = _dbContext.AssetMods.FirstOrDefault(am => am.Name == modName);
			if (assetMod == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Mod '{modName}' was not found." });

			string fileName = $"{assetMod.Name}.zip";
			string path = Path.Combine("mods", fileName);
			if (!Io.File.Exists(Path.Combine(_env.WebRootPath, path)))
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Mod file '{fileName}' does not exist." });

			return File(Io.File.ReadAllBytes(Path.Combine(_env.WebRootPath, path)), MediaTypeNames.Application.Zip, fileName);
		}

		// TODO: Implement endpoints for retrieving all ModData DTOs.
	}
}
