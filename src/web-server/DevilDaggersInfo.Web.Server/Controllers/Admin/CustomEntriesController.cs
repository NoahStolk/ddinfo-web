using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomEntries;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
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
	private readonly IAuditLogger _auditLogger;
	private readonly IFileSystemService _fileSystemService;

	public CustomEntriesController(ApplicationDbContext dbContext, IAuditLogger auditLogger, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
		_fileSystemService = fileSystemService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetCustomEntryForOverview>> GetCustomEntries(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
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
			CustomEntrySorting.ClientVersion => customEntriesQuery.OrderBy(ce => ce.ClientVersion, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DaggersFired => customEntriesQuery.OrderBy(ce => ce.DaggersFired, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DaggersHit => customEntriesQuery.OrderBy(ce => ce.DaggersHit, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.DeathType => customEntriesQuery.OrderBy(ce => ce.DeathType, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.EnemiesAlive => customEntriesQuery.OrderBy(ce => ce.EnemiesAlive, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.EnemiesKilled => customEntriesQuery.OrderBy(ce => ce.EnemiesKilled, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsCollected => customEntriesQuery.OrderBy(ce => ce.GemsCollected, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsDespawned => customEntriesQuery.OrderBy(ce => ce.GemsDespawned, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsEaten => customEntriesQuery.OrderBy(ce => ce.GemsEaten, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.GemsTotal => customEntriesQuery.OrderBy(ce => ce.GemsTotal, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.HomingStored => customEntriesQuery.OrderBy(ce => ce.HomingStored, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.HomingEaten => customEntriesQuery.OrderBy(ce => ce.HomingEaten, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime2 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime2, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime3 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime3, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.LevelUpTime4 => customEntriesQuery.OrderBy(ce => ce.LevelUpTime4, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.PlayerName => customEntriesQuery.OrderBy(ce => ce.Player.PlayerName, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.SpawnsetName => customEntriesQuery.OrderBy(ce => ce.CustomLeaderboard.Spawnset.Name, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.SubmitDate => customEntriesQuery.OrderBy(ce => ce.SubmitDate, ascending).ThenBy(ce => ce.Id),
			CustomEntrySorting.Time => customEntriesQuery.OrderBy(ce => ce.Time, ascending).ThenBy(ce => ce.Id),
			_ => customEntriesQuery.OrderBy(ce => ce.Id, ascending),
		};

		List<CustomEntryEntity> customEntries = customEntriesQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetCustomEntryForOverview>
		{
			Results = customEntries.ConvertAll(ce => ce.ToGetCustomEntryForOverview()),
			TotalResults = _dbContext.CustomEntries.Count(),
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntry> GetCustomEntryById(int id)
	{
		CustomEntryEntity? customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.FirstOrDefault(cl => cl.Id == id);

		if (customEntry == null)
			return NotFound($"Custom entry with ID '{id}' was not found.");

		return customEntry.ToGetCustomEntry();
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
			LevelUpTime2 = addCustomEntry.LevelUpTime2.To10thMilliTime(),
			LevelUpTime3 = addCustomEntry.LevelUpTime3.To10thMilliTime(),
			LevelUpTime4 = addCustomEntry.LevelUpTime4.To10thMilliTime(),
			PlayerId = addCustomEntry.PlayerId,
			SubmitDate = addCustomEntry.SubmitDate,
			Time = addCustomEntry.Time.To10thMilliTime(),
		};
		_dbContext.CustomEntries.Add(customEntry);
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogAdd(addCustomEntry.GetLog(), User, customEntry.Id);

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
			DeathType = (CustomEntryDeathType)customEntry.DeathType,
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
		customEntry.LevelUpTime2 = editCustomEntry.LevelUpTime2.To10thMilliTime();
		customEntry.LevelUpTime3 = editCustomEntry.LevelUpTime3.To10thMilliTime();
		customEntry.LevelUpTime4 = editCustomEntry.LevelUpTime4.To10thMilliTime();
		customEntry.PlayerId = editCustomEntry.PlayerId;
		customEntry.SubmitDate = editCustomEntry.SubmitDate;
		customEntry.Time = editCustomEntry.Time.To10thMilliTime();
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogEdit(logDto.GetLog(), editCustomEntry.GetLog(), User, customEntry.Id);

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

		string message = fileExists ? $"File {_fileSystemService.FormatPath(path)} was deleted." : $"File {_fileSystemService.FormatPath(path)} was not deleted because it does not exist.";
		_auditLogger.LogDelete(customEntry.GetLog(), User, customEntry.Id, new() { new(message, fileExists ? FileSystemInformationType.Delete : FileSystemInformationType.NotFound) });

		return Ok();
	}
}
