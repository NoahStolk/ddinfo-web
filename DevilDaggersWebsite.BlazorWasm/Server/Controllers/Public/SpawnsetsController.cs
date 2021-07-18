using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetData;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Converters.Public;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Spawnsets;
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
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Public
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _environment;
		private readonly SpawnsetDataCache _spawnsetDataCache;

		public SpawnsetsController(ApplicationDbContext dbContext, IWebHostEnvironment environment, SpawnsetDataCache spawnsetDataCache)
		{
			_dbContext = dbContext;
			_environment = environment;
			_spawnsetDataCache = spawnsetDataCache;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddse)]
		public List<GetSpawnset> GetPublicSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Select(cl => cl.SpawnsetFileId)
				.ToList();

			IEnumerable<SpawnsetFile> query = _dbContext.SpawnsetFiles.AsNoTracking().Include(sf => sf.Player);

			if (!string.IsNullOrWhiteSpace(authorFilter))
				query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));

			if (!string.IsNullOrWhiteSpace(nameFilter))
				query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

			return query
				.Where(sf => Io.File.Exists(Path.Combine(_environment.WebRootPath, "spawnsets", sf.Name)))
				.Select(sf => Map(sf))
				.ToList();

			GetSpawnset Map(SpawnsetFile spawnsetFile)
			{
				SpawnsetData spawnsetData = _spawnsetDataCache.GetSpawnsetDataByFilePath(Path.Combine(_environment.WebRootPath, "spawnsets", spawnsetFile.Name));
				return spawnsetFile.ToGetSpawnsetPublic(spawnsetData, spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id));
			}
		}

		[HttpGet("{fileName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Ddse | EndpointConsumers.Website)]
		public ActionResult GetSpawnsetFile([Required] string fileName)
		{
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, "spawnsets", fileName)))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "spawnsets", fileName)), MediaTypeNames.Application.Octet, fileName);
		}
	}
}
