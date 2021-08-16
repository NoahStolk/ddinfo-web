using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersInfo.Web.BlazorWasm.Shared;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/custom-entries")]
[Authorize(Roles = Roles.Admin)]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public CustomEntriesController(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetCustomEntry>> GetCustomEntries(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
		CustomEntrySorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<CustomEntryEntity> customEntriesQuery = _dbContext.CustomEntries
			.AsNoTracking()
			.AsSingleQuery()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl.Spawnset);

		customEntriesQuery = sortBy switch
		{
			CustomEntrySorting.ClientVersion => customEntriesQuery.OrderBy(ce => ce.ClientVersion, ascending),
			CustomEntrySorting.DaggersFired => customEntriesQuery.OrderBy(ce => ce.DaggersFired, ascending),
			CustomEntrySorting.DaggersHit => customEntriesQuery.OrderBy(ce => ce.DaggersHit, ascending),
			CustomEntrySorting.DeathType => customEntriesQuery.OrderBy(ce => ce.DeathType, ascending),
			CustomEntrySorting.EnemiesAlive => customEntriesQuery.OrderBy(ce => ce.EnemiesAlive, ascending),
			CustomEntrySorting.EnemiesKilled => customEntriesQuery.OrderBy(ce => ce.EnemiesKilled, ascending),
			CustomEntrySorting.GemsCollected => customEntriesQuery.OrderBy(ce => ce.GemsCollected, ascending),
			CustomEntrySorting.GemsDespawned => customEntriesQuery.OrderBy(ce => ce.GemsDespawned, ascending),
			CustomEntrySorting.GemsEaten => customEntriesQuery.OrderBy(ce => ce.GemsEaten, ascending),
			CustomEntrySorting.GemsTotal => customEntriesQuery.OrderBy(ce => ce.GemsTotal, ascending),
			CustomEntrySorting.HomingStored => customEntriesQuery.OrderBy(ce => ce.HomingStored, ascending),
			CustomEntrySorting.HomingEaten => customEntriesQuery.OrderBy(ce => ce.HomingEaten, ascending),
			CustomEntrySorting.LevelUpTime2 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime2, ascending),
			CustomEntrySorting.LevelUpTime3 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime3, ascending),
			CustomEntrySorting.LevelUpTime4 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime4, ascending),
			CustomEntrySorting.PlayerName => customEntriesQuery.OrderBy(ce => ce.Player.PlayerName, ascending),
			CustomEntrySorting.SpawnsetName => customEntriesQuery.OrderBy(ce => ce.CustomLeaderboard.Spawnset.Name, ascending),
			CustomEntrySorting.SubmitDate => customEntriesQuery.OrderBy(ce => ce.SubmitDate, ascending),
			CustomEntrySorting.Time => customEntriesQuery.OrderBy(ce => ce.Time, ascending),
			_ => customEntriesQuery.OrderBy(ce => ce.Id, ascending),
		};

		List<CustomEntryEntity> customEntries = customEntriesQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetCustomEntry>
		{
			Results = customEntries.ConvertAll(ce => ce.ToGetCustomEntry()),
			TotalResults = _dbContext.CustomEntries.Count(),
		};
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomEntry(AddCustomEntry addCustomEntry)
	{
		if (!_dbContext.Players.Any(p => p.Id == addCustomEntry.PlayerId))
			return BadRequest($"Player with ID '{addCustomEntry.PlayerId}' does not exist.");

		if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == addCustomEntry.CustomLeaderboardId))
			return BadRequest($"Custom leaderboard with ID '{addCustomEntry.CustomLeaderboardId}' does not exist.");

		if (_dbContext.CustomEntries.Any(cl => cl.CustomLeaderboardId == addCustomEntry.CustomLeaderboardId && cl.PlayerId == addCustomEntry.PlayerId))
			return BadRequest("A score for this player already exists on this custom leaderboard.");

		CustomEntryEntity customEntry = new()
		{
			ClientVersion = addCustomEntry.ClientVersion,
			CustomLeaderboardId = addCustomEntry.CustomLeaderboardId,
			DaggersFired = addCustomEntry.DaggersFired,
			DaggersHit = addCustomEntry.DaggersHit,
			DeathType = (byte)addCustomEntry.DeathType,
			EnemiesAlive = addCustomEntry.EnemiesAlive,
			EnemiesKilled = addCustomEntry.EnemiesKilled,
			GemsCollected = addCustomEntry.GemsCollected,
			GemsDespawned = addCustomEntry.GemsDespawned,
			GemsEaten = addCustomEntry.GemsEaten,
			GemsTotal = addCustomEntry.GemsTotal,
			HomingStored = addCustomEntry.HomingStored,
			HomingEaten = addCustomEntry.HomingEaten,
			LevelUpTime2 = addCustomEntry.LevelUpTime2,
			LevelUpTime3 = addCustomEntry.LevelUpTime3,
			LevelUpTime4 = addCustomEntry.LevelUpTime4,
			PlayerId = addCustomEntry.PlayerId,
			SubmitDate = addCustomEntry.SubmitDate,
			Time = addCustomEntry.Time,
		};
		_dbContext.CustomEntries.Add(customEntry);
		_dbContext.SaveChanges();

		await _auditLogger.LogAdd(addCustomEntry, User, customEntry.Id);

		return Ok(customEntry.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomEntryById(int id, EditCustomEntry editCustomEntry)
	{
		if (!_dbContext.Players.Any(p => p.Id == editCustomEntry.PlayerId))
			return BadRequest($"Player with ID '{editCustomEntry.PlayerId}' does not exist.");

		if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == editCustomEntry.CustomLeaderboardId))
			return BadRequest($"Custom leaderboard with ID '{editCustomEntry.CustomLeaderboardId}' does not exist.");

		CustomEntryEntity? customEntry = _dbContext.CustomEntries.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			return NotFound();

		EditCustomEntry logDto = new()
		{
			ClientVersion = customEntry.ClientVersion,
			CustomLeaderboardId = customEntry.CustomLeaderboardId,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			DeathType = (DeathType)customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			EnemiesKilled = customEntry.EnemiesKilled,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = customEntry.GemsDespawned,
			GemsEaten = customEntry.GemsEaten,
			GemsTotal = customEntry.GemsTotal,
			HomingStored = customEntry.HomingStored,
			HomingEaten = customEntry.HomingEaten,
			LevelUpTime2 = customEntry.LevelUpTime2,
			LevelUpTime3 = customEntry.LevelUpTime3,
			LevelUpTime4 = customEntry.LevelUpTime4,
			PlayerId = customEntry.PlayerId,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time,
		};

		customEntry.ClientVersion = editCustomEntry.ClientVersion;
		customEntry.CustomLeaderboardId = editCustomEntry.CustomLeaderboardId;
		customEntry.DaggersFired = editCustomEntry.DaggersFired;
		customEntry.DaggersHit = editCustomEntry.DaggersHit;
		customEntry.DeathType = (byte)editCustomEntry.DeathType;
		customEntry.EnemiesAlive = editCustomEntry.EnemiesAlive;
		customEntry.EnemiesKilled = editCustomEntry.EnemiesKilled;
		customEntry.GemsCollected = editCustomEntry.GemsCollected;
		customEntry.GemsDespawned = editCustomEntry.GemsDespawned;
		customEntry.GemsEaten = editCustomEntry.GemsEaten;
		customEntry.GemsTotal = editCustomEntry.GemsTotal;
		customEntry.HomingStored = editCustomEntry.HomingStored;
		customEntry.HomingEaten = editCustomEntry.HomingEaten;
		customEntry.LevelUpTime2 = editCustomEntry.LevelUpTime2;
		customEntry.LevelUpTime3 = editCustomEntry.LevelUpTime3;
		customEntry.LevelUpTime4 = editCustomEntry.LevelUpTime4;
		customEntry.PlayerId = editCustomEntry.PlayerId;
		customEntry.SubmitDate = editCustomEntry.SubmitDate;
		customEntry.Time = editCustomEntry.Time;
		_dbContext.SaveChanges();

		await _auditLogger.LogEdit(logDto, editCustomEntry, User, customEntry.Id);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteCustomEntryById(int id)
	{
		CustomEntryEntity? customEntry = _dbContext.CustomEntries.FirstOrDefault(ced => ced.Id == id);
		if (customEntry == null)
			return NotFound();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == id);
		if (customEntryData != null)
			_dbContext.CustomEntryData.Remove(customEntryData);

		_dbContext.CustomEntries.Remove(customEntry);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(customEntry, User, customEntry.Id);

		return Ok();
	}
}
