using DevilDaggersInfo.Api.Admin.CustomEntries;
using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class CustomEntryService
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ApplicationDbContext _dbContext;

	public CustomEntryService(IFileSystemService fileSystemService, ApplicationDbContext dbContext)
	{
		_fileSystemService = fileSystemService;
		_dbContext = dbContext;
	}

	public async Task AddCustomEntryAsync(AddCustomEntry addCustomEntry)
	{
		if (!_dbContext.Players.Any(p => p.Id == addCustomEntry.PlayerId))
			throw new AdminDomainException($"Player with ID '{addCustomEntry.PlayerId}' does not exist.");

		if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == addCustomEntry.CustomLeaderboardId))
			throw new AdminDomainException($"Custom leaderboard with ID '{addCustomEntry.CustomLeaderboardId}' does not exist.");

		if (_dbContext.CustomEntries.Any(cl => cl.CustomLeaderboardId == addCustomEntry.CustomLeaderboardId && cl.PlayerId == addCustomEntry.PlayerId))
			throw new AdminDomainException("A score for this player already exists on this custom leaderboard.");

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
	}

	public async Task EditCustomEntryAsync(int id, EditCustomEntry editCustomEntry)
	{
		if (!_dbContext.Players.Any(p => p.Id == editCustomEntry.PlayerId))
			throw new AdminDomainException($"Player with ID '{editCustomEntry.PlayerId}' does not exist.");

		if (!_dbContext.CustomLeaderboards.Any(cl => cl.Id == editCustomEntry.CustomLeaderboardId))
			throw new AdminDomainException($"Custom leaderboard with ID '{editCustomEntry.CustomLeaderboardId}' does not exist.");

		CustomEntryEntity? customEntry = _dbContext.CustomEntries.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			throw new NotFoundException();

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
	}

	public async Task DeleteCustomEntryAsync(int id)
	{
		CustomEntryEntity? customEntry = _dbContext.CustomEntries.FirstOrDefault(ced => ced.Id == id);
		if (customEntry == null)
			throw new NotFoundException();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == id);
		if (customEntryData != null)
			_dbContext.CustomEntryData.Remove(customEntryData);

		_dbContext.CustomEntries.Remove(customEntry);
		await _dbContext.SaveChangesAsync();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		bool fileExists = File.Exists(path);
		if (fileExists)
			File.Delete(path);
	}
}
