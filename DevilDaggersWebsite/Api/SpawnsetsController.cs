using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
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
		private readonly Entities.ApplicationDbContext _dbContext;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment environment, SpawnsetHashCache spawnsetHashCache, Entities.ApplicationDbContext dbContext)
		{
			_spawnsetHelper = spawnsetHelper;
			_environment = environment;
			_spawnsetHashCache = spawnsetHashCache;
			_dbContext = dbContext;
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
		public async Task<ActionResult<SpawnsetForDdcl>> GetSpawnsetByHash([FromQuery] byte[] hash)
		{
			SpawnsetHashCacheData? data = await _spawnsetHashCache.GetSpawnset(hash);
			if (data == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset with {nameof(hash)} '{hash.ByteArrayToHexString()}' was not found." });

			Entities.SpawnsetFile? spawnset = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.Include(s => s.Player)
				.FirstOrDefault(s => s.Name == data.Name);
			if (spawnset == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset with {nameof(hash)} '{hash.ByteArrayToHexString()}' was not found." });

			Entities.CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.FirstOrDefault(cl => cl.SpawnsetFileId == spawnset.Id);

			var customEntries = customLeaderboard == null ? null : _dbContext.CustomEntries
				.AsNoTracking()
				.Select(ce => new { ce.Id, ce.CustomLeaderboardId, ce.Time })
				.Where(ce => ce.CustomLeaderboardId == customLeaderboard.Id)
				.ToList();

			return new SpawnsetForDdcl
			{
				AuthorName = spawnset.Player.PlayerName,
				CustomLeaderboard = customLeaderboard == null ? null : new SpawnsetCustomLeaderboardForDdcl
				{
					CustomLeaderboardId = customLeaderboard.Id,
					CustomEntries = customEntries?.ConvertAll(ce => new SpawnsetCustomEntryForDdcl
					{
						HasReplay = false,
						CustomEntryId = ce.Id,
						Time = ce.Time,
					}) ?? new(),
				},
				SpawnsetId = spawnset.Id,
				Name = spawnset.Name,
			};
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
