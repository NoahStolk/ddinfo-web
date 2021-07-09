﻿using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Constants;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Spawnsets;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/spawnsets")]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly SpawnsetHelper _spawnsetHelper;
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly DiscordLogger _discordLogger;
		private readonly SpawnsetHashCache _spawnsetHashCache;
		private readonly AuditLogger _auditLogger;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment environment, ApplicationDbContext dbContext, DiscordLogger discordLogger, SpawnsetHashCache spawnsetHashCache, AuditLogger auditLogger)
		{
			_spawnsetHelper = spawnsetHelper;
			_environment = environment;
			_dbContext = dbContext;
			_discordLogger = discordLogger;
			_spawnsetHashCache = spawnsetHashCache;
			_auditLogger = auditLogger;
		}

		[HttpGet("admin")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<Page<GetSpawnset>> GetSpawnsets([Range(0, 1000)] int pageIndex = 0, [Range(5, 50)] int pageSize = 25)
		{
			List<SpawnsetFile> spawnsetFiles = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetSpawnset>
			{
				Results = spawnsetFiles.ConvertAll(sf => new GetSpawnset
				{
					Id = sf.Id,
					PlayerId = sf.PlayerId,
					Name = sf.Name,
					MaxDisplayWaves = sf.MaxDisplayWaves,
					HtmlDescription = sf.HtmlDescription,
					LastUpdated = sf.LastUpdated,
					IsPractice = sf.IsPractice,
				}),
				TotalResults = _dbContext.SpawnsetFiles.Count(),
			};
		}

		[HttpPost("admin")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
				return BadRequest($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == addSpawnset.Name))
				return BadRequest($"Spawnset with name '{addSpawnset.Name}' already exists.");

			SpawnsetFile spawnset = new()
			{
				HtmlDescription = addSpawnset.HtmlDescription,
				IsPractice = addSpawnset.IsPractice,
				MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
				Name = addSpawnset.Name,
				PlayerId = addSpawnset.PlayerId,
			};
			_dbContext.SpawnsetFiles.Add(spawnset);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addSpawnset, User, spawnset.Id);

			return Ok(spawnset.Id);
		}

		[HttpPut("admin/{id}")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> EditSpawnsetById(int id, EditSpawnset editSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
				return BadRequest($"Player with ID '{editSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == editSpawnset.Name))
				return BadRequest($"Spawnset with name '{editSpawnset.Name}' already exists.");

			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			EditSpawnset logDto = new()
			{
				HtmlDescription = spawnset.HtmlDescription,
				IsPractice = spawnset.IsPractice,
				MaxDisplayWaves = spawnset.MaxDisplayWaves,
				Name = spawnset.Name,
				PlayerId = spawnset.PlayerId,
			};

			// Do not update LastUpdated. Update this value when updating the file only.
			spawnset.HtmlDescription = editSpawnset.HtmlDescription;
			spawnset.IsPractice = editSpawnset.IsPractice;
			spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
			spawnset.Name = editSpawnset.Name;
			spawnset.PlayerId = editSpawnset.PlayerId;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editSpawnset, User, spawnset.Id);

			return Ok();
		}

		[HttpDelete("admin/{id}")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeleteSpawnsetById(int id)
		{
			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			if (_dbContext.CustomLeaderboards.Any(ce => ce.SpawnsetFileId == id))
				return BadRequest("Spawnset with custom leaderboard cannot be deleted.");

			_dbContext.SpawnsetFiles.Remove(spawnset);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(spawnset, User, spawnset.Id);

			return Ok();
		}

		[HttpPost("admin/upload-file")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> UploadSpawnsetFile(IFormFile file)
		{
			if (file == null)
				return BadRequest("No file.");

			if (file.Length > SpawnsetFileConstants.MaxFileSize)
				return BadRequest($"File too large ({file.Length:n0} / max {SpawnsetFileConstants.MaxFileSize:n0} bytes).");

			if (file.FileName.Length > SpawnsetFileConstants.MaxFileNameLength)
				return BadRequest($"File name too long ({file.FileName.Length} / max {SpawnsetFileConstants.MaxFileNameLength} characters).");

			string filePath = Path.Combine(_environment.WebRootPath, "spawnsets", file.FileName);
			if (Io.File.Exists(filePath))
				return BadRequest($"File '{file.FileName}' already exists.");

			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(m => m.Name == file.FileName);
			if (spawnset == null)
				return BadRequest($"There is no spawnset named '{file.FileName}'.");

			spawnset.LastUpdated = DateTime.UtcNow;
			_dbContext.SaveChanges();

			byte[] formFileBytes = new byte[file.Length];
			using (MemoryStream ms = new())
			{
				file.CopyTo(ms);
				formFileBytes = ms.ToArray();
			}

			if (!Spawnset.TryParse(formFileBytes, out _))
				return BadRequest("File could not be parsed to a proper survival file.");

			byte[] spawnsetHash = MD5.HashData(formFileBytes);
			SpawnsetHashCacheData? existingSpawnset = await _spawnsetHashCache.GetSpawnset(spawnsetHash);
			if (existingSpawnset != null)
				return BadRequest($"Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.");

			Io.File.WriteAllBytes(filePath, formFileBytes);
			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $":white_check_mark: `{User.GetShortName()}` uploaded new SPAWNSET file :file_folder: `{file.FileName}` (`{formFileBytes.Length:n0}` bytes).");

			return Ok();
		}

		[HttpDelete("admin/delete-file")]
		[Authorize(Roles = Roles.Spawnsets)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeleteSpawnsetFile(string fileName)
		{
			string path = Path.Combine(_environment.WebRootPath, "spawnsets", fileName);
			if (!Io.File.Exists(path))
				return BadRequest($"File '{fileName}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(s => s.Name == fileName))
				return BadRequest($"File '{fileName}' is linked to an existing spawnset and cannot be deleted.");

			Io.File.Delete(path);
			_spawnsetHashCache.Clear();

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $":white_check_mark: `{User.GetShortName()}` deleted SPAWNSET file :file_folder: `{fileName}`");

			return Ok();
		}
	}
}
