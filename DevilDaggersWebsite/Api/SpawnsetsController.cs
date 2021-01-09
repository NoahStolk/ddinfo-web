using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly SpawnsetHelper _spawnsetHelper;
		private readonly IWebHostEnvironment _env;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment env)
		{
			_spawnsetHelper = spawnsetHelper;
			_env = env;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public List<SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
			=> _spawnsetHelper.GetSpawnsets(authorFilter, nameFilter);

		[HttpGet("{fileName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetSpawnsetFile([Required] string fileName)
		{
			if (!Io.File.Exists(Path.Combine(_env.WebRootPath, "spawnsets", fileName)))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

			return File(Io.File.ReadAllBytes(Path.Combine(_env.WebRootPath, "spawnsets", fileName)), MediaTypeNames.Application.Octet, fileName);
		}
	}
}