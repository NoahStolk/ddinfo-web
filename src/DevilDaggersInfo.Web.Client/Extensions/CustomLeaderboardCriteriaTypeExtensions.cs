using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.Wiki;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Extensions;

internal static class CustomLeaderboardCriteriaTypeExtensions
{
	public static CustomLeaderboardCriteriaType ToCore(this AdminApi.CustomLeaderboardCriteriaType criteriaType)
	{
		return criteriaType switch
		{
			AdminApi.CustomLeaderboardCriteriaType.GemsCollected => CustomLeaderboardCriteriaType.GemsCollected,
			AdminApi.CustomLeaderboardCriteriaType.GemsDespawned => CustomLeaderboardCriteriaType.GemsDespawned,
			AdminApi.CustomLeaderboardCriteriaType.GemsEaten => CustomLeaderboardCriteriaType.GemsEaten,
			AdminApi.CustomLeaderboardCriteriaType.EnemiesKilled => CustomLeaderboardCriteriaType.EnemiesKilled,
			AdminApi.CustomLeaderboardCriteriaType.DaggersFired => CustomLeaderboardCriteriaType.DaggersFired,
			AdminApi.CustomLeaderboardCriteriaType.DaggersHit => CustomLeaderboardCriteriaType.DaggersHit,
			AdminApi.CustomLeaderboardCriteriaType.HomingStored => CustomLeaderboardCriteriaType.HomingStored,
			AdminApi.CustomLeaderboardCriteriaType.HomingEaten => CustomLeaderboardCriteriaType.HomingEaten,
			AdminApi.CustomLeaderboardCriteriaType.Skull1Kills => CustomLeaderboardCriteriaType.Skull1KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull2Kills => CustomLeaderboardCriteriaType.Skull2KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull3Kills => CustomLeaderboardCriteriaType.Skull3KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull4Kills => CustomLeaderboardCriteriaType.Skull4KillCount,
			AdminApi.CustomLeaderboardCriteriaType.SpiderlingKills => CustomLeaderboardCriteriaType.SpiderlingKillCount,
			AdminApi.CustomLeaderboardCriteriaType.SpiderEggKills => CustomLeaderboardCriteriaType.SpiderEggKillCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid1Kills => CustomLeaderboardCriteriaType.Squid1KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid2Kills => CustomLeaderboardCriteriaType.Squid2KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid3Kills => CustomLeaderboardCriteriaType.Squid3KillCount,
			AdminApi.CustomLeaderboardCriteriaType.CentipedeKills => CustomLeaderboardCriteriaType.CentipedeKillCount,
			AdminApi.CustomLeaderboardCriteriaType.GigapedeKills => CustomLeaderboardCriteriaType.GigapedeKillCount,
			AdminApi.CustomLeaderboardCriteriaType.GhostpedeKills => CustomLeaderboardCriteriaType.GhostpedeKillCount,
			AdminApi.CustomLeaderboardCriteriaType.Spider1Kills => CustomLeaderboardCriteriaType.Spider1KillCount,
			AdminApi.CustomLeaderboardCriteriaType.Spider2Kills => CustomLeaderboardCriteriaType.Spider2KillCount,
			AdminApi.CustomLeaderboardCriteriaType.LeviathanKills => CustomLeaderboardCriteriaType.LeviathanKillCount,
			AdminApi.CustomLeaderboardCriteriaType.OrbKills => CustomLeaderboardCriteriaType.OrbKillCount,
			AdminApi.CustomLeaderboardCriteriaType.ThornKills => CustomLeaderboardCriteriaType.ThornKillCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull1sAlive => CustomLeaderboardCriteriaType.Skull1AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull2sAlive => CustomLeaderboardCriteriaType.Skull2AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull3sAlive => CustomLeaderboardCriteriaType.Skull3AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Skull4sAlive => CustomLeaderboardCriteriaType.Skull4AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.SpiderlingsAlive => CustomLeaderboardCriteriaType.SpiderlingAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.SpiderEggsAlive => CustomLeaderboardCriteriaType.SpiderEggAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid1sAlive => CustomLeaderboardCriteriaType.Squid1AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid2sAlive => CustomLeaderboardCriteriaType.Squid2AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Squid3sAlive => CustomLeaderboardCriteriaType.Squid3AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.CentipedesAlive => CustomLeaderboardCriteriaType.CentipedeAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.GigapedesAlive => CustomLeaderboardCriteriaType.GigapedeAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.GhostpedesAlive => CustomLeaderboardCriteriaType.GhostpedeAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Spider1sAlive => CustomLeaderboardCriteriaType.Spider1AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.Spider2sAlive => CustomLeaderboardCriteriaType.Spider2AliveCount,
			AdminApi.CustomLeaderboardCriteriaType.LeviathansAlive => CustomLeaderboardCriteriaType.LeviathanAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.OrbsAlive => CustomLeaderboardCriteriaType.OrbAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.ThornsAlive => CustomLeaderboardCriteriaType.ThornAliveCount,
			AdminApi.CustomLeaderboardCriteriaType.DeathType => CustomLeaderboardCriteriaType.DeathType,
			AdminApi.CustomLeaderboardCriteriaType.Time => CustomLeaderboardCriteriaType.Time,
			AdminApi.CustomLeaderboardCriteriaType.LevelUpTime2 => CustomLeaderboardCriteriaType.LevelUpTime2,
			AdminApi.CustomLeaderboardCriteriaType.LevelUpTime3 => CustomLeaderboardCriteriaType.LevelUpTime3,
			AdminApi.CustomLeaderboardCriteriaType.LevelUpTime4 => CustomLeaderboardCriteriaType.LevelUpTime4,
			AdminApi.CustomLeaderboardCriteriaType.EnemiesAlive => CustomLeaderboardCriteriaType.EnemiesAlive,
			_ => throw new UnreachableException(),
		};
	}

