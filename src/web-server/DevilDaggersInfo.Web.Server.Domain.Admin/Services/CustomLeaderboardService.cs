using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Web;
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
		};
		_dbContext.CustomLeaderboards.Add(customLeaderboard);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditCustomLeaderboardAsync(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard with ID '{id}' does not exist.");

		if (customLeaderboard.Category != editCustomLeaderboard.Category && _dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
			throw new AdminDomainException("Cannot change category for custom leaderboard with scores.");

		ValidateCustomLeaderboard(customLeaderboard.SpawnsetId, editCustomLeaderboard.Category, editCustomLeaderboard.Daggers, editCustomLeaderboard.IsFeatured);

		customLeaderboard.Category = editCustomLeaderboard.Category;
		customLeaderboard.TimeBronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime();
		customLeaderboard.TimeSilver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime();
		customLeaderboard.TimeGolden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime();
		customLeaderboard.TimeDevil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime();
		customLeaderboard.TimeLeviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime();

		customLeaderboard.IsFeatured = editCustomLeaderboard.IsFeatured;
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

		if (category == CustomLeaderboardCategory.Survival && !spawnsetBinary.HasEndLoop())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have an end loop.");

		if (category is CustomLeaderboardCategory.Pacifist or CustomLeaderboardCategory.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have spawns.");
	}
}
