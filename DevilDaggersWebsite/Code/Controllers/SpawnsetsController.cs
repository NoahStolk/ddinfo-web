using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly IWebHostEnvironment env;

		public SpawnsetsController(IWebHostEnvironment env)
		{
			this.env = env;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		public List<SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
			=> SpawnsetUtils.GetSpawnsets(env, authorFilter, nameFilter);

		[HttpGet("{fileName}/file")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public ActionResult GetSpawnsetFile([Required] string fileName)
		{
			if (!Io.File.Exists(Path.Combine(env.WebRootPath, "spawnsets", fileName)))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

			return File(Io.File.ReadAllBytes(Path.Combine(env.WebRootPath, "spawnsets", fileName)), MediaTypeNames.Application.Octet, fileName);
		}
	}
}