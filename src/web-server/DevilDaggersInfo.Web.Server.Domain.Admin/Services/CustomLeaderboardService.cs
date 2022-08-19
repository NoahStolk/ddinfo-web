using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Core.CriteriaExpression;
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

		byte[]? gemsCollectedExpression = ValidateCriteriaExpression(addCustomLeaderboard.GemsCollectedCriteria.Expression);
		byte[]? gemsDespawnedCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GemsDespawnedCriteria.Expression);
		byte[]? gemsEatenCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GemsEatenCriteria.Expression);
		byte[]? enemiesKilledCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.EnemiesKilledCriteria.Expression);
		byte[]? daggersFiredCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.DaggersFiredCriteria.Expression);
		byte[]? daggersHitCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.DaggersHitCriteria.Expression);
		byte[]? homingStoredCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.HomingStoredCriteria.Expression);
		byte[]? homingEatenCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.HomingEatenCriteria.Expression);
		byte[]? skull1KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull1KillsCriteria.Expression);
		byte[]? skull2KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull2KillsCriteria.Expression);
		byte[]? skull3KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull3KillsCriteria.Expression);
		byte[]? skull4KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull4KillsCriteria.Expression);
		byte[]? spiderlingKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderlingKillsCriteria.Expression);
		byte[]? spiderEggKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderEggKillsCriteria.Expression);
		byte[]? squid1KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid1KillsCriteria.Expression);
		byte[]? squid2KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid2KillsCriteria.Expression);
		byte[]? squid3KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid3KillsCriteria.Expression);
		byte[]? centipedeKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.CentipedeKillsCriteria.Expression);
		byte[]? gigapedeKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GigapedeKillsCriteria.Expression);
		byte[]? ghostpedeKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GhostpedeKillsCriteria.Expression);
		byte[]? spider1KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider1KillsCriteria.Expression);
		byte[]? spider2KillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider2KillsCriteria.Expression);
		byte[]? leviathanKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LeviathanKillsCriteria.Expression);
		byte[]? orbKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.OrbKillsCriteria.Expression);
		byte[]? thornKillsCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.ThornKillsCriteria.Expression);

		ValidateCustomLeaderboard(addCustomLeaderboard.SpawnsetId, addCustomLeaderboard.Category, addCustomLeaderboard.Daggers, addCustomLeaderboard.IsFeatured);

		CustomLeaderboardEntity customLeaderboard = new()
		{
			DateCreated = DateTime.UtcNow,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
			Category = addCustomLeaderboard.Category,
			Bronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
			Silver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
			Golden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
			Devil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
			Leviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			IsFeatured = addCustomLeaderboard.IsFeatured,
			GemsCollectedCriteria = new() { Operator = addCustomLeaderboard.GemsCollectedCriteria.Operator, Expression = gemsCollectedExpression },
			GemsDespawnedCriteria = new() { Operator = addCustomLeaderboard.GemsDespawnedCriteria.Operator, Expression = gemsDespawnedCriteriaExpression },
			GemsEatenCriteria = new() { Operator = addCustomLeaderboard.GemsEatenCriteria.Operator, Expression = gemsEatenCriteriaExpression },
			EnemiesKilledCriteria = new() { Operator = addCustomLeaderboard.EnemiesKilledCriteria.Operator, Expression = enemiesKilledCriteriaExpression },
			DaggersFiredCriteria = new() { Operator = addCustomLeaderboard.DaggersFiredCriteria.Operator, Expression = daggersFiredCriteriaExpression },
			DaggersHitCriteria = new() { Operator = addCustomLeaderboard.DaggersHitCriteria.Operator, Expression = daggersHitCriteriaExpression },
			HomingStoredCriteria = new() { Operator = addCustomLeaderboard.HomingStoredCriteria.Operator, Expression = homingStoredCriteriaExpression },
			HomingEatenCriteria = new() { Operator = addCustomLeaderboard.HomingEatenCriteria.Operator, Expression = homingEatenCriteriaExpression },
			Skull1KillsCriteria = new() { Operator = addCustomLeaderboard.Skull1KillsCriteria.Operator, Expression = skull1KillsCriteriaExpression },
			Skull2KillsCriteria = new() { Operator = addCustomLeaderboard.Skull2KillsCriteria.Operator, Expression = skull2KillsCriteriaExpression },
			Skull3KillsCriteria = new() { Operator = addCustomLeaderboard.Skull3KillsCriteria.Operator, Expression = skull3KillsCriteriaExpression },
			Skull4KillsCriteria = new() { Operator = addCustomLeaderboard.Skull4KillsCriteria.Operator, Expression = skull4KillsCriteriaExpression },
			SpiderlingKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderlingKillsCriteria.Operator, Expression = spiderlingKillsCriteriaExpression },
			SpiderEggKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderEggKillsCriteria.Operator, Expression = spiderEggKillsCriteriaExpression },
			Squid1KillsCriteria = new() { Operator = addCustomLeaderboard.Squid1KillsCriteria.Operator, Expression = squid1KillsCriteriaExpression },
			Squid2KillsCriteria = new() { Operator = addCustomLeaderboard.Squid2KillsCriteria.Operator, Expression = squid2KillsCriteriaExpression },
			Squid3KillsCriteria = new() { Operator = addCustomLeaderboard.Squid3KillsCriteria.Operator, Expression = squid3KillsCriteriaExpression },
			CentipedeKillsCriteria = new() { Operator = addCustomLeaderboard.CentipedeKillsCriteria.Operator, Expression = centipedeKillsCriteriaExpression },
			GigapedeKillsCriteria = new() { Operator = addCustomLeaderboard.GigapedeKillsCriteria.Operator, Expression = gigapedeKillsCriteriaExpression },
			GhostpedeKillsCriteria = new() { Operator = addCustomLeaderboard.GhostpedeKillsCriteria.Operator, Expression = ghostpedeKillsCriteriaExpression },
			Spider1KillsCriteria = new() { Operator = addCustomLeaderboard.Spider1KillsCriteria.Operator, Expression = spider1KillsCriteriaExpression },
			Spider2KillsCriteria = new() { Operator = addCustomLeaderboard.Spider2KillsCriteria.Operator, Expression = spider2KillsCriteriaExpression },
			LeviathanKillsCriteria = new() { Operator = addCustomLeaderboard.LeviathanKillsCriteria.Operator, Expression = leviathanKillsCriteriaExpression },
			OrbKillsCriteria = new() { Operator = addCustomLeaderboard.OrbKillsCriteria.Operator, Expression = orbKillsCriteriaExpression },
			ThornKillsCriteria = new() { Operator = addCustomLeaderboard.ThornKillsCriteria.Operator, Expression = thornKillsCriteriaExpression },
		};
		_dbContext.CustomLeaderboards.Add(customLeaderboard);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditCustomLeaderboardAsync(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard with ID '{id}' does not exist.");

		byte[]? gemsCollectedExpression = ValidateCriteriaExpression(editCustomLeaderboard.GemsCollectedCriteria.Expression);
		byte[]? gemsDespawnedCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GemsDespawnedCriteria.Expression);
		byte[]? gemsEatenCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GemsEatenCriteria.Expression);
		byte[]? enemiesKilledCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.EnemiesKilledCriteria.Expression);
		byte[]? daggersFiredCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.DaggersFiredCriteria.Expression);
		byte[]? daggersHitCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.DaggersHitCriteria.Expression);
		byte[]? homingStoredCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.HomingStoredCriteria.Expression);
		byte[]? homingEatenCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.HomingEatenCriteria.Expression);
		byte[]? skull1KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull1KillsCriteria.Expression);
		byte[]? skull2KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull2KillsCriteria.Expression);
		byte[]? skull3KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull3KillsCriteria.Expression);
		byte[]? skull4KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull4KillsCriteria.Expression);
		byte[]? spiderlingKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderlingKillsCriteria.Expression);
		byte[]? spiderEggKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderEggKillsCriteria.Expression);
		byte[]? squid1KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid1KillsCriteria.Expression);
		byte[]? squid2KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid2KillsCriteria.Expression);
		byte[]? squid3KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid3KillsCriteria.Expression);
		byte[]? centipedeKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.CentipedeKillsCriteria.Expression);
		byte[]? gigapedeKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GigapedeKillsCriteria.Expression);
		byte[]? ghostpedeKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GhostpedeKillsCriteria.Expression);
		byte[]? spider1KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider1KillsCriteria.Expression);
		byte[]? spider2KillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider2KillsCriteria.Expression);
		byte[]? leviathanKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LeviathanKillsCriteria.Expression);
		byte[]? orbKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.OrbKillsCriteria.Expression);
		byte[]? thornKillsCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.ThornKillsCriteria.Expression);

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
				!ExpressionEqual(gemsCollectedExpression, customLeaderboard.GemsCollectedCriteria.Expression) ||
				!ExpressionEqual(gemsDespawnedCriteriaExpression, customLeaderboard.GemsDespawnedCriteria.Expression) ||
				!ExpressionEqual(gemsEatenCriteriaExpression, customLeaderboard.GemsEatenCriteria.Expression) ||
				!ExpressionEqual(enemiesKilledCriteriaExpression, customLeaderboard.EnemiesKilledCriteria.Expression) ||
				!ExpressionEqual(daggersFiredCriteriaExpression, customLeaderboard.DaggersFiredCriteria.Expression) ||
				!ExpressionEqual(daggersHitCriteriaExpression, customLeaderboard.DaggersHitCriteria.Expression) ||
				!ExpressionEqual(homingStoredCriteriaExpression, customLeaderboard.HomingStoredCriteria.Expression) ||
				!ExpressionEqual(homingEatenCriteriaExpression, customLeaderboard.HomingEatenCriteria.Expression) ||
				!ExpressionEqual(skull1KillsCriteriaExpression, customLeaderboard.Skull1KillsCriteria.Expression) ||
				!ExpressionEqual(skull2KillsCriteriaExpression, customLeaderboard.Skull2KillsCriteria.Expression) ||
				!ExpressionEqual(skull3KillsCriteriaExpression, customLeaderboard.Skull3KillsCriteria.Expression) ||
				!ExpressionEqual(skull4KillsCriteriaExpression, customLeaderboard.Skull4KillsCriteria.Expression) ||
				!ExpressionEqual(spiderlingKillsCriteriaExpression, customLeaderboard.SpiderlingKillsCriteria.Expression) ||
				!ExpressionEqual(spiderEggKillsCriteriaExpression, customLeaderboard.SpiderEggKillsCriteria.Expression) ||
				!ExpressionEqual(squid1KillsCriteriaExpression, customLeaderboard.Squid1KillsCriteria.Expression) ||
				!ExpressionEqual(squid2KillsCriteriaExpression, customLeaderboard.Squid2KillsCriteria.Expression) ||
				!ExpressionEqual(squid3KillsCriteriaExpression, customLeaderboard.Squid3KillsCriteria.Expression) ||
				!ExpressionEqual(centipedeKillsCriteriaExpression, customLeaderboard.CentipedeKillsCriteria.Expression) ||
				!ExpressionEqual(gigapedeKillsCriteriaExpression, customLeaderboard.GigapedeKillsCriteria.Expression) ||
				!ExpressionEqual(ghostpedeKillsCriteriaExpression, customLeaderboard.GhostpedeKillsCriteria.Expression) ||
				!ExpressionEqual(spider1KillsCriteriaExpression, customLeaderboard.Spider1KillsCriteria.Expression) ||
				!ExpressionEqual(spider2KillsCriteriaExpression, customLeaderboard.Spider2KillsCriteria.Expression) ||
				!ExpressionEqual(leviathanKillsCriteriaExpression, customLeaderboard.LeviathanKillsCriteria.Expression) ||
				!ExpressionEqual(orbKillsCriteriaExpression, customLeaderboard.OrbKillsCriteria.Expression) ||
				!ExpressionEqual(thornKillsCriteriaExpression, customLeaderboard.ThornKillsCriteria.Expression);

			if (anyCriteriaOperatorChanged || anyCriteriaValueChanged)
				throw new AdminDomainException("Cannot change criteria for custom leaderboard with scores.");
		}

		ValidateCustomLeaderboard(customLeaderboard.SpawnsetId, editCustomLeaderboard.Category, editCustomLeaderboard.Daggers, editCustomLeaderboard.IsFeatured);

		customLeaderboard.Category = editCustomLeaderboard.Category;
		customLeaderboard.Bronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime();
		customLeaderboard.Silver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime();
		customLeaderboard.Golden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime();
		customLeaderboard.Devil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime();
		customLeaderboard.Leviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime();
		customLeaderboard.IsFeatured = editCustomLeaderboard.IsFeatured;
		customLeaderboard.GemsCollectedCriteria = new() { Operator = editCustomLeaderboard.GemsCollectedCriteria.Operator, Expression = gemsCollectedExpression };
		customLeaderboard.GemsDespawnedCriteria = new() { Operator = editCustomLeaderboard.GemsDespawnedCriteria.Operator, Expression = gemsDespawnedCriteriaExpression };
		customLeaderboard.GemsEatenCriteria = new() { Operator = editCustomLeaderboard.GemsEatenCriteria.Operator, Expression = gemsEatenCriteriaExpression };
		customLeaderboard.EnemiesKilledCriteria = new() { Operator = editCustomLeaderboard.EnemiesKilledCriteria.Operator, Expression = enemiesKilledCriteriaExpression };
		customLeaderboard.DaggersFiredCriteria = new() { Operator = editCustomLeaderboard.DaggersFiredCriteria.Operator, Expression = daggersFiredCriteriaExpression };
		customLeaderboard.DaggersHitCriteria = new() { Operator = editCustomLeaderboard.DaggersHitCriteria.Operator, Expression = daggersHitCriteriaExpression };
		customLeaderboard.HomingStoredCriteria = new() { Operator = editCustomLeaderboard.HomingStoredCriteria.Operator, Expression = homingStoredCriteriaExpression };
		customLeaderboard.HomingEatenCriteria = new() { Operator = editCustomLeaderboard.HomingEatenCriteria.Operator, Expression = homingEatenCriteriaExpression };
		customLeaderboard.Skull1KillsCriteria = new() { Operator = editCustomLeaderboard.Skull1KillsCriteria.Operator, Expression = skull1KillsCriteriaExpression };
		customLeaderboard.Skull2KillsCriteria = new() { Operator = editCustomLeaderboard.Skull2KillsCriteria.Operator, Expression = skull2KillsCriteriaExpression };
		customLeaderboard.Skull3KillsCriteria = new() { Operator = editCustomLeaderboard.Skull3KillsCriteria.Operator, Expression = skull3KillsCriteriaExpression };
		customLeaderboard.Skull4KillsCriteria = new() { Operator = editCustomLeaderboard.Skull4KillsCriteria.Operator, Expression = skull4KillsCriteriaExpression };
		customLeaderboard.SpiderlingKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderlingKillsCriteria.Operator, Expression = spiderlingKillsCriteriaExpression };
		customLeaderboard.SpiderEggKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderEggKillsCriteria.Operator, Expression = spiderEggKillsCriteriaExpression };
		customLeaderboard.Squid1KillsCriteria = new() { Operator = editCustomLeaderboard.Squid1KillsCriteria.Operator, Expression = squid1KillsCriteriaExpression };
		customLeaderboard.Squid2KillsCriteria = new() { Operator = editCustomLeaderboard.Squid2KillsCriteria.Operator, Expression = squid2KillsCriteriaExpression };
		customLeaderboard.Squid3KillsCriteria = new() { Operator = editCustomLeaderboard.Squid3KillsCriteria.Operator, Expression = squid3KillsCriteriaExpression };
		customLeaderboard.CentipedeKillsCriteria = new() { Operator = editCustomLeaderboard.CentipedeKillsCriteria.Operator, Expression = centipedeKillsCriteriaExpression };
		customLeaderboard.GigapedeKillsCriteria = new() { Operator = editCustomLeaderboard.GigapedeKillsCriteria.Operator, Expression = gigapedeKillsCriteriaExpression };
		customLeaderboard.GhostpedeKillsCriteria = new() { Operator = editCustomLeaderboard.GhostpedeKillsCriteria.Operator, Expression = ghostpedeKillsCriteriaExpression };
		customLeaderboard.Spider1KillsCriteria = new() { Operator = editCustomLeaderboard.Spider1KillsCriteria.Operator, Expression = spider1KillsCriteriaExpression };
		customLeaderboard.Spider2KillsCriteria = new() { Operator = editCustomLeaderboard.Spider2KillsCriteria.Operator, Expression = spider2KillsCriteriaExpression };
		customLeaderboard.LeviathanKillsCriteria = new() { Operator = editCustomLeaderboard.LeviathanKillsCriteria.Operator, Expression = leviathanKillsCriteriaExpression };
		customLeaderboard.OrbKillsCriteria = new() { Operator = editCustomLeaderboard.OrbKillsCriteria.Operator, Expression = orbKillsCriteriaExpression };
		customLeaderboard.ThornKillsCriteria = new() { Operator = editCustomLeaderboard.ThornKillsCriteria.Operator, Expression = thornKillsCriteriaExpression };

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
				if (dagger is < min or > max)
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

		GameMode requiredGameMode = category.RequiredGameModeForCategory();
		if (spawnsetBinary.GameMode != requiredGameMode)
			throw new CustomLeaderboardValidationException($"Game mode must be '{requiredGameMode}' when the custom leaderboard category is '{category}'. The spawnset has game mode '{spawnsetBinary.GameMode}'.");

		if (category == CustomLeaderboardCategory.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException($"Custom leaderboard with category '{category}' must have spawns.");
	}

	private static byte[]? ValidateCriteriaExpression(string? criteriaExpression)
	{
		if (string.IsNullOrWhiteSpace(criteriaExpression))
			return null;

		if (!Expression.TryParse(criteriaExpression, out Expression? expression))
			throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' cannot be parsed.");

		byte[] expressionBytes = expression.ToBytes();
		if (expressionBytes.Length > Expression.MaxByteLength)
			throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' is too long.");

		return expressionBytes;
	}

	private static bool ExpressionEqual(byte[]? a, byte[]? b)
	{
		if (a == null && b == null)
			return true;

		if (a == null || b == null)
			return false;

		if (a.Length != b.Length)
			return false;

		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
				return false;
		}

		return true;
	}
}
