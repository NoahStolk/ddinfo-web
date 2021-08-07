using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetHash;
using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Enumerators;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Server.Transients;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;
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
		private readonly ApplicationDbContext _dbContext;
		private readonly IFileSystemService _fileSystemService;
		private readonly SpawnsetHashCache _spawnsetHashCache;
		private readonly AuditLogger _auditLogger;

		public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, SpawnsetHashCache spawnsetHashCache, AuditLogger auditLogger)
		{
			_dbContext = dbContext;
			_fileSystemService = fileSystemService;
			_spawnsetHashCache = spawnsetHashCache;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetSpawnsetForOverview>> GetSpawnsets(
			[Range(0, 1000)] int pageIndex = 0,
			[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
			SpawnsetSorting? sortBy = null,
			bool ascending = false)
		{
			IQueryable<SpawnsetFile> spawnsetsQuery = _dbContext.SpawnsetFiles.AsNoTracking().Include(s => s.Player);

			spawnsetsQuery = sortBy switch
			{
				SpawnsetSorting.Author => spawnsetsQuery.OrderBy(s => s.Player.PlayerName, ascending),
				SpawnsetSorting.HtmlDescription => spawnsetsQuery.OrderBy(s => s.HtmlDescription, ascending),
				SpawnsetSorting.IsPractice => spawnsetsQuery.OrderBy(s => s.IsPractice, ascending),
				SpawnsetSorting.LastUpdated => spawnsetsQuery.OrderBy(s => s.LastUpdated, ascending),
				SpawnsetSorting.MaxDisplayWaves => spawnsetsQuery.OrderBy(s => s.MaxDisplayWaves, ascending),
				SpawnsetSorting.Name => spawnsetsQuery.OrderBy(s => s.Name, ascending),
				_ => spawnsetsQuery.OrderBy(s => s.Id, ascending),
			};

			List<SpawnsetFile> spawnsets = spawnsetsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetSpawnsetForOverview>
			{
				Results = spawnsets.ConvertAll(s => s.ToGetSpawnsetForOverview()),
				TotalResults = _dbContext.SpawnsetFiles.Count(),
			};
		}

		[HttpGet("names")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<GetSpawnsetName>> GetSpawnsetNames()
		{
			var spawnsets = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.Select(s => new { s.Id, s.Name })
				.ToList();

			return spawnsets.ConvertAll(s => new GetSpawnsetName
			{
				Id = s.Id,
				Name = s.Name,
			});
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<GetSpawnset> GetSpawnsetById(int id)
		{
			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles
				.AsNoTracking()
				.FirstOrDefault(p => p.Id == id);
			if (spawnset == null)
				return NotFound();

			return spawnset.ToGetSpawnset();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
		{
			if (!Spawnset.TryParse(addSpawnset.FileContents, out _))
				return BadRequest("File could not be parsed to a proper survival file.");

			byte[] spawnsetHash = MD5.HashData(addSpawnset.FileContents);
			SpawnsetHashCacheData? existingSpawnset = await _spawnsetHashCache.GetSpawnset(spawnsetHash);
			if (existingSpawnset != null)
				return BadRequest($"Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.");

			// Entity validation.
			if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
				return BadRequest($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

			if (_dbContext.SpawnsetFiles.Any(m => m.Name == addSpawnset.Name))
				return BadRequest($"Spawnset with name '{addSpawnset.Name}' already exists.");

			// Add file.
			string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), addSpawnset.Name);
			Io.File.WriteAllBytes(path, addSpawnset.FileContents);

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

			await _auditLogger.LogAdd(addSpawnset, User, spawnset.Id, new() { new($"File '{_fileSystemService.GetRelevantDisplayPath(path)}' was added.", FileSystemInformationType.Add) });

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

			SpawnsetFile? spawnset = _dbContext.SpawnsetFiles.FirstOrDefault(s => s.Id == id);
			if (spawnset == null)
				return NotFound();

			string? moveInfo = null;
			if (spawnset.Name != editSpawnset.Name)
			{
				if (_dbContext.SpawnsetFiles.Any(m => m.Name == editSpawnset.Name))
					return BadRequest($"Spawnset with name '{editSpawnset.Name}' already exists.");

				string directory = _fileSystemService.GetPath(DataSubDirectory.Spawnsets);
				string oldPath = Path.Combine(directory, spawnset.Name);
				string newPath = Path.Combine(directory, editSpawnset.Name);
				Io.File.Move(oldPath, newPath);
				moveInfo = $"File '{_fileSystemService.GetRelevantDisplayPath(oldPath)}' was moved to {_fileSystemService.GetRelevantDisplayPath(newPath)}.";
			}

			EditSpawnset logDto = new()
			{
				HtmlDescription = spawnset.HtmlDescription,
				IsPractice = spawnset.IsPractice,
				MaxDisplayWaves = spawnset.MaxDisplayWaves,
				Name = spawnset.Name,
				PlayerId = spawnset.PlayerId,
			};

			// Do not update LastUpdated here. This value is based only on the file which cannot be edited.
			spawnset.HtmlDescription = editSpawnset.HtmlDescription;
			spawnset.IsPractice = editSpawnset.IsPractice;
			spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
			spawnset.Name = editSpawnset.Name;
			spawnset.PlayerId = editSpawnset.PlayerId;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editSpawnset, User, spawnset.Id, moveInfo == null ? null : new() { new(moveInfo, FileSystemInformationType.Move) });

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

			string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
			bool fileExists = Io.File.Exists(path);
			if (fileExists)
			{
				Io.File.Delete(path);
				_spawnsetHashCache.Clear();
			}

			_dbContext.SpawnsetFiles.Remove(spawnset);
			_dbContext.SaveChanges();

			string message = fileExists ? $"File '{_fileSystemService.GetRelevantDisplayPath(path)}' was deleted." : $"File '{_fileSystemService.GetRelevantDisplayPath(path)}' was not deleted because it does not exist.";
			await _auditLogger.LogDelete(spawnset, User, spawnset.Id, new() { new(message, fileExists ? FileSystemInformationType.Delete : FileSystemInformationType.NotFoundUnexpected) });

			return Ok();
		}
	}
}
