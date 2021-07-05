﻿using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Dto.CustomLeaderboards;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _environment;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment environment)
		{
			_dbContext = dbContext;
			_environment = environment;
		}

		[HttpGet]
		[Authorize(Policies.CustomLeaderboardsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<List<GetCustomLeaderboard>> GetCustomLeaderboards()
		{
			return _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.Select(cl => cl.ToDto())
				.ToList();
		}

		[HttpGet("{id}")]
		[Authorize(Policies.CustomLeaderboardsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<GetCustomLeaderboard> GetCustomLeaderboard(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.FirstOrDefault(cl => cl.Id == id);

			if (customLeaderboard == null)
				return NotFound($"Leaderboard with ID '{id}' was not found.");

			return customLeaderboard.ToDto();
		}

		[HttpPost]
		[Authorize(Policies.CustomLeaderboardsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
		{
			if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetFileId == addCustomLeaderboard.SpawnsetFileId))
				return BadRequest("A leaderboard for this spawnset already exists.");

			if (addCustomLeaderboard.Category.IsAscending())
			{
				if (addCustomLeaderboard.TimeLeviathan >= addCustomLeaderboard.TimeDevil)
					return BadRequest("For ascending leaderboards, Leviathan time must be smaller than Devil time.");
				if (addCustomLeaderboard.TimeDevil >= addCustomLeaderboard.TimeGolden)
					return BadRequest("For ascending leaderboards, Devil time must be smaller than Golden time.");
				if (addCustomLeaderboard.TimeGolden >= addCustomLeaderboard.TimeSilver)
					return BadRequest("For ascending leaderboards, Golden time must be smaller than Silver time.");
				if (addCustomLeaderboard.TimeSilver >= addCustomLeaderboard.TimeBronze)
					return BadRequest("For ascending leaderboards, Silver time must be smaller than Bronze time.");
			}
			else
			{
				if (addCustomLeaderboard.TimeLeviathan <= addCustomLeaderboard.TimeDevil)
					return BadRequest("For descending leaderboards, Leviathan time must be greater than Devil time.");
				if (addCustomLeaderboard.TimeDevil <= addCustomLeaderboard.TimeGolden)
					return BadRequest("For descending leaderboards, Devil time must be greater than Golden time.");
				if (addCustomLeaderboard.TimeGolden <= addCustomLeaderboard.TimeSilver)
					return BadRequest("For descending leaderboards, Golden time must be greater than Silver time.");
				if (addCustomLeaderboard.TimeSilver <= addCustomLeaderboard.TimeBronze)
					return BadRequest("For descending leaderboards, Silver time must be greater than Bronze time.");
			}

			var spawnsetFile = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.Select(sf => new { sf.Id, sf.Name })
				.FirstOrDefault(sf => sf.Id == addCustomLeaderboard.SpawnsetFileId);
			if (spawnsetFile == null)
				return BadRequest($"Spawnset with ID '{addCustomLeaderboard.SpawnsetFileId}' does not exist.");

			if (!Spawnset.TryParse(System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "spawnsets", spawnsetFile.Name)), out Spawnset spawnset))
				throw new($"Could not parse survival file '{spawnsetFile.Name}'. Please review the file. Also review how this file ended up in the 'spawnsets' directory, as it is not possible to upload non-survival files from within the Admin pages.");

			if (addCustomLeaderboard.Category == CustomLeaderboardCategory.TimeAttack && spawnset.GameMode != GameMode.TimeAttack
			 || addCustomLeaderboard.Category != CustomLeaderboardCategory.TimeAttack && spawnset.GameMode == GameMode.TimeAttack)
			{
				return BadRequest($"Spawnset game mode is '{spawnset.GameMode}' while custom leaderboard category is '{addCustomLeaderboard.Category}'.");
			}

			if (spawnset.TimerStart != 0)
				return BadRequest("Cannot create a leaderboard for spawnset that uses the TimerStart value. This value is meant for practice and it is confusing to use it with custom leaderboards, as custom leaderboards always use the 'actual' timer value.");

			CustomLeaderboard customLeaderboard = new()
			{
				DateCreated = DateTime.UtcNow,
				SpawnsetFileId = addCustomLeaderboard.SpawnsetFileId,
				Category = addCustomLeaderboard.Category,
				TimeBronze = addCustomLeaderboard.TimeBronze,
				TimeSilver = addCustomLeaderboard.TimeSilver,
				TimeGolden = addCustomLeaderboard.TimeGolden,
				TimeDevil = addCustomLeaderboard.TimeDevil,
				TimeLeviathan = addCustomLeaderboard.TimeLeviathan,
				IsArchived = addCustomLeaderboard.IsArchived,
			};
			_dbContext.CustomLeaderboards.Add(customLeaderboard);
			_dbContext.SaveChanges();

			return Ok(customLeaderboard.Id);
		}

		[HttpPut("{id}")]
		[Authorize(Policies.CustomLeaderboardsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult EditCustomLeaderboard(int id, EditCustomLeaderboard editCustomLeaderboard)
		{
			if (editCustomLeaderboard.Category.IsAscending())
			{
				if (editCustomLeaderboard.TimeLeviathan >= editCustomLeaderboard.TimeDevil)
					return BadRequest("For ascending leaderboards, Leviathan time must be smaller than Devil time.");
				if (editCustomLeaderboard.TimeDevil >= editCustomLeaderboard.TimeGolden)
					return BadRequest("For ascending leaderboards, Devil time must be smaller than Golden time.");
				if (editCustomLeaderboard.TimeGolden >= editCustomLeaderboard.TimeSilver)
					return BadRequest("For ascending leaderboards, Golden time must be smaller than Silver time.");
				if (editCustomLeaderboard.TimeSilver >= editCustomLeaderboard.TimeBronze)
					return BadRequest("For ascending leaderboards, Silver time must be smaller than Bronze time.");
			}
			else
			{
				if (editCustomLeaderboard.TimeLeviathan <= editCustomLeaderboard.TimeDevil)
					return BadRequest("For descending leaderboards, Leviathan time must be greater than Devil time.");
				if (editCustomLeaderboard.TimeDevil <= editCustomLeaderboard.TimeGolden)
					return BadRequest("For descending leaderboards, Devil time must be greater than Golden time.");
				if (editCustomLeaderboard.TimeGolden <= editCustomLeaderboard.TimeSilver)
					return BadRequest("For descending leaderboards, Golden time must be greater than Silver time.");
				if (editCustomLeaderboard.TimeSilver <= editCustomLeaderboard.TimeBronze)
					return BadRequest("For descending leaderboards, Silver time must be greater than Bronze time.");
			}

			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
			if (customLeaderboard == null)
				return NotFound();

			var spawnsetFile = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.Select(sf => new { sf.Id, sf.Name })
				.FirstOrDefault(sf => sf.Id == customLeaderboard.SpawnsetFileId);
			if (spawnsetFile == null)
				return BadRequest($"Spawnset with ID '{customLeaderboard.SpawnsetFileId}' does not exist.");

			if (!Spawnset.TryParse(System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "spawnsets", spawnsetFile.Name)), out Spawnset spawnset))
				throw new($"Could not parse survival file '{spawnsetFile.Name}'. Please review the file. Also review how this file ended up in the 'spawnsets' directory, as it is not possible to upload non-survival files from within the Admin pages.");

			if (editCustomLeaderboard.Category == CustomLeaderboardCategory.TimeAttack && spawnset.GameMode != GameMode.TimeAttack
			 || editCustomLeaderboard.Category != CustomLeaderboardCategory.TimeAttack && spawnset.GameMode == GameMode.TimeAttack)
			{
				return BadRequest($"Spawnset game mode is '{spawnset.GameMode}' while custom leaderboard category is '{editCustomLeaderboard.Category}'.");
			}

			if (spawnset.TimerStart != 0)
				return BadRequest("Cannot create a leaderboard for spawnset that uses the TimerStart value. This value is meant for practice and it is confusing to use it with custom leaderboards, as custom leaderboards always use the 'actual' timer value.");

			customLeaderboard.Category = editCustomLeaderboard.Category;
			customLeaderboard.TimeBronze = editCustomLeaderboard.TimeBronze;
			customLeaderboard.TimeSilver = editCustomLeaderboard.TimeSilver;
			customLeaderboard.TimeGolden = editCustomLeaderboard.TimeGolden;
			customLeaderboard.TimeDevil = editCustomLeaderboard.TimeDevil;
			customLeaderboard.TimeLeviathan = editCustomLeaderboard.TimeLeviathan;
			customLeaderboard.IsArchived = editCustomLeaderboard.IsArchived;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Policies.CustomLeaderboardsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult DeleteSpawnset(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
			if (customLeaderboard == null)
				return NotFound();

			if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
				return BadRequest("Custom leaderboard with scores cannot be deleted.");

			_dbContext.CustomLeaderboards.Remove(customLeaderboard);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