	public static CustomLeaderboardCriteriaType ToCore(this MainApi.CustomLeaderboardCriteriaType criteriaType)
	{
		return criteriaType switch
		{
			MainApi.CustomLeaderboardCriteriaType.GemsCollected => CustomLeaderboardCriteriaType.GemsCollected,
			MainApi.CustomLeaderboardCriteriaType.GemsDespawned => CustomLeaderboardCriteriaType.GemsDespawned,
			MainApi.CustomLeaderboardCriteriaType.GemsEaten => CustomLeaderboardCriteriaType.GemsEaten,
			MainApi.CustomLeaderboardCriteriaType.EnemiesKilled => CustomLeaderboardCriteriaType.EnemiesKilled,
			MainApi.CustomLeaderboardCriteriaType.DaggersFired => CustomLeaderboardCriteriaType.DaggersFired,
			MainApi.CustomLeaderboardCriteriaType.DaggersHit => CustomLeaderboardCriteriaType.DaggersHit,
			MainApi.CustomLeaderboardCriteriaType.HomingStored => CustomLeaderboardCriteriaType.HomingStored,
			MainApi.CustomLeaderboardCriteriaType.HomingEaten => CustomLeaderboardCriteriaType.HomingEaten,
			MainApi.CustomLeaderboardCriteriaType.Skull1Kills => CustomLeaderboardCriteriaType.Skull1KillCount,
			MainApi.CustomLeaderboardCriteriaType.Skull2Kills => CustomLeaderboardCriteriaType.Skull2KillCount,
			MainApi.CustomLeaderboardCriteriaType.Skull3Kills => CustomLeaderboardCriteriaType.Skull3KillCount,
			MainApi.CustomLeaderboardCriteriaType.Skull4Kills => CustomLeaderboardCriteriaType.Skull4KillCount,
			MainApi.CustomLeaderboardCriteriaType.SpiderlingKills => CustomLeaderboardCriteriaType.SpiderlingKillCount,
			MainApi.CustomLeaderboardCriteriaType.SpiderEggKills => CustomLeaderboardCriteriaType.SpiderEggKillCount,
			MainApi.CustomLeaderboardCriteriaType.Squid1Kills => CustomLeaderboardCriteriaType.Squid1KillCount,
			MainApi.CustomLeaderboardCriteriaType.Squid2Kills => CustomLeaderboardCriteriaType.Squid2KillCount,
			MainApi.CustomLeaderboardCriteriaType.Squid3Kills => CustomLeaderboardCriteriaType.Squid3KillCount,
			MainApi.CustomLeaderboardCriteriaType.CentipedeKills => CustomLeaderboardCriteriaType.CentipedeKillCount,
			MainApi.CustomLeaderboardCriteriaType.GigapedeKills => CustomLeaderboardCriteriaType.GigapedeKillCount,
			MainApi.CustomLeaderboardCriteriaType.GhostpedeKills => CustomLeaderboardCriteriaType.GhostpedeKillCount,
			MainApi.CustomLeaderboardCriteriaType.Spider1Kills => CustomLeaderboardCriteriaType.Spider1KillCount,
			MainApi.CustomLeaderboardCriteriaType.Spider2Kills => CustomLeaderboardCriteriaType.Spider2KillCount,
			MainApi.CustomLeaderboardCriteriaType.LeviathanKills => CustomLeaderboardCriteriaType.LeviathanKillCount,
			MainApi.CustomLeaderboardCriteriaType.OrbKills => CustomLeaderboardCriteriaType.OrbKillCount,
			MainApi.CustomLeaderboardCriteriaType.ThornKills => CustomLeaderboardCriteriaType.ThornKillCount,
			MainApi.CustomLeaderboardCriteriaType.Skull1sAlive => CustomLeaderboardCriteriaType.Skull1AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Skull2sAlive => CustomLeaderboardCriteriaType.Skull2AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Skull3sAlive => CustomLeaderboardCriteriaType.Skull3AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Skull4sAlive => CustomLeaderboardCriteriaType.Skull4AliveCount,
			MainApi.CustomLeaderboardCriteriaType.SpiderlingsAlive => CustomLeaderboardCriteriaType.SpiderlingAliveCount,
			MainApi.CustomLeaderboardCriteriaType.SpiderEggsAlive => CustomLeaderboardCriteriaType.SpiderEggAliveCount,
			MainApi.CustomLeaderboardCriteriaType.Squid1sAlive => CustomLeaderboardCriteriaType.Squid1AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Squid2sAlive => CustomLeaderboardCriteriaType.Squid2AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Squid3sAlive => CustomLeaderboardCriteriaType.Squid3AliveCount,
			MainApi.CustomLeaderboardCriteriaType.CentipedesAlive => CustomLeaderboardCriteriaType.CentipedeAliveCount,
			MainApi.CustomLeaderboardCriteriaType.GigapedesAlive => CustomLeaderboardCriteriaType.GigapedeAliveCount,
			MainApi.CustomLeaderboardCriteriaType.GhostpedesAlive => CustomLeaderboardCriteriaType.GhostpedeAliveCount,
			MainApi.CustomLeaderboardCriteriaType.Spider1sAlive => CustomLeaderboardCriteriaType.Spider1AliveCount,
			MainApi.CustomLeaderboardCriteriaType.Spider2sAlive => CustomLeaderboardCriteriaType.Spider2AliveCount,
			MainApi.CustomLeaderboardCriteriaType.LeviathansAlive => CustomLeaderboardCriteriaType.LeviathanAliveCount,
			MainApi.CustomLeaderboardCriteriaType.OrbsAlive => CustomLeaderboardCriteriaType.OrbAliveCount,
			MainApi.CustomLeaderboardCriteriaType.ThornsAlive => CustomLeaderboardCriteriaType.ThornAliveCount,
			MainApi.CustomLeaderboardCriteriaType.DeathType => CustomLeaderboardCriteriaType.DeathType,
			MainApi.CustomLeaderboardCriteriaType.Time => CustomLeaderboardCriteriaType.Time,
			MainApi.CustomLeaderboardCriteriaType.LevelUpTime2 => CustomLeaderboardCriteriaType.LevelUpTime2,
			MainApi.CustomLeaderboardCriteriaType.LevelUpTime3 => CustomLeaderboardCriteriaType.LevelUpTime3,
			MainApi.CustomLeaderboardCriteriaType.LevelUpTime4 => CustomLeaderboardCriteriaType.LevelUpTime4,
			MainApi.CustomLeaderboardCriteriaType.EnemiesAlive => CustomLeaderboardCriteriaType.EnemiesAlive,
			_ => throw new UnreachableException(),
		};
	}

