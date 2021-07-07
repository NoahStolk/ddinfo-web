using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.CustomEntries;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/custom-entries/admin")]
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
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<GetCustomEntryBase>> GetCustomEntries()
		{
			List<CustomEntry> customEntries = _dbContext.CustomEntries.AsNoTracking().ToList();

			return customEntries.ConvertAll(ce => new GetCustomEntryBase
			{
				Id = ce.Id,
				ClientVersion = ce.ClientVersion,
				DaggersFired = ce.DaggersFired,
				DaggersHit = ce.DaggersHit,
				DeathType = ce.DeathType,
				EnemiesAlive = ce.EnemiesAlive,
				EnemiesKilled = ce.EnemiesKilled,
				GemsCollected = ce.GemsCollected,
				GemsDespawned = ce.GemsDespawned,
				GemsEaten = ce.GemsEaten,
				GemsTotal = ce.GemsTotal,
				HomingDaggers = ce.HomingDaggers,
				HomingDaggersEaten = ce.HomingDaggersEaten,
				LevelUpTime2 = ce.LevelUpTime2,
				LevelUpTime3 = ce.LevelUpTime3,
				LevelUpTime4 = ce.LevelUpTime4,
				PlayerId = ce.PlayerId,
				SubmitDate = ce.SubmitDate,
				Time = ce.Time,
			});
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> AddCustomEntry(AddCustomEntry addCustomEntry)
		{
			if (!_dbContext.Players.Any(p => p.Id == addCustomEntry.PlayerId))
				return BadRequest($"Player with ID '{addCustomEntry.PlayerId}' does not exist.");

			if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == addCustomEntry.CustomLeaderboardId))
				return BadRequest($"Custom leaderboard with ID '{addCustomEntry.CustomLeaderboardId}' does not exist.");

			if (_dbContext.CustomEntries.Any(cl => cl.CustomLeaderboardId == addCustomEntry.CustomLeaderboardId && cl.PlayerId == addCustomEntry.PlayerId))
				return BadRequest("A score for this player already exists on this custom leaderboard.");

			CustomEntry customEntry = new()
			{
				ClientVersion = addCustomEntry.ClientVersion,
				CustomLeaderboardId = addCustomEntry.CustomLeaderboardId,
				DaggersFired = addCustomEntry.DaggersFired,
				DaggersHit = addCustomEntry.DaggersHit,
				DeathType = addCustomEntry.DeathType,
				EnemiesAlive = addCustomEntry.EnemiesAlive,
				EnemiesKilled = addCustomEntry.EnemiesKilled,
				GemsCollected = addCustomEntry.GemsCollected,
				GemsDespawned = addCustomEntry.GemsDespawned,
				GemsEaten = addCustomEntry.GemsEaten,
				GemsTotal = addCustomEntry.GemsTotal,
				HomingDaggers = addCustomEntry.HomingDaggers,
				HomingDaggersEaten = addCustomEntry.HomingDaggersEaten,
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
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> EditCustomEntryById(int id, EditCustomEntry editCustomEntry)
		{
			if (!_dbContext.Players.Any(p => p.Id == editCustomEntry.PlayerId))
				return BadRequest($"Player with ID '{editCustomEntry.PlayerId}' does not exist.");

			if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == editCustomEntry.CustomLeaderboardId))
				return BadRequest($"Custom leaderboard with ID '{editCustomEntry.CustomLeaderboardId}' does not exist.");

			CustomEntry? customEntry = _dbContext.CustomEntries.FirstOrDefault(ce => ce.Id == id);
			if (customEntry == null)
				return NotFound();

			EditCustomEntry logDto = new()
			{
				ClientVersion = customEntry.ClientVersion,
				CustomLeaderboardId = customEntry.CustomLeaderboardId,
				DaggersFired = customEntry.DaggersFired,
				DaggersHit = customEntry.DaggersHit,
				DeathType = customEntry.DeathType,
				EnemiesAlive = customEntry.EnemiesAlive,
				EnemiesKilled = customEntry.EnemiesKilled,
				GemsCollected = customEntry.GemsCollected,
				GemsDespawned = customEntry.GemsDespawned,
				GemsEaten = customEntry.GemsEaten,
				GemsTotal = customEntry.GemsTotal,
				HomingDaggers = customEntry.HomingDaggers,
				HomingDaggersEaten = customEntry.HomingDaggersEaten,
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
			customEntry.DeathType = editCustomEntry.DeathType;
			customEntry.EnemiesAlive = editCustomEntry.EnemiesAlive;
			customEntry.EnemiesKilled = editCustomEntry.EnemiesKilled;
			customEntry.GemsCollected = editCustomEntry.GemsCollected;
			customEntry.GemsDespawned = editCustomEntry.GemsDespawned;
			customEntry.GemsEaten = editCustomEntry.GemsEaten;
			customEntry.GemsTotal = editCustomEntry.GemsTotal;
			customEntry.HomingDaggers = editCustomEntry.HomingDaggers;
			customEntry.HomingDaggersEaten = editCustomEntry.HomingDaggersEaten;
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
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> DeleteCustomEntryById(int id)
		{
			CustomEntry? customEntry = _dbContext.CustomEntries.FirstOrDefault(ced => ced.Id == id);
			if (customEntry == null)
				return NotFound();

			CustomEntryData? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == id);
			if (customEntryData != null)
				_dbContext.CustomEntryData.Remove(customEntryData);

			_dbContext.CustomEntries.Remove(customEntry);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(customEntry, User, customEntry.Id);

			return Ok();
		}
	}
}
