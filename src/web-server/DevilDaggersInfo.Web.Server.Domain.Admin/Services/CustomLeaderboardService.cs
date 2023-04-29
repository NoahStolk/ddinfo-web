using DevilDaggersInfo.Api.Admin.CustomEntries;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class CustomLeaderboardService
{
	private readonly ApplicationDbContext _dbContext;

	public CustomLeaderboardService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task AddCustomLeaderboardAsync(Api.Admin.CustomLeaderboards.AddCustomLeaderboard addCustomLeaderboard)
	{
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
		byte[]? deathTypeCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.DeathTypeCriteria.Expression, true, i => Enum.IsDefined((CustomEntryDeathType)i));
		byte[]? timeCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.TimeCriteria.Expression, true);
		byte[]? levelUpTime2CriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LevelUpTime2Criteria.Expression, true);
		byte[]? levelUpTime3CriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LevelUpTime3Criteria.Expression, true);
		byte[]? levelUpTime4CriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LevelUpTime4Criteria.Expression, true);
		byte[]? enemiesAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.EnemiesAliveCriteria.Expression);
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
		byte[]? skull1sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull1sAliveCriteria.Expression);
		byte[]? skull2sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull2sAliveCriteria.Expression);
		byte[]? skull3sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull3sAliveCriteria.Expression);
		byte[]? skull4sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull4sAliveCriteria.Expression);
		byte[]? spiderlingsAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderlingsAliveCriteria.Expression);
		byte[]? spiderEggsAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderEggsAliveCriteria.Expression);
		byte[]? squid1sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid1sAliveCriteria.Expression);
		byte[]? squid2sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid2sAliveCriteria.Expression);
		byte[]? squid3sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid3sAliveCriteria.Expression);
		byte[]? centipedesAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.CentipedesAliveCriteria.Expression);
		byte[]? gigapedesAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GigapedesAliveCriteria.Expression);
		byte[]? ghostpedesAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GhostpedesAliveCriteria.Expression);
		byte[]? spider1sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider1sAliveCriteria.Expression);
		byte[]? spider2sAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider2sAliveCriteria.Expression);
		byte[]? leviathansAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LeviathansAliveCriteria.Expression);
		byte[]? orbsAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.OrbsAliveCriteria.Expression);
		byte[]? thornsAliveCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.ThornsAliveCriteria.Expression);

		await ValidateCustomLeaderboardAsync(
			addCustomLeaderboard.SpawnsetId,
			addCustomLeaderboard.RankSorting.ToDomain(),
			addCustomLeaderboard.Daggers,
			addCustomLeaderboard.IsFeatured,
			addCustomLeaderboard.DeathTypeCriteria,
			addCustomLeaderboard.TimeCriteria,
			addCustomLeaderboard.LevelUpTime2Criteria,
			addCustomLeaderboard.LevelUpTime3Criteria,
			addCustomLeaderboard.LevelUpTime4Criteria);

		CustomLeaderboardEntity customLeaderboard = new()
		{
			DateCreated = DateTime.UtcNow,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
			RankSorting = addCustomLeaderboard.RankSorting.ToDomain(),
			Bronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
			Silver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
			Golden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
			Devil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
			Leviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			IsFeatured = addCustomLeaderboard.IsFeatured,
			GemsCollectedCriteria = new() { Operator = addCustomLeaderboard.GemsCollectedCriteria.Operator.ToDomain(), Expression = gemsCollectedExpression },
			GemsDespawnedCriteria = new() { Operator = addCustomLeaderboard.GemsDespawnedCriteria.Operator.ToDomain(), Expression = gemsDespawnedCriteriaExpression },
			GemsEatenCriteria = new() { Operator = addCustomLeaderboard.GemsEatenCriteria.Operator.ToDomain(), Expression = gemsEatenCriteriaExpression },
			EnemiesKilledCriteria = new() { Operator = addCustomLeaderboard.EnemiesKilledCriteria.Operator.ToDomain(), Expression = enemiesKilledCriteriaExpression },
			DaggersFiredCriteria = new() { Operator = addCustomLeaderboard.DaggersFiredCriteria.Operator.ToDomain(), Expression = daggersFiredCriteriaExpression },
			DaggersHitCriteria = new() { Operator = addCustomLeaderboard.DaggersHitCriteria.Operator.ToDomain(), Expression = daggersHitCriteriaExpression },
			HomingStoredCriteria = new() { Operator = addCustomLeaderboard.HomingStoredCriteria.Operator.ToDomain(), Expression = homingStoredCriteriaExpression },
			HomingEatenCriteria = new() { Operator = addCustomLeaderboard.HomingEatenCriteria.Operator.ToDomain(), Expression = homingEatenCriteriaExpression },
			DeathTypeCriteria = new() { Operator = addCustomLeaderboard.DeathTypeCriteria.Operator.ToDomain(), Expression = deathTypeCriteriaExpression },
			TimeCriteria = new() { Operator = addCustomLeaderboard.TimeCriteria.Operator.ToDomain(), Expression = timeCriteriaExpression },
			LevelUpTime2Criteria = new() { Operator = addCustomLeaderboard.LevelUpTime2Criteria.Operator.ToDomain(), Expression = levelUpTime2CriteriaExpression },
			LevelUpTime3Criteria = new() { Operator = addCustomLeaderboard.LevelUpTime3Criteria.Operator.ToDomain(), Expression = levelUpTime3CriteriaExpression },
			LevelUpTime4Criteria = new() { Operator = addCustomLeaderboard.LevelUpTime4Criteria.Operator.ToDomain(), Expression = levelUpTime4CriteriaExpression },
			EnemiesAliveCriteria = new() { Operator = addCustomLeaderboard.EnemiesAliveCriteria.Operator.ToDomain(), Expression = enemiesAliveCriteriaExpression },
			Skull1KillsCriteria = new() { Operator = addCustomLeaderboard.Skull1KillsCriteria.Operator.ToDomain(), Expression = skull1KillsCriteriaExpression },
			Skull2KillsCriteria = new() { Operator = addCustomLeaderboard.Skull2KillsCriteria.Operator.ToDomain(), Expression = skull2KillsCriteriaExpression },
			Skull3KillsCriteria = new() { Operator = addCustomLeaderboard.Skull3KillsCriteria.Operator.ToDomain(), Expression = skull3KillsCriteriaExpression },
			Skull4KillsCriteria = new() { Operator = addCustomLeaderboard.Skull4KillsCriteria.Operator.ToDomain(), Expression = skull4KillsCriteriaExpression },
			SpiderlingKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderlingKillsCriteria.Operator.ToDomain(), Expression = spiderlingKillsCriteriaExpression },
			SpiderEggKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderEggKillsCriteria.Operator.ToDomain(), Expression = spiderEggKillsCriteriaExpression },
			Squid1KillsCriteria = new() { Operator = addCustomLeaderboard.Squid1KillsCriteria.Operator.ToDomain(), Expression = squid1KillsCriteriaExpression },
			Squid2KillsCriteria = new() { Operator = addCustomLeaderboard.Squid2KillsCriteria.Operator.ToDomain(), Expression = squid2KillsCriteriaExpression },
			Squid3KillsCriteria = new() { Operator = addCustomLeaderboard.Squid3KillsCriteria.Operator.ToDomain(), Expression = squid3KillsCriteriaExpression },
			CentipedeKillsCriteria = new() { Operator = addCustomLeaderboard.CentipedeKillsCriteria.Operator.ToDomain(), Expression = centipedeKillsCriteriaExpression },
			GigapedeKillsCriteria = new() { Operator = addCustomLeaderboard.GigapedeKillsCriteria.Operator.ToDomain(), Expression = gigapedeKillsCriteriaExpression },
			GhostpedeKillsCriteria = new() { Operator = addCustomLeaderboard.GhostpedeKillsCriteria.Operator.ToDomain(), Expression = ghostpedeKillsCriteriaExpression },
			Spider1KillsCriteria = new() { Operator = addCustomLeaderboard.Spider1KillsCriteria.Operator.ToDomain(), Expression = spider1KillsCriteriaExpression },
			Spider2KillsCriteria = new() { Operator = addCustomLeaderboard.Spider2KillsCriteria.Operator.ToDomain(), Expression = spider2KillsCriteriaExpression },
			LeviathanKillsCriteria = new() { Operator = addCustomLeaderboard.LeviathanKillsCriteria.Operator.ToDomain(), Expression = leviathanKillsCriteriaExpression },
			OrbKillsCriteria = new() { Operator = addCustomLeaderboard.OrbKillsCriteria.Operator.ToDomain(), Expression = orbKillsCriteriaExpression },
			ThornKillsCriteria = new() { Operator = addCustomLeaderboard.ThornKillsCriteria.Operator.ToDomain(), Expression = thornKillsCriteriaExpression },
			Skull1sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull1sAliveCriteria.Operator.ToDomain(), Expression = skull1sAliveCriteriaExpression },
			Skull2sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull2sAliveCriteria.Operator.ToDomain(), Expression = skull2sAliveCriteriaExpression },
			Skull3sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull3sAliveCriteria.Operator.ToDomain(), Expression = skull3sAliveCriteriaExpression },
			Skull4sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull4sAliveCriteria.Operator.ToDomain(), Expression = skull4sAliveCriteriaExpression },
			SpiderlingsAliveCriteria = new() { Operator = addCustomLeaderboard.SpiderlingsAliveCriteria.Operator.ToDomain(), Expression = spiderlingsAliveCriteriaExpression },
			SpiderEggsAliveCriteria = new() { Operator = addCustomLeaderboard.SpiderEggsAliveCriteria.Operator.ToDomain(), Expression = spiderEggsAliveCriteriaExpression },
			Squid1sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid1sAliveCriteria.Operator.ToDomain(), Expression = squid1sAliveCriteriaExpression },
			Squid2sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid2sAliveCriteria.Operator.ToDomain(), Expression = squid2sAliveCriteriaExpression },
			Squid3sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid3sAliveCriteria.Operator.ToDomain(), Expression = squid3sAliveCriteriaExpression },
			CentipedesAliveCriteria = new() { Operator = addCustomLeaderboard.CentipedesAliveCriteria.Operator.ToDomain(), Expression = centipedesAliveCriteriaExpression },
			GigapedesAliveCriteria = new() { Operator = addCustomLeaderboard.GigapedesAliveCriteria.Operator.ToDomain(), Expression = gigapedesAliveCriteriaExpression },
			GhostpedesAliveCriteria = new() { Operator = addCustomLeaderboard.GhostpedesAliveCriteria.Operator.ToDomain(), Expression = ghostpedesAliveCriteriaExpression },
			Spider1sAliveCriteria = new() { Operator = addCustomLeaderboard.Spider1sAliveCriteria.Operator.ToDomain(), Expression = spider1sAliveCriteriaExpression },
			Spider2sAliveCriteria = new() { Operator = addCustomLeaderboard.Spider2sAliveCriteria.Operator.ToDomain(), Expression = spider2sAliveCriteriaExpression },
			LeviathansAliveCriteria = new() { Operator = addCustomLeaderboard.LeviathansAliveCriteria.Operator.ToDomain(), Expression = leviathansAliveCriteriaExpression },
			OrbsAliveCriteria = new() { Operator = addCustomLeaderboard.OrbsAliveCriteria.Operator.ToDomain(), Expression = orbsAliveCriteriaExpression },
			ThornsAliveCriteria = new() { Operator = addCustomLeaderboard.ThornsAliveCriteria.Operator.ToDomain(), Expression = thornsAliveCriteriaExpression },
		};
		_dbContext.CustomLeaderboards.Add(customLeaderboard);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditCustomLeaderboardAsync(int id, Api.Admin.CustomLeaderboards.EditCustomLeaderboard editCustomLeaderboard)
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
		byte[]? deathTypeCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.DeathTypeCriteria.Expression, true, i => Enum.IsDefined((CustomEntryDeathType)i));
		byte[]? timeCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.TimeCriteria.Expression, true);
		byte[]? levelUpTime2CriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LevelUpTime2Criteria.Expression, true);
		byte[]? levelUpTime3CriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LevelUpTime3Criteria.Expression, true);
		byte[]? levelUpTime4CriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LevelUpTime4Criteria.Expression, true);
		byte[]? enemiesAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.EnemiesAliveCriteria.Expression);
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
		byte[]? skull1sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull1sAliveCriteria.Expression);
		byte[]? skull2sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull2sAliveCriteria.Expression);
		byte[]? skull3sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull3sAliveCriteria.Expression);
		byte[]? skull4sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull4sAliveCriteria.Expression);
		byte[]? spiderlingsAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderlingsAliveCriteria.Expression);
		byte[]? spiderEggsAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderEggsAliveCriteria.Expression);
		byte[]? squid1sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid1sAliveCriteria.Expression);
		byte[]? squid2sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid2sAliveCriteria.Expression);
		byte[]? squid3sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid3sAliveCriteria.Expression);
		byte[]? centipedesAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.CentipedesAliveCriteria.Expression);
		byte[]? gigapedesAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GigapedesAliveCriteria.Expression);
		byte[]? ghostpedesAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GhostpedesAliveCriteria.Expression);
		byte[]? spider1sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider1sAliveCriteria.Expression);
		byte[]? spider2sAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider2sAliveCriteria.Expression);
		byte[]? leviathansAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LeviathansAliveCriteria.Expression);
		byte[]? orbsAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.OrbsAliveCriteria.Expression);
		byte[]? thornsAliveCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.ThornsAliveCriteria.Expression);

		if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
		{
			if (customLeaderboard.RankSorting != editCustomLeaderboard.RankSorting.ToDomain())
				throw new AdminDomainException("Cannot change rank sorting for custom leaderboard with scores.");

			bool anyCriteriaOperatorChanged =
				customLeaderboard.GemsCollectedCriteria.Operator != editCustomLeaderboard.GemsCollectedCriteria.Operator.ToDomain() ||
				customLeaderboard.GemsDespawnedCriteria.Operator != editCustomLeaderboard.GemsDespawnedCriteria.Operator.ToDomain() ||
				customLeaderboard.GemsEatenCriteria.Operator != editCustomLeaderboard.GemsEatenCriteria.Operator.ToDomain() ||
				customLeaderboard.EnemiesKilledCriteria.Operator != editCustomLeaderboard.EnemiesKilledCriteria.Operator.ToDomain() ||
				customLeaderboard.DaggersFiredCriteria.Operator != editCustomLeaderboard.DaggersFiredCriteria.Operator.ToDomain() ||
				customLeaderboard.DaggersHitCriteria.Operator != editCustomLeaderboard.DaggersHitCriteria.Operator.ToDomain() ||
				customLeaderboard.HomingStoredCriteria.Operator != editCustomLeaderboard.HomingStoredCriteria.Operator.ToDomain() ||
				customLeaderboard.HomingEatenCriteria.Operator != editCustomLeaderboard.HomingEatenCriteria.Operator.ToDomain() ||
				customLeaderboard.DeathTypeCriteria.Operator != editCustomLeaderboard.DeathTypeCriteria.Operator.ToDomain() ||
				customLeaderboard.TimeCriteria.Operator != editCustomLeaderboard.TimeCriteria.Operator.ToDomain() ||
				customLeaderboard.LevelUpTime2Criteria.Operator != editCustomLeaderboard.LevelUpTime2Criteria.Operator.ToDomain() ||
				customLeaderboard.LevelUpTime3Criteria.Operator != editCustomLeaderboard.LevelUpTime3Criteria.Operator.ToDomain() ||
				customLeaderboard.LevelUpTime4Criteria.Operator != editCustomLeaderboard.LevelUpTime4Criteria.Operator.ToDomain() ||
				customLeaderboard.EnemiesAliveCriteria.Operator != editCustomLeaderboard.EnemiesAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull1KillsCriteria.Operator != editCustomLeaderboard.Skull1KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull2KillsCriteria.Operator != editCustomLeaderboard.Skull2KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull3KillsCriteria.Operator != editCustomLeaderboard.Skull3KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull4KillsCriteria.Operator != editCustomLeaderboard.Skull4KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.SpiderlingKillsCriteria.Operator != editCustomLeaderboard.SpiderlingKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.SpiderEggKillsCriteria.Operator != editCustomLeaderboard.SpiderEggKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid1KillsCriteria.Operator != editCustomLeaderboard.Squid1KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid2KillsCriteria.Operator != editCustomLeaderboard.Squid2KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid3KillsCriteria.Operator != editCustomLeaderboard.Squid3KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.CentipedeKillsCriteria.Operator != editCustomLeaderboard.CentipedeKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.GigapedeKillsCriteria.Operator != editCustomLeaderboard.GigapedeKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.GhostpedeKillsCriteria.Operator != editCustomLeaderboard.GhostpedeKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Spider1KillsCriteria.Operator != editCustomLeaderboard.Spider1KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Spider2KillsCriteria.Operator != editCustomLeaderboard.Spider2KillsCriteria.Operator.ToDomain() ||
				customLeaderboard.LeviathanKillsCriteria.Operator != editCustomLeaderboard.LeviathanKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.OrbKillsCriteria.Operator != editCustomLeaderboard.OrbKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.ThornKillsCriteria.Operator != editCustomLeaderboard.ThornKillsCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull1sAliveCriteria.Operator != editCustomLeaderboard.Skull1sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull2sAliveCriteria.Operator != editCustomLeaderboard.Skull2sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull3sAliveCriteria.Operator != editCustomLeaderboard.Skull3sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Skull4sAliveCriteria.Operator != editCustomLeaderboard.Skull4sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.SpiderlingsAliveCriteria.Operator != editCustomLeaderboard.SpiderlingsAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.SpiderEggsAliveCriteria.Operator != editCustomLeaderboard.SpiderEggsAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid1sAliveCriteria.Operator != editCustomLeaderboard.Squid1sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid2sAliveCriteria.Operator != editCustomLeaderboard.Squid2sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Squid3sAliveCriteria.Operator != editCustomLeaderboard.Squid3sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.CentipedesAliveCriteria.Operator != editCustomLeaderboard.CentipedesAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.GigapedesAliveCriteria.Operator != editCustomLeaderboard.GigapedesAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.GhostpedesAliveCriteria.Operator != editCustomLeaderboard.GhostpedesAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Spider1sAliveCriteria.Operator != editCustomLeaderboard.Spider1sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.Spider2sAliveCriteria.Operator != editCustomLeaderboard.Spider2sAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.LeviathansAliveCriteria.Operator != editCustomLeaderboard.LeviathansAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.OrbsAliveCriteria.Operator != editCustomLeaderboard.OrbsAliveCriteria.Operator.ToDomain() ||
				customLeaderboard.ThornsAliveCriteria.Operator != editCustomLeaderboard.ThornsAliveCriteria.Operator.ToDomain();

			bool anyCriteriaValueChanged =
				!ExpressionEqual(gemsCollectedExpression, customLeaderboard.GemsCollectedCriteria.Expression) ||
				!ExpressionEqual(gemsDespawnedCriteriaExpression, customLeaderboard.GemsDespawnedCriteria.Expression) ||
				!ExpressionEqual(gemsEatenCriteriaExpression, customLeaderboard.GemsEatenCriteria.Expression) ||
				!ExpressionEqual(enemiesKilledCriteriaExpression, customLeaderboard.EnemiesKilledCriteria.Expression) ||
				!ExpressionEqual(daggersFiredCriteriaExpression, customLeaderboard.DaggersFiredCriteria.Expression) ||
				!ExpressionEqual(daggersHitCriteriaExpression, customLeaderboard.DaggersHitCriteria.Expression) ||
				!ExpressionEqual(homingStoredCriteriaExpression, customLeaderboard.HomingStoredCriteria.Expression) ||
				!ExpressionEqual(homingEatenCriteriaExpression, customLeaderboard.HomingEatenCriteria.Expression) ||
				!ExpressionEqual(deathTypeCriteriaExpression, customLeaderboard.DeathTypeCriteria.Expression) ||
				!ExpressionEqual(timeCriteriaExpression, customLeaderboard.TimeCriteria.Expression) ||
				!ExpressionEqual(levelUpTime2CriteriaExpression, customLeaderboard.LevelUpTime2Criteria.Expression) ||
				!ExpressionEqual(levelUpTime3CriteriaExpression, customLeaderboard.LevelUpTime3Criteria.Expression) ||
				!ExpressionEqual(levelUpTime4CriteriaExpression, customLeaderboard.LevelUpTime4Criteria.Expression) ||
				!ExpressionEqual(enemiesAliveCriteriaExpression, customLeaderboard.EnemiesAliveCriteria.Expression) ||
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
				!ExpressionEqual(thornKillsCriteriaExpression, customLeaderboard.ThornKillsCriteria.Expression) ||
				!ExpressionEqual(skull1sAliveCriteriaExpression, customLeaderboard.Skull1sAliveCriteria.Expression) ||
				!ExpressionEqual(skull2sAliveCriteriaExpression, customLeaderboard.Skull2sAliveCriteria.Expression) ||
				!ExpressionEqual(skull3sAliveCriteriaExpression, customLeaderboard.Skull3sAliveCriteria.Expression) ||
				!ExpressionEqual(skull4sAliveCriteriaExpression, customLeaderboard.Skull4sAliveCriteria.Expression) ||
				!ExpressionEqual(spiderlingsAliveCriteriaExpression, customLeaderboard.SpiderlingsAliveCriteria.Expression) ||
				!ExpressionEqual(spiderEggsAliveCriteriaExpression, customLeaderboard.SpiderEggsAliveCriteria.Expression) ||
				!ExpressionEqual(squid1sAliveCriteriaExpression, customLeaderboard.Squid1sAliveCriteria.Expression) ||
				!ExpressionEqual(squid2sAliveCriteriaExpression, customLeaderboard.Squid2sAliveCriteria.Expression) ||
				!ExpressionEqual(squid3sAliveCriteriaExpression, customLeaderboard.Squid3sAliveCriteria.Expression) ||
				!ExpressionEqual(centipedesAliveCriteriaExpression, customLeaderboard.CentipedesAliveCriteria.Expression) ||
				!ExpressionEqual(gigapedesAliveCriteriaExpression, customLeaderboard.GigapedesAliveCriteria.Expression) ||
				!ExpressionEqual(ghostpedesAliveCriteriaExpression, customLeaderboard.GhostpedesAliveCriteria.Expression) ||
				!ExpressionEqual(spider1sAliveCriteriaExpression, customLeaderboard.Spider1sAliveCriteria.Expression) ||
				!ExpressionEqual(spider2sAliveCriteriaExpression, customLeaderboard.Spider2sAliveCriteria.Expression) ||
				!ExpressionEqual(leviathansAliveCriteriaExpression, customLeaderboard.LeviathansAliveCriteria.Expression) ||
				!ExpressionEqual(orbsAliveCriteriaExpression, customLeaderboard.OrbsAliveCriteria.Expression) ||
				!ExpressionEqual(thornsAliveCriteriaExpression, customLeaderboard.ThornsAliveCriteria.Expression);

			if (anyCriteriaOperatorChanged || anyCriteriaValueChanged)
				throw new AdminDomainException("Cannot change criteria for custom leaderboard with scores.");
		}

		await ValidateCustomLeaderboardAsync(
			customLeaderboard.SpawnsetId,
			editCustomLeaderboard.RankSorting.ToDomain(),
			editCustomLeaderboard.Daggers,
			editCustomLeaderboard.IsFeatured,
			editCustomLeaderboard.DeathTypeCriteria,
			editCustomLeaderboard.TimeCriteria,
			editCustomLeaderboard.LevelUpTime2Criteria,
			editCustomLeaderboard.LevelUpTime3Criteria,
			editCustomLeaderboard.LevelUpTime4Criteria);

		customLeaderboard.RankSorting = editCustomLeaderboard.RankSorting.ToDomain();
		customLeaderboard.Bronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime();
		customLeaderboard.Silver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime();
		customLeaderboard.Golden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime();
		customLeaderboard.Devil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime();
		customLeaderboard.Leviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime();
		customLeaderboard.IsFeatured = editCustomLeaderboard.IsFeatured;
		customLeaderboard.GemsCollectedCriteria = new() { Operator = editCustomLeaderboard.GemsCollectedCriteria.Operator.ToDomain(), Expression = gemsCollectedExpression };
		customLeaderboard.GemsDespawnedCriteria = new() { Operator = editCustomLeaderboard.GemsDespawnedCriteria.Operator.ToDomain(), Expression = gemsDespawnedCriteriaExpression };
		customLeaderboard.GemsEatenCriteria = new() { Operator = editCustomLeaderboard.GemsEatenCriteria.Operator.ToDomain(), Expression = gemsEatenCriteriaExpression };
		customLeaderboard.EnemiesKilledCriteria = new() { Operator = editCustomLeaderboard.EnemiesKilledCriteria.Operator.ToDomain(), Expression = enemiesKilledCriteriaExpression };
		customLeaderboard.DaggersFiredCriteria = new() { Operator = editCustomLeaderboard.DaggersFiredCriteria.Operator.ToDomain(), Expression = daggersFiredCriteriaExpression };
		customLeaderboard.DaggersHitCriteria = new() { Operator = editCustomLeaderboard.DaggersHitCriteria.Operator.ToDomain(), Expression = daggersHitCriteriaExpression };
		customLeaderboard.HomingStoredCriteria = new() { Operator = editCustomLeaderboard.HomingStoredCriteria.Operator.ToDomain(), Expression = homingStoredCriteriaExpression };
		customLeaderboard.HomingEatenCriteria = new() { Operator = editCustomLeaderboard.HomingEatenCriteria.Operator.ToDomain(), Expression = homingEatenCriteriaExpression };
		customLeaderboard.DeathTypeCriteria = new() { Operator = editCustomLeaderboard.DeathTypeCriteria.Operator.ToDomain(), Expression = deathTypeCriteriaExpression };
		customLeaderboard.TimeCriteria = new() { Operator = editCustomLeaderboard.TimeCriteria.Operator.ToDomain(), Expression = timeCriteriaExpression };
		customLeaderboard.LevelUpTime2Criteria = new() { Operator = editCustomLeaderboard.LevelUpTime2Criteria.Operator.ToDomain(), Expression = levelUpTime2CriteriaExpression };
		customLeaderboard.LevelUpTime3Criteria = new() { Operator = editCustomLeaderboard.LevelUpTime3Criteria.Operator.ToDomain(), Expression = levelUpTime3CriteriaExpression };
		customLeaderboard.LevelUpTime4Criteria = new() { Operator = editCustomLeaderboard.LevelUpTime4Criteria.Operator.ToDomain(), Expression = levelUpTime4CriteriaExpression };
		customLeaderboard.EnemiesAliveCriteria = new() { Operator = editCustomLeaderboard.EnemiesAliveCriteria.Operator.ToDomain(), Expression = enemiesAliveCriteriaExpression };
		customLeaderboard.Skull1KillsCriteria = new() { Operator = editCustomLeaderboard.Skull1KillsCriteria.Operator.ToDomain(), Expression = skull1KillsCriteriaExpression };
		customLeaderboard.Skull2KillsCriteria = new() { Operator = editCustomLeaderboard.Skull2KillsCriteria.Operator.ToDomain(), Expression = skull2KillsCriteriaExpression };
		customLeaderboard.Skull3KillsCriteria = new() { Operator = editCustomLeaderboard.Skull3KillsCriteria.Operator.ToDomain(), Expression = skull3KillsCriteriaExpression };
		customLeaderboard.Skull4KillsCriteria = new() { Operator = editCustomLeaderboard.Skull4KillsCriteria.Operator.ToDomain(), Expression = skull4KillsCriteriaExpression };
		customLeaderboard.SpiderlingKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderlingKillsCriteria.Operator.ToDomain(), Expression = spiderlingKillsCriteriaExpression };
		customLeaderboard.SpiderEggKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderEggKillsCriteria.Operator.ToDomain(), Expression = spiderEggKillsCriteriaExpression };
		customLeaderboard.Squid1KillsCriteria = new() { Operator = editCustomLeaderboard.Squid1KillsCriteria.Operator.ToDomain(), Expression = squid1KillsCriteriaExpression };
		customLeaderboard.Squid2KillsCriteria = new() { Operator = editCustomLeaderboard.Squid2KillsCriteria.Operator.ToDomain(), Expression = squid2KillsCriteriaExpression };
		customLeaderboard.Squid3KillsCriteria = new() { Operator = editCustomLeaderboard.Squid3KillsCriteria.Operator.ToDomain(), Expression = squid3KillsCriteriaExpression };
		customLeaderboard.CentipedeKillsCriteria = new() { Operator = editCustomLeaderboard.CentipedeKillsCriteria.Operator.ToDomain(), Expression = centipedeKillsCriteriaExpression };
		customLeaderboard.GigapedeKillsCriteria = new() { Operator = editCustomLeaderboard.GigapedeKillsCriteria.Operator.ToDomain(), Expression = gigapedeKillsCriteriaExpression };
		customLeaderboard.GhostpedeKillsCriteria = new() { Operator = editCustomLeaderboard.GhostpedeKillsCriteria.Operator.ToDomain(), Expression = ghostpedeKillsCriteriaExpression };
		customLeaderboard.Spider1KillsCriteria = new() { Operator = editCustomLeaderboard.Spider1KillsCriteria.Operator.ToDomain(), Expression = spider1KillsCriteriaExpression };
		customLeaderboard.Spider2KillsCriteria = new() { Operator = editCustomLeaderboard.Spider2KillsCriteria.Operator.ToDomain(), Expression = spider2KillsCriteriaExpression };
		customLeaderboard.LeviathanKillsCriteria = new() { Operator = editCustomLeaderboard.LeviathanKillsCriteria.Operator.ToDomain(), Expression = leviathanKillsCriteriaExpression };
		customLeaderboard.OrbKillsCriteria = new() { Operator = editCustomLeaderboard.OrbKillsCriteria.Operator.ToDomain(), Expression = orbKillsCriteriaExpression };
		customLeaderboard.ThornKillsCriteria = new() { Operator = editCustomLeaderboard.ThornKillsCriteria.Operator.ToDomain(), Expression = thornKillsCriteriaExpression };
		customLeaderboard.Skull1sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull1sAliveCriteria.Operator.ToDomain(), Expression = skull1sAliveCriteriaExpression };
		customLeaderboard.Skull2sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull2sAliveCriteria.Operator.ToDomain(), Expression = skull2sAliveCriteriaExpression };
		customLeaderboard.Skull3sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull3sAliveCriteria.Operator.ToDomain(), Expression = skull3sAliveCriteriaExpression };
		customLeaderboard.Skull4sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull4sAliveCriteria.Operator.ToDomain(), Expression = skull4sAliveCriteriaExpression };
		customLeaderboard.SpiderlingsAliveCriteria = new() { Operator = editCustomLeaderboard.SpiderlingsAliveCriteria.Operator.ToDomain(), Expression = spiderlingsAliveCriteriaExpression };
		customLeaderboard.SpiderEggsAliveCriteria = new() { Operator = editCustomLeaderboard.SpiderEggsAliveCriteria.Operator.ToDomain(), Expression = spiderEggsAliveCriteriaExpression };
		customLeaderboard.Squid1sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid1sAliveCriteria.Operator.ToDomain(), Expression = squid1sAliveCriteriaExpression };
		customLeaderboard.Squid2sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid2sAliveCriteria.Operator.ToDomain(), Expression = squid2sAliveCriteriaExpression };
		customLeaderboard.Squid3sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid3sAliveCriteria.Operator.ToDomain(), Expression = squid3sAliveCriteriaExpression };
		customLeaderboard.CentipedesAliveCriteria = new() { Operator = editCustomLeaderboard.CentipedesAliveCriteria.Operator.ToDomain(), Expression = centipedesAliveCriteriaExpression };
		customLeaderboard.GigapedesAliveCriteria = new() { Operator = editCustomLeaderboard.GigapedesAliveCriteria.Operator.ToDomain(), Expression = gigapedesAliveCriteriaExpression };
		customLeaderboard.GhostpedesAliveCriteria = new() { Operator = editCustomLeaderboard.GhostpedesAliveCriteria.Operator.ToDomain(), Expression = ghostpedesAliveCriteriaExpression };
		customLeaderboard.Spider1sAliveCriteria = new() { Operator = editCustomLeaderboard.Spider1sAliveCriteria.Operator.ToDomain(), Expression = spider1sAliveCriteriaExpression };
		customLeaderboard.Spider2sAliveCriteria = new() { Operator = editCustomLeaderboard.Spider2sAliveCriteria.Operator.ToDomain(), Expression = spider2sAliveCriteriaExpression };
		customLeaderboard.LeviathansAliveCriteria = new() { Operator = editCustomLeaderboard.LeviathansAliveCriteria.Operator.ToDomain(), Expression = leviathansAliveCriteriaExpression };
		customLeaderboard.OrbsAliveCriteria = new() { Operator = editCustomLeaderboard.OrbsAliveCriteria.Operator.ToDomain(), Expression = orbsAliveCriteriaExpression };
		customLeaderboard.ThornsAliveCriteria = new() { Operator = editCustomLeaderboard.ThornsAliveCriteria.Operator.ToDomain(), Expression = thornsAliveCriteriaExpression };

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

	private async Task ValidateCustomLeaderboardAsync(
		int spawnsetId,
		CustomLeaderboardRankSorting rankSorting,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardDaggers customLeaderboardDaggers,
		bool isFeatured,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardCriteria deathTypeCriteria,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardCriteria timeCriteria,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardCriteria levelUpTime2Criteria,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardCriteria levelUpTime3Criteria,
		Api.Admin.CustomLeaderboards.AddCustomLeaderboardCriteria levelUpTime4Criteria)
	{
		if (!Enum.IsDefined(rankSorting))
			throw new CustomLeaderboardValidationException($"Rank sorting '{rankSorting}' is not defined.");

		if (isFeatured)
		{
			foreach (double dagger in new[] { customLeaderboardDaggers.Leviathan, customLeaderboardDaggers.Devil, customLeaderboardDaggers.Golden, customLeaderboardDaggers.Silver, customLeaderboardDaggers.Bronze })
			{
				const int min = 1;
				const int max = 1500;
				if (dagger is < min or > max)
					throw new CustomLeaderboardValidationException($"All daggers times must be between {min} and {max} for featured leaderboards.");
			}

			if (rankSorting.IsAscending())
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

		var spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(sf => new { sf.Id, sf.Name, sf.File })
			.FirstOrDefaultAsync(sf => sf.Id == spawnsetId);
		if (spawnset == null)
			throw new CustomLeaderboardValidationException($"Spawnset with ID '{spawnsetId}' does not exist.");

		if (!SpawnsetBinary.TryParse(spawnset.File, out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException($"Could not parse survival file '{spawnset.Name}'. Please review the file. Also review how this file ended up in the database, as it should not be possible to upload non-survival files from the Admin API.");

		if (spawnsetBinary.GameMode == GameMode.TimeAttack && !spawnsetBinary.HasSpawns())
			throw new CustomLeaderboardValidationException("Time Attack spawnset must have spawns.");

		if (spawnsetBinary.GameMode is GameMode.TimeAttack or GameMode.Race && rankSorting != CustomLeaderboardRankSorting.TimeAsc)
			throw new CustomLeaderboardValidationException("Time Attack or Race spawnset must use the Time Ascending rank sorting.");

		CustomLeaderboardCriteriaOperator deathTypeOperator = deathTypeCriteria.Operator.ToDomain();
		if (deathTypeOperator is not (CustomLeaderboardCriteriaOperator.Any or CustomLeaderboardCriteriaOperator.Equal or CustomLeaderboardCriteriaOperator.NotEqual))
			throw new CustomLeaderboardValidationException($"Custom leaderboard cannot contain death type criteria that uses the '{deathTypeCriteria.Operator}' operator.");

		CustomLeaderboardCriteriaOperator timeOperator = timeCriteria.Operator.ToDomain();
		CustomLeaderboardCriteriaOperator levelUpTime2Operator = levelUpTime2Criteria.Operator.ToDomain();
		CustomLeaderboardCriteriaOperator levelUpTime3Operator = levelUpTime3Criteria.Operator.ToDomain();
		CustomLeaderboardCriteriaOperator levelUpTime4Operator = levelUpTime4Criteria.Operator.ToDomain();
		if (timeOperator is CustomLeaderboardCriteriaOperator.Equal or CustomLeaderboardCriteriaOperator.NotEqual or CustomLeaderboardCriteriaOperator.Modulo ||
		    levelUpTime2Operator is CustomLeaderboardCriteriaOperator.Equal or CustomLeaderboardCriteriaOperator.NotEqual or CustomLeaderboardCriteriaOperator.Modulo ||
		    levelUpTime3Operator is CustomLeaderboardCriteriaOperator.Equal or CustomLeaderboardCriteriaOperator.NotEqual or CustomLeaderboardCriteriaOperator.Modulo ||
		    levelUpTime4Operator is CustomLeaderboardCriteriaOperator.Equal or CustomLeaderboardCriteriaOperator.NotEqual or CustomLeaderboardCriteriaOperator.Modulo)
		{
			throw new CustomLeaderboardValidationException($"Custom leaderboard cannot contain time criteria that uses the '{timeCriteria.Operator}' operator.");
		}
	}

	private static byte[]? ValidateCriteriaExpression(string? criteriaExpression, bool mustBeSingleValue = false, Func<int, bool>? singleValueCondition = null, [CallerArgumentExpression("singleValueCondition")] string? singleValueConditionExpression = null)
	{
		if (string.IsNullOrWhiteSpace(criteriaExpression))
			return null;

		if (!Expression.TryParse(criteriaExpression, out Expression? expression))
			throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' cannot be parsed.");

		if (mustBeSingleValue)
		{
			if (expression.Parts.Count > 1)
				throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' must not contain multiple parts.");

			IExpressionPart firstPart = expression.Parts[0];
			if (firstPart is not ExpressionValue value)
				throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' must consist of exactly one expression value (number).");

			if (singleValueCondition != null && !singleValueCondition(value.Value))
				throw new CustomLeaderboardValidationException($"Criteria expression '{criteriaExpression}' with value '{value.Value}' does not meet value condition '{singleValueConditionExpression}'.");
		}

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
