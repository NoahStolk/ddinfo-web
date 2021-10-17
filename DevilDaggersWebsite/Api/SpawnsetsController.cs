using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly SpawnsetHelper _spawnsetHelper;
		private readonly IWebHostEnvironment _environment;
		private readonly SpawnsetHashCache _spawnsetHashCache;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment environment, SpawnsetHashCache spawnsetHashCache)
		{
			_spawnsetHelper = spawnsetHelper;
			_environment = environment;
			_spawnsetHashCache = spawnsetHashCache;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[Obsolete("Use " + nameof(GetSpawnsetsForDdse) + " instead. This is still in use by DDSE 2.34.0.0.")]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public List<SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
			=> _spawnsetHelper.GetSpawnsets(authorFilter, nameFilter);

		[HttpGet("ddse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public List<SpawnsetFile> GetSpawnsetsForDdse(string? authorFilter = null, string? nameFilter = null)
			=> _spawnsetHelper.GetSpawnsets(authorFilter, nameFilter);

		[HttpGet("by-hash")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<SpawnsetFile>> GetSpawnsetByHash([FromQuery] byte[] hash)
		{
			SpawnsetHashCacheData? data = await _spawnsetHashCache.GetSpawnset(hash);
			if (data == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset with {nameof(hash)} '{hash.ByteArrayToHexString()}' was not found." });

			SpawnsetFile? dto = _spawnsetHelper.GetSpawnset(data.Name);
			if (dto == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset with {nameof(hash)} '{hash.ByteArrayToHexString()}' was not found." });

			return dto;
		}

		[HttpGet("hash")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<byte[]> GetSpawnsetHash([Required] string fileName)
		{
			string path = Path.Combine(_environment.WebRootPath, "spawnsets", fileName);
			if (!Io.File.Exists(path))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset with {nameof(fileName)} '{fileName}' was not found." });

			byte[] spawnsetBytes = Io.File.ReadAllBytes(path);
			return MD5.HashData(spawnsetBytes);
		}

		[HttpGet("{fileName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetSpawnsetFile([Required] string fileName)
		{
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, "spawnsets", fileName)))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "spawnsets", fileName)), MediaTypeNames.Application.Octet, fileName);
		}
	}
}
