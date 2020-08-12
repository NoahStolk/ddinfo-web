using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

		[HttpGet("spawnset-path/by-file-name")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<string> GetSpawnsetPath([Required] string fileName)
		{
			if (!Io.File.Exists(Path.Combine(env.WebRootPath, "spawnsets", fileName)))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

			return Path.Combine("spawnsets", fileName);
		}

		[HttpGet]
		[ProducesResponseType(200)]
		public List<SpawnsetFile> GetSpawnsets(string searchAuthor, string searchName)
			=> SpawnsetUtils.GetSpawnsets(env, searchAuthor, searchName);
	}
}