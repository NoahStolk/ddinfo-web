using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomEntries;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-entries")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomEntriesController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, CustomEntryRepository customEntryRepository)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_customEntryRepository = customEntryRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetCustomEntryForOverview>>> GetCustomEntries(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomEntrySorting? sortBy = null,
		bool ascending = false)
		=> await _customEntryRepository.GetCustomEntriesAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomEntry>> GetCustomEntryById(int id)
		=> await _customEntryRepository.GetCustomEntryAsync(id);

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
			LevelUpTime2 = addCustomEntry.LevelUpTime2.To10thMilliTime(),
			LevelUpTime3 = addCustomEntry.LevelUpTime3.To10thMilliTime(),
			LevelUpTime4 = addCustomEntry.LevelUpTime4.To10thMilliTime(),
			PlayerId = addCustomEntry.PlayerId,
			SubmitDate = addCustomEntry.SubmitDate,
			Time = addCustomEntry.Time.To10thMilliTime(),
		};
		_dbContext.CustomEntries.Add(customEntry);
		await _dbContext.SaveChangesAsync();

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
		customEntry.LevelUpTime2 = editCustomEntry.LevelUpTime2.To10thMilliTime();
		customEntry.LevelUpTime3 = editCustomEntry.LevelUpTime3.To10thMilliTime();
		customEntry.LevelUpTime4 = editCustomEntry.LevelUpTime4.To10thMilliTime();
		customEntry.PlayerId = editCustomEntry.PlayerId;
		customEntry.SubmitDate = editCustomEntry.SubmitDate;
		customEntry.Time = editCustomEntry.Time.To10thMilliTime();
		await _dbContext.SaveChangesAsync();

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
		await _dbContext.SaveChangesAsync();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		bool fileExists = IoFile.Exists(path);
		if (fileExists)
			IoFile.Delete(path);

		return Ok();
	}
}
