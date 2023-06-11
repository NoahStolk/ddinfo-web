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
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

// TODO: Unit test.
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
		byte[]? skull1KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull1KillsCriteria.Expression);
		byte[]? skull2KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull2KillsCriteria.Expression);
		byte[]? skull3KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull3KillsCriteria.Expression);
		byte[]? skull4KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull4KillsCriteria.Expression);
		byte[]? spiderlingKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderlingKillsCriteria.Expression);
		byte[]? spiderEggKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderEggKillsCriteria.Expression);
		byte[]? squid1KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid1KillsCriteria.Expression);
		byte[]? squid2KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid2KillsCriteria.Expression);
		byte[]? squid3KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid3KillsCriteria.Expression);
		byte[]? centipedeKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.CentipedeKillsCriteria.Expression);
		byte[]? gigapedeKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GigapedeKillsCriteria.Expression);
		byte[]? ghostpedeKillsCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GhostpedeKillsCriteria.Expression);
		byte[]? spider1KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider1KillsCriteria.Expression);
		byte[]? spider2KillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider2KillsCriteria.Expression);
		byte[]? leviathanKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LeviathanKillsCriteria.Expression);
		byte[]? orbKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.OrbKillsCriteria.Expression);
		byte[]? thornKillCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.ThornKillsCriteria.Expression);
		byte[]? skull1AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull1sAliveCriteria.Expression);
		byte[]? skull2AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull2sAliveCriteria.Expression);
		byte[]? skull3AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull3sAliveCriteria.Expression);
		byte[]? skull4AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Skull4sAliveCriteria.Expression);
		byte[]? spiderlingAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderlingsAliveCriteria.Expression);
		byte[]? spiderEggAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.SpiderEggsAliveCriteria.Expression);
		byte[]? squid1AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid1sAliveCriteria.Expression);
		byte[]? squid2AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid2sAliveCriteria.Expression);
		byte[]? squid3AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Squid3sAliveCriteria.Expression);
		byte[]? centipedeAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.CentipedesAliveCriteria.Expression);
		byte[]? gigapedeAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GigapedesAliveCriteria.Expression);
		byte[]? ghostpedeAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.GhostpedesAliveCriteria.Expression);
		byte[]? spider1AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider1sAliveCriteria.Expression);
		byte[]? spider2AliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.Spider2sAliveCriteria.Expression);
		byte[]? leviathanAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.LeviathansAliveCriteria.Expression);
		byte[]? orbAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.OrbsAliveCriteria.Expression);
		byte[]? thornAliveCountCriteriaExpression = ValidateCriteriaExpression(addCustomLeaderboard.ThornsAliveCriteria.Expression);

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
			Bronze = GetDaggerValue(addCustomLeaderboard.RankSorting, addCustomLeaderboard.Daggers.Bronze),
			Silver = GetDaggerValue(addCustomLeaderboard.RankSorting, addCustomLeaderboard.Daggers.Silver),
			Golden = GetDaggerValue(addCustomLeaderboard.RankSorting, addCustomLeaderboard.Daggers.Golden),
			Devil = GetDaggerValue(addCustomLeaderboard.RankSorting, addCustomLeaderboard.Daggers.Devil),
			Leviathan = GetDaggerValue(addCustomLeaderboard.RankSorting, addCustomLeaderboard.Daggers.Leviathan),
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
			Skull1KillsCriteria = new() { Operator = addCustomLeaderboard.Skull1KillsCriteria.Operator.ToDomain(), Expression = skull1KillCountCriteriaExpression },
			Skull2KillsCriteria = new() { Operator = addCustomLeaderboard.Skull2KillsCriteria.Operator.ToDomain(), Expression = skull2KillCountCriteriaExpression },
			Skull3KillsCriteria = new() { Operator = addCustomLeaderboard.Skull3KillsCriteria.Operator.ToDomain(), Expression = skull3KillCountCriteriaExpression },
			Skull4KillsCriteria = new() { Operator = addCustomLeaderboard.Skull4KillsCriteria.Operator.ToDomain(), Expression = skull4KillCountCriteriaExpression },
			SpiderlingKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderlingKillsCriteria.Operator.ToDomain(), Expression = spiderlingKillCountCriteriaExpression },
			SpiderEggKillsCriteria = new() { Operator = addCustomLeaderboard.SpiderEggKillsCriteria.Operator.ToDomain(), Expression = spiderEggKillCountCriteriaExpression },
			Squid1KillsCriteria = new() { Operator = addCustomLeaderboard.Squid1KillsCriteria.Operator.ToDomain(), Expression = squid1KillCountCriteriaExpression },
			Squid2KillsCriteria = new() { Operator = addCustomLeaderboard.Squid2KillsCriteria.Operator.ToDomain(), Expression = squid2KillCountCriteriaExpression },
			Squid3KillsCriteria = new() { Operator = addCustomLeaderboard.Squid3KillsCriteria.Operator.ToDomain(), Expression = squid3KillCountCriteriaExpression },
			CentipedeKillsCriteria = new() { Operator = addCustomLeaderboard.CentipedeKillsCriteria.Operator.ToDomain(), Expression = centipedeKillCountCriteriaExpression },
			GigapedeKillsCriteria = new() { Operator = addCustomLeaderboard.GigapedeKillsCriteria.Operator.ToDomain(), Expression = gigapedeKillCountCriteriaExpression },
			GhostpedeKillsCriteria = new() { Operator = addCustomLeaderboard.GhostpedeKillsCriteria.Operator.ToDomain(), Expression = ghostpedeKillsCountCriteriaExpression },
			Spider1KillsCriteria = new() { Operator = addCustomLeaderboard.Spider1KillsCriteria.Operator.ToDomain(), Expression = spider1KillCountCriteriaExpression },
			Spider2KillsCriteria = new() { Operator = addCustomLeaderboard.Spider2KillsCriteria.Operator.ToDomain(), Expression = spider2KillCountCriteriaExpression },
			LeviathanKillsCriteria = new() { Operator = addCustomLeaderboard.LeviathanKillsCriteria.Operator.ToDomain(), Expression = leviathanKillCountCriteriaExpression },
			OrbKillsCriteria = new() { Operator = addCustomLeaderboard.OrbKillsCriteria.Operator.ToDomain(), Expression = orbKillCountCriteriaExpression },
			ThornKillsCriteria = new() { Operator = addCustomLeaderboard.ThornKillsCriteria.Operator.ToDomain(), Expression = thornKillCountCriteriaExpression },
			Skull1sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull1sAliveCriteria.Operator.ToDomain(), Expression = skull1AliveCountCriteriaExpression },
			Skull2sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull2sAliveCriteria.Operator.ToDomain(), Expression = skull2AliveCountCriteriaExpression },
			Skull3sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull3sAliveCriteria.Operator.ToDomain(), Expression = skull3AliveCountCriteriaExpression },
			Skull4sAliveCriteria = new() { Operator = addCustomLeaderboard.Skull4sAliveCriteria.Operator.ToDomain(), Expression = skull4AliveCountCriteriaExpression },
			SpiderlingsAliveCriteria = new() { Operator = addCustomLeaderboard.SpiderlingsAliveCriteria.Operator.ToDomain(), Expression = spiderlingAliveCountCriteriaExpression },
			SpiderEggsAliveCriteria = new() { Operator = addCustomLeaderboard.SpiderEggsAliveCriteria.Operator.ToDomain(), Expression = spiderEggAliveCountCriteriaExpression },
			Squid1sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid1sAliveCriteria.Operator.ToDomain(), Expression = squid1AliveCountCriteriaExpression },
			Squid2sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid2sAliveCriteria.Operator.ToDomain(), Expression = squid2AliveCountCriteriaExpression },
			Squid3sAliveCriteria = new() { Operator = addCustomLeaderboard.Squid3sAliveCriteria.Operator.ToDomain(), Expression = squid3AliveCountCriteriaExpression },
			CentipedesAliveCriteria = new() { Operator = addCustomLeaderboard.CentipedesAliveCriteria.Operator.ToDomain(), Expression = centipedeAliveCountCriteriaExpression },
			GigapedesAliveCriteria = new() { Operator = addCustomLeaderboard.GigapedesAliveCriteria.Operator.ToDomain(), Expression = gigapedeAliveCountCriteriaExpression },
			GhostpedesAliveCriteria = new() { Operator = addCustomLeaderboard.GhostpedesAliveCriteria.Operator.ToDomain(), Expression = ghostpedeAliveCountCriteriaExpression },
			Spider1sAliveCriteria = new() { Operator = addCustomLeaderboard.Spider1sAliveCriteria.Operator.ToDomain(), Expression = spider1AliveCountCriteriaExpression },
			Spider2sAliveCriteria = new() { Operator = addCustomLeaderboard.Spider2sAliveCriteria.Operator.ToDomain(), Expression = spider2AliveCountCriteriaExpression },
			LeviathansAliveCriteria = new() { Operator = addCustomLeaderboard.LeviathansAliveCriteria.Operator.ToDomain(), Expression = leviathanAliveCountCriteriaExpression },
			OrbsAliveCriteria = new() { Operator = addCustomLeaderboard.OrbsAliveCriteria.Operator.ToDomain(), Expression = orbAliveCountCriteriaExpression },
			ThornsAliveCriteria = new() { Operator = addCustomLeaderboard.ThornsAliveCriteria.Operator.ToDomain(), Expression = thornAliveCountCriteriaExpression },
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
		byte[]? skull1KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull1KillsCriteria.Expression);
		byte[]? skull2KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull2KillsCriteria.Expression);
		byte[]? skull3KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull3KillsCriteria.Expression);
		byte[]? skull4KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull4KillsCriteria.Expression);
		byte[]? spiderlingKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderlingKillsCriteria.Expression);
		byte[]? spiderEggKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderEggKillsCriteria.Expression);
		byte[]? squid1KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid1KillsCriteria.Expression);
		byte[]? squid2KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid2KillsCriteria.Expression);
		byte[]? squid3KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid3KillsCriteria.Expression);
		byte[]? centipedeKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.CentipedeKillsCriteria.Expression);
		byte[]? gigapedeKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GigapedeKillsCriteria.Expression);
		byte[]? ghostpedeKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GhostpedeKillsCriteria.Expression);
		byte[]? spider1KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider1KillsCriteria.Expression);
		byte[]? spider2KillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider2KillsCriteria.Expression);
		byte[]? leviathanKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LeviathanKillsCriteria.Expression);
		byte[]? orbKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.OrbKillsCriteria.Expression);
		byte[]? thornKillCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.ThornKillsCriteria.Expression);
		byte[]? skull1AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull1sAliveCriteria.Expression);
		byte[]? skull2AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull2sAliveCriteria.Expression);
		byte[]? skull3AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull3sAliveCriteria.Expression);
		byte[]? skull4AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Skull4sAliveCriteria.Expression);
		byte[]? spiderlingAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderlingsAliveCriteria.Expression);
		byte[]? spiderEggAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.SpiderEggsAliveCriteria.Expression);
		byte[]? squid1AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid1sAliveCriteria.Expression);
		byte[]? squid2AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid2sAliveCriteria.Expression);
		byte[]? squid3AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Squid3sAliveCriteria.Expression);
		byte[]? centipedeAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.CentipedesAliveCriteria.Expression);
		byte[]? gigapedeAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GigapedesAliveCriteria.Expression);
		byte[]? ghostpedeAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.GhostpedesAliveCriteria.Expression);
		byte[]? spider1AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider1sAliveCriteria.Expression);
		byte[]? spider2AliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.Spider2sAliveCriteria.Expression);
		byte[]? leviathanAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.LeviathansAliveCriteria.Expression);
		byte[]? orbAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.OrbsAliveCriteria.Expression);
		byte[]? thornAliveCountCriteriaExpression = ValidateCriteriaExpression(editCustomLeaderboard.ThornsAliveCriteria.Expression);

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
				!ExpressionEqual(skull1KillCountCriteriaExpression, customLeaderboard.Skull1KillsCriteria.Expression) ||
				!ExpressionEqual(skull2KillCountCriteriaExpression, customLeaderboard.Skull2KillsCriteria.Expression) ||
				!ExpressionEqual(skull3KillCountCriteriaExpression, customLeaderboard.Skull3KillsCriteria.Expression) ||
				!ExpressionEqual(skull4KillCountCriteriaExpression, customLeaderboard.Skull4KillsCriteria.Expression) ||
				!ExpressionEqual(spiderlingKillCountCriteriaExpression, customLeaderboard.SpiderlingKillsCriteria.Expression) ||
				!ExpressionEqual(spiderEggKillCountCriteriaExpression, customLeaderboard.SpiderEggKillsCriteria.Expression) ||
				!ExpressionEqual(squid1KillCountCriteriaExpression, customLeaderboard.Squid1KillsCriteria.Expression) ||
				!ExpressionEqual(squid2KillCountCriteriaExpression, customLeaderboard.Squid2KillsCriteria.Expression) ||
				!ExpressionEqual(squid3KillCountCriteriaExpression, customLeaderboard.Squid3KillsCriteria.Expression) ||
				!ExpressionEqual(centipedeKillCountCriteriaExpression, customLeaderboard.CentipedeKillsCriteria.Expression) ||
				!ExpressionEqual(gigapedeKillCountCriteriaExpression, customLeaderboard.GigapedeKillsCriteria.Expression) ||
				!ExpressionEqual(ghostpedeKillCountCriteriaExpression, customLeaderboard.GhostpedeKillsCriteria.Expression) ||
				!ExpressionEqual(spider1KillCountCriteriaExpression, customLeaderboard.Spider1KillsCriteria.Expression) ||
				!ExpressionEqual(spider2KillCountCriteriaExpression, customLeaderboard.Spider2KillsCriteria.Expression) ||
				!ExpressionEqual(leviathanKillCountCriteriaExpression, customLeaderboard.LeviathanKillsCriteria.Expression) ||
				!ExpressionEqual(orbKillCountCriteriaExpression, customLeaderboard.OrbKillsCriteria.Expression) ||
				!ExpressionEqual(thornKillCountCriteriaExpression, customLeaderboard.ThornKillsCriteria.Expression) ||
				!ExpressionEqual(skull1AliveCountCriteriaExpression, customLeaderboard.Skull1sAliveCriteria.Expression) ||
				!ExpressionEqual(skull2AliveCountCriteriaExpression, customLeaderboard.Skull2sAliveCriteria.Expression) ||
				!ExpressionEqual(skull3AliveCountCriteriaExpression, customLeaderboard.Skull3sAliveCriteria.Expression) ||
				!ExpressionEqual(skull4AliveCountCriteriaExpression, customLeaderboard.Skull4sAliveCriteria.Expression) ||
				!ExpressionEqual(spiderlingAliveCountCriteriaExpression, customLeaderboard.SpiderlingsAliveCriteria.Expression) ||
				!ExpressionEqual(spiderEggAliveCountCriteriaExpression, customLeaderboard.SpiderEggsAliveCriteria.Expression) ||
				!ExpressionEqual(squid1AliveCountCriteriaExpression, customLeaderboard.Squid1sAliveCriteria.Expression) ||
				!ExpressionEqual(squid2AliveCountCriteriaExpression, customLeaderboard.Squid2sAliveCriteria.Expression) ||
				!ExpressionEqual(squid3AliveCountCriteriaExpression, customLeaderboard.Squid3sAliveCriteria.Expression) ||
				!ExpressionEqual(centipedeAliveCountCriteriaExpression, customLeaderboard.CentipedesAliveCriteria.Expression) ||
				!ExpressionEqual(gigapedeAliveCountCriteriaExpression, customLeaderboard.GigapedesAliveCriteria.Expression) ||
				!ExpressionEqual(ghostpedeAliveCountCriteriaExpression, customLeaderboard.GhostpedesAliveCriteria.Expression) ||
				!ExpressionEqual(spider1AliveCountCriteriaExpression, customLeaderboard.Spider1sAliveCriteria.Expression) ||
				!ExpressionEqual(spider2AliveCountCriteriaExpression, customLeaderboard.Spider2sAliveCriteria.Expression) ||
				!ExpressionEqual(leviathanAliveCountCriteriaExpression, customLeaderboard.LeviathansAliveCriteria.Expression) ||
				!ExpressionEqual(orbAliveCountCriteriaExpression, customLeaderboard.OrbsAliveCriteria.Expression) ||
				!ExpressionEqual(thornAliveCountCriteriaExpression, customLeaderboard.ThornsAliveCriteria.Expression);

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
		customLeaderboard.Bronze = GetDaggerValue(editCustomLeaderboard.RankSorting, editCustomLeaderboard.Daggers.Bronze);
		customLeaderboard.Silver = GetDaggerValue(editCustomLeaderboard.RankSorting, editCustomLeaderboard.Daggers.Silver);
		customLeaderboard.Golden = GetDaggerValue(editCustomLeaderboard.RankSorting, editCustomLeaderboard.Daggers.Golden);
		customLeaderboard.Devil = GetDaggerValue(editCustomLeaderboard.RankSorting, editCustomLeaderboard.Daggers.Devil);
		customLeaderboard.Leviathan = GetDaggerValue(editCustomLeaderboard.RankSorting, editCustomLeaderboard.Daggers.Leviathan);
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
		customLeaderboard.Skull1KillsCriteria = new() { Operator = editCustomLeaderboard.Skull1KillsCriteria.Operator.ToDomain(), Expression = skull1KillCountCriteriaExpression };
		customLeaderboard.Skull2KillsCriteria = new() { Operator = editCustomLeaderboard.Skull2KillsCriteria.Operator.ToDomain(), Expression = skull2KillCountCriteriaExpression };
		customLeaderboard.Skull3KillsCriteria = new() { Operator = editCustomLeaderboard.Skull3KillsCriteria.Operator.ToDomain(), Expression = skull3KillCountCriteriaExpression };
		customLeaderboard.Skull4KillsCriteria = new() { Operator = editCustomLeaderboard.Skull4KillsCriteria.Operator.ToDomain(), Expression = skull4KillCountCriteriaExpression };
		customLeaderboard.SpiderlingKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderlingKillsCriteria.Operator.ToDomain(), Expression = spiderlingKillCountCriteriaExpression };
		customLeaderboard.SpiderEggKillsCriteria = new() { Operator = editCustomLeaderboard.SpiderEggKillsCriteria.Operator.ToDomain(), Expression = spiderEggKillCountCriteriaExpression };
		customLeaderboard.Squid1KillsCriteria = new() { Operator = editCustomLeaderboard.Squid1KillsCriteria.Operator.ToDomain(), Expression = squid1KillCountCriteriaExpression };
		customLeaderboard.Squid2KillsCriteria = new() { Operator = editCustomLeaderboard.Squid2KillsCriteria.Operator.ToDomain(), Expression = squid2KillCountCriteriaExpression };
		customLeaderboard.Squid3KillsCriteria = new() { Operator = editCustomLeaderboard.Squid3KillsCriteria.Operator.ToDomain(), Expression = squid3KillCountCriteriaExpression };
		customLeaderboard.CentipedeKillsCriteria = new() { Operator = editCustomLeaderboard.CentipedeKillsCriteria.Operator.ToDomain(), Expression = centipedeKillCountCriteriaExpression };
		customLeaderboard.GigapedeKillsCriteria = new() { Operator = editCustomLeaderboard.GigapedeKillsCriteria.Operator.ToDomain(), Expression = gigapedeKillCountCriteriaExpression };
		customLeaderboard.GhostpedeKillsCriteria = new() { Operator = editCustomLeaderboard.GhostpedeKillsCriteria.Operator.ToDomain(), Expression = ghostpedeKillCountCriteriaExpression };
		customLeaderboard.Spider1KillsCriteria = new() { Operator = editCustomLeaderboard.Spider1KillsCriteria.Operator.ToDomain(), Expression = spider1KillCountCriteriaExpression };
		customLeaderboard.Spider2KillsCriteria = new() { Operator = editCustomLeaderboard.Spider2KillsCriteria.Operator.ToDomain(), Expression = spider2KillCountCriteriaExpression };
		customLeaderboard.LeviathanKillsCriteria = new() { Operator = editCustomLeaderboard.LeviathanKillsCriteria.Operator.ToDomain(), Expression = leviathanKillCountCriteriaExpression };
		customLeaderboard.OrbKillsCriteria = new() { Operator = editCustomLeaderboard.OrbKillsCriteria.Operator.ToDomain(), Expression = orbKillCountCriteriaExpression };
		customLeaderboard.ThornKillsCriteria = new() { Operator = editCustomLeaderboard.ThornKillsCriteria.Operator.ToDomain(), Expression = thornKillCountCriteriaExpression };
		customLeaderboard.Skull1sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull1sAliveCriteria.Operator.ToDomain(), Expression = skull1AliveCountCriteriaExpression };
		customLeaderboard.Skull2sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull2sAliveCriteria.Operator.ToDomain(), Expression = skull2AliveCountCriteriaExpression };
		customLeaderboard.Skull3sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull3sAliveCriteria.Operator.ToDomain(), Expression = skull3AliveCountCriteriaExpression };
		customLeaderboard.Skull4sAliveCriteria = new() { Operator = editCustomLeaderboard.Skull4sAliveCriteria.Operator.ToDomain(), Expression = skull4AliveCountCriteriaExpression };
		customLeaderboard.SpiderlingsAliveCriteria = new() { Operator = editCustomLeaderboard.SpiderlingsAliveCriteria.Operator.ToDomain(), Expression = spiderlingAliveCountCriteriaExpression };
		customLeaderboard.SpiderEggsAliveCriteria = new() { Operator = editCustomLeaderboard.SpiderEggsAliveCriteria.Operator.ToDomain(), Expression = spiderEggAliveCountCriteriaExpression };
		customLeaderboard.Squid1sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid1sAliveCriteria.Operator.ToDomain(), Expression = squid1AliveCountCriteriaExpression };
		customLeaderboard.Squid2sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid2sAliveCriteria.Operator.ToDomain(), Expression = squid2AliveCountCriteriaExpression };
		customLeaderboard.Squid3sAliveCriteria = new() { Operator = editCustomLeaderboard.Squid3sAliveCriteria.Operator.ToDomain(), Expression = squid3AliveCountCriteriaExpression };
		customLeaderboard.CentipedesAliveCriteria = new() { Operator = editCustomLeaderboard.CentipedesAliveCriteria.Operator.ToDomain(), Expression = centipedeAliveCountCriteriaExpression };
		customLeaderboard.GigapedesAliveCriteria = new() { Operator = editCustomLeaderboard.GigapedesAliveCriteria.Operator.ToDomain(), Expression = gigapedeAliveCountCriteriaExpression };
		customLeaderboard.GhostpedesAliveCriteria = new() { Operator = editCustomLeaderboard.GhostpedesAliveCriteria.Operator.ToDomain(), Expression = ghostpedeAliveCountCriteriaExpression };
		customLeaderboard.Spider1sAliveCriteria = new() { Operator = editCustomLeaderboard.Spider1sAliveCriteria.Operator.ToDomain(), Expression = spider1AliveCountCriteriaExpression };
		customLeaderboard.Spider2sAliveCriteria = new() { Operator = editCustomLeaderboard.Spider2sAliveCriteria.Operator.ToDomain(), Expression = spider2AliveCountCriteriaExpression };
		customLeaderboard.LeviathansAliveCriteria = new() { Operator = editCustomLeaderboard.LeviathansAliveCriteria.Operator.ToDomain(), Expression = leviathanAliveCountCriteriaExpression };
		customLeaderboard.OrbsAliveCriteria = new() { Operator = editCustomLeaderboard.OrbsAliveCriteria.Operator.ToDomain(), Expression = orbAliveCountCriteriaExpression };
		customLeaderboard.ThornsAliveCriteria = new() { Operator = editCustomLeaderboard.ThornsAliveCriteria.Operator.ToDomain(), Expression = thornAliveCountCriteriaExpression };

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
			if (rankSorting.IsTime())
			{
				foreach (double dagger in new[] { customLeaderboardDaggers.Leviathan, customLeaderboardDaggers.Devil, customLeaderboardDaggers.Golden, customLeaderboardDaggers.Silver, customLeaderboardDaggers.Bronze })
				{
					const int min = 1;
					const int max = 1500;
					if (dagger is < min or > max)
						throw new CustomLeaderboardValidationException($"All daggers times must be between {min} and {max} for featured leaderboards.");
				}
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

		if (!CustomLeaderboardUtils.IsGameModeAndRankSortingCombinationAllowed(spawnsetBinary.GameMode, rankSorting))
			throw new CustomLeaderboardValidationException($"Combining game mode '{spawnsetBinary.GameMode}' and rank sorting '{rankSorting}' is not allowed.");

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

	private static int GetDaggerValue(Api.Admin.CustomLeaderboards.CustomLeaderboardRankSorting apiRankSorting, double apiValue)
	{
		if (apiRankSorting.ToDomain().IsTime())
			return apiValue.To10thMilliTime();

		return (int)apiValue;
	}

	private static bool ExpressionEqual(byte[]? a, byte[]? b)
	{
		if (a == null && b == null)
			return true;

		if (a == null || b == null)
			return false;

		return a.SequenceEqual(b);
	}
}
