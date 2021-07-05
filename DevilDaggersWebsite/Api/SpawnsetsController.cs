using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Constants;
using DevilDaggersWebsite.Dto.Spawnsets;
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
		private readonly ApplicationDbContext _dbContext;
		private readonly DiscordLogger _discordLogger;
		private readonly SpawnsetHashCache _spawnsetHashCache;

		public SpawnsetsController(SpawnsetHelper spawnsetHelper, IWebHostEnvironment environment, ApplicationDbContext dbContext, DiscordLogger discordLogger, SpawnsetHashCache spawnsetHashCache)
		{
			_spawnsetHelper = spawnsetHelper;
			_environment = environment;
			_dbContext = dbContext;
			_discordLogger = discordLogger;
			_spawnsetHashCache = spawnsetHashCache;
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
		[Authorize(Policies.SpawnsetsPolicy)]
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
		[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult AddSpawnset(AddSpawnset addSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
				return BadRequest($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == addSpawnset.Name))
				return BadRequest($"Spawnset with name '{addSpawnset.Name}' already exists.");

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
		[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult EditSpawnset(int id, EditSpawnset editSpawnset)
		{
			if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
				return BadRequest($"Player with ID '{editSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == editSpawnset.Name))
				return BadRequest($"Spawnset with name '{editSpawnset.Name}' already exists.");

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
		[Authorize(Policies.SpawnsetsPolicy)]
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

		[HttpPost("upload-file")]
		[Authorize(Policies.SpawnsetsPolicy)]
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

		[HttpDelete("delete-file")]
		[Authorize(Policies.SpawnsetsPolicy)]
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
