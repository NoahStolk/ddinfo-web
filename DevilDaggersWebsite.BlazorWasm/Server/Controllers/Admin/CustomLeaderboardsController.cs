using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
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
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/custom-leaderboards")]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _environment;
		private readonly AuditLogger _auditLogger;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment environment, AuditLogger auditLogger)
		{
			_dbContext = dbContext;
			_environment = environment;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetCustomLeaderboard>> GetCustomLeaderboards([Range(0, 1000)] int pageIndex = 0, [Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault, string? sortBy = null, bool ascending = false)
		{
			IQueryable<CustomLeaderboard> customLeaderboardsQuery = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player);

			if (sortBy != null)
				customLeaderboardsQuery = customLeaderboardsQuery.OrderByMember(sortBy, ascending);

			List<CustomLeaderboard> customLeaderboards = customLeaderboardsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetCustomLeaderboard>
			{
				Results = customLeaderboards.ConvertAll(cl => cl.ToGetCustomLeaderboard()),
				TotalResults = _dbContext.CustomLeaderboards.Count(),
			};
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.FirstOrDefault(cl => cl.Id == id);

			if (customLeaderboard == null)
				return NotFound($"Leaderboard with ID '{id}' was not found.");

			return customLeaderboard.ToGetCustomLeaderboard();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
		{
			if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetFileId == addCustomLeaderboard.SpawnsetId))
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
				.FirstOrDefault(sf => sf.Id == addCustomLeaderboard.SpawnsetId);
			if (spawnsetFile == null)
				return BadRequest($"Spawnset with ID '{addCustomLeaderboard.SpawnsetId}' does not exist.");

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
				SpawnsetFileId = addCustomLeaderboard.SpawnsetId,
				Category = addCustomLeaderboard.Category,
				TimeBronze = addCustomLeaderboard.TimeBronze.To10thMilliTime(),
				TimeSilver = addCustomLeaderboard.TimeSilver.To10thMilliTime(),
				TimeGolden = addCustomLeaderboard.TimeGolden.To10thMilliTime(),
				TimeDevil = addCustomLeaderboard.TimeDevil.To10thMilliTime(),
				TimeLeviathan = addCustomLeaderboard.TimeLeviathan.To10thMilliTime(),
			};
			_dbContext.CustomLeaderboards.Add(customLeaderboard);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addCustomLeaderboard, User, customLeaderboard.Id);

			return Ok(customLeaderboard.Id);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
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

			EditCustomLeaderboard logDto = new()
			{
				Category = customLeaderboard.Category,
				TimeBronze = customLeaderboard.TimeBronze.ToSecondsTime(),
				TimeSilver = customLeaderboard.TimeSilver.ToSecondsTime(),
				TimeGolden = customLeaderboard.TimeGolden.ToSecondsTime(),
				TimeDevil = customLeaderboard.TimeDevil.ToSecondsTime(),
				TimeLeviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
				IsArchived = customLeaderboard.IsArchived,
			};

			customLeaderboard.Category = editCustomLeaderboard.Category;
			customLeaderboard.TimeBronze = editCustomLeaderboard.TimeBronze.To10thMilliTime();
			customLeaderboard.TimeSilver = editCustomLeaderboard.TimeSilver.To10thMilliTime();
			customLeaderboard.TimeGolden = editCustomLeaderboard.TimeGolden.To10thMilliTime();
			customLeaderboard.TimeDevil = editCustomLeaderboard.TimeDevil.To10thMilliTime();
			customLeaderboard.TimeLeviathan = editCustomLeaderboard.TimeLeviathan.To10thMilliTime();
			customLeaderboard.IsArchived = editCustomLeaderboard.IsArchived;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editCustomLeaderboard, User, customLeaderboard.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteCustomLeaderboardById(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
			if (customLeaderboard == null)
				return NotFound();

			if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
				return BadRequest("Custom leaderboard with scores cannot be deleted.");

			_dbContext.CustomLeaderboards.Remove(customLeaderboard);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(customLeaderboard, User, customLeaderboard.Id);

			return Ok();
		}
	}
}