	public static string GetColor(this CustomLeaderboardCriteriaType criteriaType)
	{
		return criteriaType switch
		{
			CustomLeaderboardCriteriaType.GemsCollected => "#f00", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.GemsDespawned => "#888", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.GemsEaten => "#0f0", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.EnemiesKilled => "#f80", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.DaggersFired => "#f80", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.DaggersHit => "#f80", // TODO: Use same color as graph
			CustomLeaderboardCriteriaType.HomingStored => UpgradesV3_2.Level4.Color.HexCode,
			CustomLeaderboardCriteriaType.HomingEaten => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull1KillCount => EnemiesV3_2.Skull1.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull2KillCount => EnemiesV3_2.Skull2.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull3KillCount => EnemiesV3_2.Skull3.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull4KillCount => EnemiesV3_2.Skull4.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderlingKillCount => EnemiesV3_2.Spiderling.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderEggKillCount => EnemiesV3_2.SpiderEgg1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid1KillCount => EnemiesV3_2.Squid1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid2KillCount => EnemiesV3_2.Squid2.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid3KillCount => EnemiesV3_2.Squid3.Color.HexCode,
			CustomLeaderboardCriteriaType.CentipedeKillCount => EnemiesV3_2.Centipede.Color.HexCode,
			CustomLeaderboardCriteriaType.GigapedeKillCount => EnemiesV3_2.Gigapede.Color.HexCode,
			CustomLeaderboardCriteriaType.GhostpedeKillCount => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider1KillCount => EnemiesV3_2.Spider1.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider2KillCount => EnemiesV3_2.Spider2.Color.HexCode,
			CustomLeaderboardCriteriaType.LeviathanKillCount => EnemiesV3_2.Leviathan.Color.HexCode,
			CustomLeaderboardCriteriaType.OrbKillCount => EnemiesV3_2.TheOrb.Color.HexCode,
			CustomLeaderboardCriteriaType.ThornKillCount => EnemiesV3_2.Thorn.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull1AliveCount => EnemiesV3_2.Skull1.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull2AliveCount => EnemiesV3_2.Skull2.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull3AliveCount => EnemiesV3_2.Skull3.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull4AliveCount => EnemiesV3_2.Skull4.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderlingAliveCount => EnemiesV3_2.Spiderling.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderEggAliveCount => EnemiesV3_2.SpiderEgg1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid1AliveCount => EnemiesV3_2.Squid1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid2AliveCount => EnemiesV3_2.Squid2.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid3AliveCount => EnemiesV3_2.Squid3.Color.HexCode,
			CustomLeaderboardCriteriaType.CentipedeAliveCount => EnemiesV3_2.Centipede.Color.HexCode,
			CustomLeaderboardCriteriaType.GigapedeAliveCount => EnemiesV3_2.Gigapede.Color.HexCode,
			CustomLeaderboardCriteriaType.GhostpedeAliveCount => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider1AliveCount => EnemiesV3_2.Spider1.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider2AliveCount => EnemiesV3_2.Spider2.Color.HexCode,
			CustomLeaderboardCriteriaType.LeviathanAliveCount => EnemiesV3_2.Leviathan.Color.HexCode,
			CustomLeaderboardCriteriaType.OrbAliveCount => EnemiesV3_2.TheOrb.Color.HexCode,
			CustomLeaderboardCriteriaType.ThornAliveCount => EnemiesV3_2.Thorn.Color.HexCode,
			CustomLeaderboardCriteriaType.DeathType => "#f00",
			CustomLeaderboardCriteriaType.Time => "#f00",
			CustomLeaderboardCriteriaType.LevelUpTime2 => UpgradesV3_2.Level2.Color.HexCode,
			CustomLeaderboardCriteriaType.LevelUpTime3 => UpgradesV3_2.Level3.Color.HexCode,
			CustomLeaderboardCriteriaType.LevelUpTime4 => UpgradesV3_2.Level4.Color.HexCode,
			CustomLeaderboardCriteriaType.EnemiesAlive => EnemiesV3_2.Skull4.Color.HexCode,
			_ => throw new UnreachableException(),
		};
	}
}
