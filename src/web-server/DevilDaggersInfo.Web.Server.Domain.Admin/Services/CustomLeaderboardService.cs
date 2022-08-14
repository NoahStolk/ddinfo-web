using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class CustomLeaderboardService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomLeaderboardService(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	public async Task AddCustomLeaderboardAsync(AddCustomLeaderboard addCustomLeaderboard)
	{
		if (addCustomLeaderboard.Category == CustomLeaderboardCategory.Speedrun)
			throw new AdminDomainException("The Speedrun category is obsolete and should not be used anymore. Consider using the Race category.");

		if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == addCustomLeaderboard.SpawnsetId))
			throw new AdminDomainException("A leaderboard for this spawnset already exists.");

		ValidateCustomLeaderboard(addCustomLeaderboard.SpawnsetId, addCustomLeaderboard.Category, addCustomLeaderboard.Daggers, addCustomLeaderboard.IsFeatured);

		CustomLeaderboardEntity customLeaderboard = new()
		{
			DateCreated = DateTime.UtcNow,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
			Category = addCustomLeaderboard.Category,
			TimeBronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
			TimeSilver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
			TimeGolden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
			TimeDevil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
			TimeLeviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			IsFeatured = addCustomLeaderboard.IsFeatured,
			GemsCollectedCriteria = addCustomLeaderboard.GemsCollectedCriteria.ToEntity(),
			GemsDespawnedCriteria = addCustomLeaderboard.GemsDespawnedCriteria.ToEntity(),
			GemsEatenCriteria = addCustomLeaderboard.GemsEatenCriteria.ToEntity(),
			EnemiesKilledCriteria = addCustomLeaderboard.EnemiesKilledCriteria.ToEntity(),
			DaggersFiredCriteria = addCustomLeaderboard.DaggersFiredCriteria.ToEntity(),
			DaggersHitCriteria = addCustomLeaderboard.DaggersHitCriteria.ToEntity(),
			HomingStoredCriteria = addCustomLeaderboard.HomingStoredCriteria.ToEntity(),
			HomingEatenCriteria = addCustomLeaderboard.HomingEatenCriteria.ToEntity(),
			Skull1KillsCriteria = addCustomLeaderboard.Skull1KillsCriteria.ToEntity(),
			Skull2KillsCriteria = addCustomLeaderboard.Skull2KillsCriteria.ToEntity(),
			Skull3KillsCriteria = addCustomLeaderboard.Skull3KillsCriteria.ToEntity(),
			Skull4KillsCriteria = addCustomLeaderboard.Skull4KillsCriteria.ToEntity(),
			SpiderlingKillsCriteria = addCustomLeaderboard.SpiderlingKillsCriteria.ToEntity(),
			SpiderEggKillsCriteria = addCustomLeaderboard.SpiderEggKillsCriteria.ToEntity(),
			Squid1KillsCriteria = addCustomLeaderboard.Squid1KillsCriteria.ToEntity(),
			Squid2KillsCriteria = addCustomLeaderboard.Squid2KillsCriteria.ToEntity(),
			Squid3KillsCriteria = addCustomLeaderboard.Squid3KillsCriteria.ToEntity(),
			CentipedeKillsCriteria = addCustomLeaderboard.CentipedeKillsCriteria.ToEntity(),
			GigapedeKillsCriteria = addCustomLeaderboard.GigapedeKillsCriteria.ToEntity(),
			GhostpedeKillsCriteria = addCustomLeaderboard.GhostpedeKillsCriteria.ToEntity(),
			Spider1KillsCriteria = addCustomLeaderboard.Spider1KillsCriteria.ToEntity(),
			Spider2KillsCriteria = addCustomLeaderboard.Spider2KillsCriteria.ToEntity(),
			LeviathanKillsCriteria = addCustomLeaderboard.LeviathanKillsCriteria.ToEntity(),
			OrbKillsCriteria = addCustomLeaderboard.OrbKillsCriteria.ToEntity(),
			ThornKillsCriteria = addCustomLeaderboard.ThornKillsCriteria.ToEntity(),
		};
		_dbContext.CustomLeaderboards.Add(customLeaderboard);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditCustomLeaderboardAsync(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard with ID '{id}' does not exist.");

		if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
		{
			if (customLeaderboard.Category != editCustomLeaderboard.Category)
				throw new AdminDomainException("Cannot change category for custom leaderboard with scores.");

			bool anyCriteriaOperatorChanged =
				customLeaderboard.GemsCollectedCriteria.Operator != editCustomLeaderboard.GemsCollectedCriteria.Operator ||
				customLeaderboard.GemsDespawnedCriteria.Operator != editCustomLeaderboard.GemsDespawnedCriteria.Operator ||
				customLeaderboard.GemsEatenCriteria.Operator != editCustomLeaderboard.GemsEatenCriteria.Operator ||
				customLeaderboard.EnemiesKilledCriteria.Operator != editCustomLeaderboard.EnemiesKilledCriteria.Operator ||
				customLeaderboard.DaggersFiredCriteria.Operator != editCustomLeaderboard.DaggersFiredCriteria.Operator ||
				customLeaderboard.DaggersHitCriteria.Operator != editCustomLeaderboard.DaggersHitCriteria.Operator ||
				customLeaderboard.HomingStoredCriteria.Operator != editCustomLeaderboard.HomingStoredCriteria.Operator ||
				customLeaderboard.HomingEatenCriteria.Operator != editCustomLeaderboard.HomingEatenCriteria.Operator ||
				customLeaderboard.Skull1KillsCriteria.Operator != editCustomLeaderboard.Skull1KillsCriteria.Operator ||
				customLeaderboard.Skull2KillsCriteria.Operator != editCustomLeaderboard.Skull2KillsCriteria.Operator ||
				customLeaderboard.Skull3KillsCriteria.Operator != editCustomLeaderboard.Skull3KillsCriteria.Operator ||
				customLeaderboard.Skull4KillsCriteria.Operator != editCustomLeaderboard.Skull4KillsCriteria.Operator ||
				customLeaderboard.SpiderlingKillsCriteria.Operator != editCustomLeaderboard.SpiderlingKillsCriteria.Operator ||
				customLeaderboard.SpiderEggKillsCriteria.Operator != editCustomLeaderboard.SpiderEggKillsCriteria.Operator ||
				customLeaderboard.Squid1KillsCriteria.Operator != editCustomLeaderboard.Squid1KillsCriteria.Operator ||
				customLeaderboard.Squid2KillsCriteria.Operator != editCustomLeaderboard.Squid2KillsCriteria.Operator ||
				customLeaderboard.Squid3KillsCriteria.Operator != editCustomLeaderboard.Squid3KillsCriteria.Operator ||
				customLeaderboard.CentipedeKillsCriteria.Operator != editCustomLeaderboard.CentipedeKillsCriteria.Operator ||
				customLeaderboard.GigapedeKillsCriteria.Operator != editCustomLeaderboard.GigapedeKillsCriteria.Operator ||
				customLeaderboard.GhostpedeKillsCriteria.Operator != editCustomLeaderboard.GhostpedeKillsCriteria.Operator ||
				customLeaderboard.Spider1KillsCriteria.Operator != editCustomLeaderboard.Spider1KillsCriteria.Operator ||
				customLeaderboard.Spider2KillsCriteria.Operator != editCustomLeaderboard.Spider2KillsCriteria.Operator ||
				customLeaderboard.LeviathanKillsCriteria.Operator != editCustomLeaderboard.LeviathanKillsCriteria.Operator ||
				customLeaderboard.OrbKillsCriteria.Operator != editCustomLeaderboard.OrbKillsCriteria.Operator ||
				customLeaderboard.ThornKillsCriteria.Operator != editCustomLeaderboard.ThornKillsCriteria.Operator;

			bool anyCriteriaValueChanged =
				customLeaderboard.GemsCollectedCriteria.Value != editCustomLeaderboard.GemsCollectedCriteria.Value ||
				customLeaderboard.GemsDespawnedCriteria.Value != editCustomLeaderboard.GemsDespawnedCriteria.Value ||
				customLeaderboard.GemsEatenCriteria.Value != editCustomLeaderboard.GemsEatenCriteria.Value ||
				customLeaderboard.EnemiesKilledCriteria.Value != editCustomLeaderboard.EnemiesKilledCriteria.Value ||
				customLeaderboard.DaggersFiredCriteria.Value != editCustomLeaderboard.DaggersFiredCriteria.Value ||
				customLeaderboard.DaggersHitCriteria.Value != editCustomLeaderboard.DaggersHitCriteria.Value ||
				customLeaderboard.HomingStoredCriteria.Value != editCustomLeaderboard.HomingStoredCriteria.Value ||
				customLeaderboard.HomingEatenCriteria.Value != editCustomLeaderboard.HomingEatenCriteria.Value ||
				customLeaderboard.Skull1KillsCriteria.Value != editCustomLeaderboard.Skull1KillsCriteria.Value ||
				customLeaderboard.Skull2KillsCriteria.Value != editCustomLeaderboard.Skull2KillsCriteria.Value ||
				customLeaderboard.Skull3KillsCriteria.Value != editCustomLeaderboard.Skull3KillsCriteria.Value ||
				customLeaderboard.Skull4KillsCriteria.Value != editCustomLeaderboard.Skull4KillsCriteria.Value ||
				customLeaderboard.SpiderlingKillsCriteria.Value != editCustomLeaderboard.SpiderlingKillsCriteria.Value ||
				customLeaderboard.SpiderEggKillsCriteria.Value != editCustomLeaderboard.SpiderEggKillsCriteria.Value ||
				customLeaderboard.Squid1KillsCriteria.Value != editCustomLeaderboard.Squid1KillsCriteria.Value ||
				customLeaderboard.Squid2KillsCriteria.Value != editCustomLeaderboard.Squid2KillsCriteria.Value ||
				customLeaderboard.Squid3KillsCriteria.Value != editCustomLeaderboard.Squid3KillsCriteria.Value ||
				customLeaderboard.CentipedeKillsCriteria.Value != editCustomLeaderboard.CentipedeKillsCriteria.Value ||
				customLeaderboard.GigapedeKillsCriteria.Value != editCustomLeaderboard.GigapedeKillsCriteria.Value ||
				customLeaderboard.GhostpedeKillsCriteria.Value != editCustomLeaderboard.GhostpedeKillsCriteria.Value ||
				customLeaderboard.Spider1KillsCriteria.Value != editCustomLeaderboard.Spider1KillsCriteria.Value ||
				customLeaderboard.Spider2KillsCriteria.Value != editCustomLeaderboard.Spider2KillsCriteria.Value ||
				customLeaderboard.LeviathanKillsCriteria.Value != editCustomLeaderboard.LeviathanKillsCriteria.Value ||
				customLeaderboard.OrbKillsCriteria.Value != editCustomLeaderboard.OrbKillsCriteria.Value ||
				customLeaderboard.ThornKillsCriteria.Value != editCustomLeaderboard.ThornKillsCriteria.Value;

			if (anyCriteriaOperatorChanged || anyCriteriaValueChanged)
				throw new AdminDomainException("Cannot change criteria for custom leaderboard with scores.");
		}

		ValidateCustomLeaderboard(customLeaderboard.SpawnsetId, editCustomLeaderboard.Category, editCustomLeaderboard.Daggers, editCustomLeaderboard.IsFeatured);

		customLeaderboard.Category = editCustomLeaderboard.Category;
		customLeaderboard.TimeBronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime();
		customLeaderboard.TimeSilver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime();
		customLeaderboard.TimeGolden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime();
		customLeaderboard.TimeDevil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime();
		customLeaderboard.TimeLeviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime();
		customLeaderboard.IsFeatured = editCustomLeaderboard.IsFeatured;
		customLeaderboard.GemsCollectedCriteria = editCustomLeaderboard.GemsCollectedCriteria.ToEntity();
		customLeaderboard.GemsDespawnedCriteria = editCustomLeaderboard.GemsDespawnedCriteria.ToEntity();
		customLeaderboard.GemsEatenCriteria = editCustomLeaderboard.GemsEatenCriteria.ToEntity();
		customLeaderboard.EnemiesKilledCriteria = editCustomLeaderboard.EnemiesKilledCriteria.ToEntity();
		customLeaderboard.DaggersFiredCriteria = editCustomLeaderboard.DaggersFiredCriteria.ToEntity();
		customLeaderboard.DaggersHitCriteria = editCustomLeaderboard.DaggersHitCriteria.ToEntity();
		customLeaderboard.HomingStoredCriteria = editCustomLeaderboard.HomingStoredCriteria.ToEntity();
		customLeaderboard.HomingEatenCriteria = editCustomLeaderboard.HomingEatenCriteria.ToEntity();
		customLeaderboard.Skull1KillsCriteria = editCustomLeaderboard.Skull1KillsCriteria.ToEntity();
		customLeaderboard.Skull2KillsCriteria = editCustomLeaderboard.Skull2KillsCriteria.ToEntity();
		customLeaderboard.Skull3KillsCriteria = editCustomLeaderboard.Skull3KillsCriteria.ToEntity();
		customLeaderboard.Skull4KillsCriteria = editCustomLeaderboard.Skull4KillsCriteria.ToEntity();
		customLeaderboard.SpiderlingKillsCriteria = editCustomLeaderboard.SpiderlingKillsCriteria.ToEntity();
		customLeaderboard.SpiderEggKillsCriteria = editCustomLeaderboard.SpiderEggKillsCriteria.ToEntity();
		customLeaderboard.Squid1KillsCriteria = editCustomLeaderboard.Squid1KillsCriteria.ToEntity();
		customLeaderboard.Squid2KillsCriteria = editCustomLeaderboard.Squid2KillsCriteria.ToEntity();
		customLeaderboard.Squid3KillsCriteria = editCustomLeaderboard.Squid3KillsCriteria.ToEntity();
		customLeaderboard.CentipedeKillsCriteria = editCustomLeaderboard.CentipedeKillsCriteria.ToEntity();
		customLeaderboard.GigapedeKillsCriteria = editCustomLeaderboard.GigapedeKillsCriteria.ToEntity();
		customLeaderboard.GhostpedeKillsCriteria = editCustomLeaderboard.GhostpedeKillsCriteria.ToEntity();
		customLeaderboard.Spider1KillsCriteria = editCustomLeaderboard.Spider1KillsCriteria.ToEntity();
		customLeaderboard.Spider2KillsCriteria = editCustomLeaderboard.Spider2KillsCriteria.ToEntity();
		customLeaderboard.LeviathanKillsCriteria = editCustomLeaderboard.LeviathanKillsCriteria.ToEntity();
		customLeaderboard.OrbKillsCriteria = editCustomLeaderboard.OrbKillsCriteria.ToEntity();
		customLeaderboard.ThornKillsCriteria = editCustomLeaderboard.ThornKillsCriteria.ToEntity();

		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteCustomLeaderboardAsync(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard with ID '{id}' does not exist.");

		if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
			throw new AdminDomainException("Custom leaderboard with scores cannot be deleted.");

		_dbContext.CustomLeaderboards.Remove(customLeaderboard);
		await _dbContext.SaveChangesAsync();
	}

	private void ValidateCustomLeaderboard(int spawnsetId, CustomLeaderboardCategory category, AddCustomLeaderboardDaggers customLeaderboardDaggers, bool isFeatured)
	{
		if (!Enum.IsDefined(category))
			throw new CustomLeaderboardValidationException($"Category '{category}' is not defined.");

		if (isFeatured)
		{
			foreach (double dagger in new[] { customLeaderboardDaggers.Leviathan, customLeaderboardDaggers.Devil, customLeaderboardDaggers.Golden, customLeaderboardDaggers.Silver, customLeaderboardDaggers.Bronze })
			{
				const int min = 1;
				const int max = 1500;
				if (dagger < min || dagger > max)
					throw new CustomLeaderboardValidationException($"All daggers times must be between {min} and {max} for featured leaderboards.");
			}

			if (category.IsAscending())
			{
				if (customLeaderboardDaggers.Leviathan >= customLeaderboardDaggers.Devil)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Leviathan time must be smaller than Devil time.");
				if (customLeaderboardDaggers.Devil >= customLeaderboardDaggers.Golden)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Devil time must be smaller than Golden time.");
				if (customLeaderboardDaggers.Golden >= customLeaderboardDaggers.Silver)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Golden time must be smaller than Silver time.");
				if (customLeaderboardDaggers.Silver >= customLeaderboardDaggers.Bronze)
					throw new CustomLeaderboardValidationException("For ascending leaderboards, Silver time must be smaller than Bronze time.");
			}
			else
			{
				if (customLeaderboardDaggers.Leviathan <= customLeaderboardDaggers.Devil)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Leviathan time must be greater than Devil time.");
				if (customLeaderboardDaggers.Devil <= customLeaderboardDaggers.Golden)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Devil time must be greater than Golden time.");
				if (customLeaderboardDaggers.Golden <= customLeaderboardDaggers.Silver)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Golden time must be greater than Silver time.");
				if (customLeaderboardDaggers.Silver <= customLeaderboardDaggers.Bronze)
					throw new CustomLeaderboardValidationException("For descending leaderboards, Silver time must be greater than Bronze time.");
			}
		}

		var spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.Select(sf => new { sf.Id, sf.Name })
			.FirstOrDefault(sf => sf.Id == spawnsetId);
		if (spawnset == null)
			throw new CustomLeaderboardValidationException($"Spawnset with ID '{spawnsetId}' does not exist.");

		string spawnsetFilePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		if (!File.Exists(spawnsetFilePath))
			throw new InvalidOperationException($"Spawnset file '{spawnset.Name}' does not exist. Spawnset with ID '{spawnsetId}' does not have a file which should never happen.");

		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(spawnsetFilePath), out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException($"Could not parse survival file '{spawnset.Name}'. Please review the file. Also review how this file ended up in the 'spawnsets' directory, as it should not be possible to upload non-survival files from the Admin API.");

		GameMode requiredGameMode = category.GetRequiredGameModeForCategory();
		if (spawnsetBinary.GameMode != requiredGameMode)
			throw new CustomLeaderboardValidationException($"Game mode must be '{requiredGameMode}' when the custom leaderboard category is '{category}'. The spawnset has game mode '{spawnsetBinary.GameMode}'.");

		if (category == CustomLeaderboardCategory.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have spawns.");
	}
}
