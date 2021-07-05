﻿using DevilDaggersWebsite.Api.Attributes;
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

		[HttpPut("{id}")]
		//[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult EditSpawnset(int id, EditSpawnset editSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
				return BadRequest($"Player with ID {editSpawnset.PlayerId} does not exist.");

			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			// Do not update LastUpdated. Update this value when updating the file only.
			spawnset.HtmlDescription = editSpawnset.HtmlDescription;
			spawnset.IsPractice = editSpawnset.IsPractice;
			spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
			spawnset.Name = editSpawnset.Name;
			spawnset.PlayerId = editSpawnset.PlayerId;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		//[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult DeleteSpawnset(int id)
		{
			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			if (_dbContext.CustomLeaderboards.Any(ce => ce.SpawnsetFileId == id))
				return BadRequest("Spawnset with custom leaderboard cannot be deleted.");

			_dbContext.SpawnsetFiles.Remove(spawnset);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
