using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Dto.Spawnsets;
using DevilDaggersWebsite.Entities;
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
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly SpawnsetHelper _spawnsetHelper;
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment environment, ApplicationDbContext dbContext)
		{
			_spawnsetHelper = spawnsetHelper;
			_environment = environment;
			_dbContext = dbContext;
		}

		// TODO: Re-route to /public.
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddse)]
		public List<GetPublicSpawnset> GetPublicSpawnsets(string? authorFilter = null, string? nameFilter = null)
			=> _spawnsetHelper.GetSpawnsets(authorFilter, nameFilter);

		// TODO: Remove private.
		[HttpGet("private")]
		//[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public List<GetSpawnset> GetSpawnsets()
		{
			List<SpawnsetFile> spawnsetFiles = _dbContext.SpawnsetFiles.AsNoTracking().ToList();

			return spawnsetFiles.ConvertAll(sf => new GetSpawnset
			{
				Id = sf.Id,
				PlayerId = sf.PlayerId,
				Name = sf.Name,
				MaxDisplayWaves = sf.MaxDisplayWaves,
				HtmlDescription = sf.HtmlDescription,
				LastUpdated = sf.LastUpdated,
				IsPractice = sf.IsPractice,
			});
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

		[HttpPost]
		//[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult AddSpawnset(AddSpawnset addSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
				return BadRequest($"Player with ID {addSpawnset.PlayerId} does not exist.");

			SpawnsetFile spawnset = new()
			{
				HtmlDescription = addSpawnset.HtmlDescription,
				IsPractice = addSpawnset.IsPractice,
				LastUpdated = DateTime.Now,
				MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
				Name = addSpawnset.Name,
				PlayerId = addSpawnset.PlayerId,
			};
			_dbContext.SpawnsetFiles.Add(spawnset);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
