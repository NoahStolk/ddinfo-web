using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetHash;
using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Spawnsets;
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

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/spawnsets")]
	[Authorize(Roles = Roles.Spawnsets)]
	[ApiController]
	public class SpawnsetsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly SpawnsetHashCache _spawnsetHashCache;
		private readonly AuditLogger _auditLogger;

		public SpawnsetsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, SpawnsetHashCache spawnsetHashCache, AuditLogger auditLogger)
		{
			_environment = environment;
			_dbContext = dbContext;
			_spawnsetHashCache = spawnsetHashCache;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetSpawnset>> GetSpawnsets([Range(0, 1000)] int pageIndex = 0, [Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault, string? sortBy = null, bool ascending = false)
		{
			IQueryable<SpawnsetFile> spawnsetsQuery = _dbContext.SpawnsetFiles.AsNoTracking();

			if (sortBy != null)
				spawnsetsQuery = spawnsetsQuery.OrderByMember(sortBy, ascending);

			List<SpawnsetFile> spawnsets = spawnsetsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetSpawnset>
			{
				Results = spawnsets.ConvertAll(s => s.ToGetSpawnset()),
				TotalResults = _dbContext.SpawnsetFiles.Count(),
			};
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset, IFormFile file)
		{
			// File validation.
			if (file == null)
				return BadRequest("No file.");

			if (file.Length > SpawnsetFileConstants.MaxFileSize)
				return BadRequest($"File too large ({file.Length:n0} / max {SpawnsetFileConstants.MaxFileSize:n0} bytes).");

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

			// Entity validation.
			if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
				return BadRequest($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == addSpawnset.Name))
				return BadRequest($"Spawnset with name '{addSpawnset.Name}' already exists.");

			// Add file.
			Io.File.WriteAllBytes(Path.Combine(_environment.WebRootPath, "spawnsets", addSpawnset.Name), formFileBytes);

			// Add entity.
			SpawnsetFile spawnset = new()
			{
				HtmlDescription = addSpawnset.HtmlDescription,
				IsPractice = addSpawnset.IsPractice,
				MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
				Name = addSpawnset.Name,
				PlayerId = addSpawnset.PlayerId,
				LastUpdated = DateTime.UtcNow,
			};
			_dbContext.SpawnsetFiles.Add(spawnset);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addSpawnset, User, spawnset.Id);

			return Ok(spawnset.Id);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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

			if (spawnset.Name != editSpawnset.Name)
			{
				string directory = Path.Combine(_environment.WebRootPath, "spawnsets");
				Io.File.Move(Path.Combine(directory, spawnset.Name), Path.Combine(directory, editSpawnset.Name));
			}

			// Do not update LastUpdated here. This value is based only on the file which cannot be edited.
			spawnset.HtmlDescription = editSpawnset.HtmlDescription;
			spawnset.IsPractice = editSpawnset.IsPractice;
			spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
			spawnset.Name = editSpawnset.Name;
			spawnset.PlayerId = editSpawnset.PlayerId;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editSpawnset, User, spawnset.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteSpawnsetById(int id)
		{
			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			if (_dbContext.CustomLeaderboards.Any(ce => ce.SpawnsetFileId == id))
				return BadRequest("Spawnset with custom leaderboard cannot be deleted.");

			string path = Path.Combine(_environment.WebRootPath, "spawnsets", spawnset.Name);
			if (Io.File.Exists(path))
			{
				Io.File.Delete(path);
				_spawnsetHashCache.Clear();
			}

			_dbContext.SpawnsetFiles.Remove(spawnset);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(spawnset, User, spawnset.Id);

			return Ok();
		}
	}
}
