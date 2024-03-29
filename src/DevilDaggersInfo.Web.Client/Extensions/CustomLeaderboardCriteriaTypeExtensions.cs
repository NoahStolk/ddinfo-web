using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.Wiki;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCriteriaTypeExtensions
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
			AdminApi.CustomLeaderboardCriteriaType.Skull1Kills => CustomLeaderboardCriteriaType.Skull1Kills,
			AdminApi.CustomLeaderboardCriteriaType.Skull2Kills => CustomLeaderboardCriteriaType.Skull2Kills,
			AdminApi.CustomLeaderboardCriteriaType.Skull3Kills => CustomLeaderboardCriteriaType.Skull3Kills,
			AdminApi.CustomLeaderboardCriteriaType.Skull4Kills => CustomLeaderboardCriteriaType.Skull4Kills,
			AdminApi.CustomLeaderboardCriteriaType.SpiderlingKills => CustomLeaderboardCriteriaType.SpiderlingKills,
			AdminApi.CustomLeaderboardCriteriaType.SpiderEggKills => CustomLeaderboardCriteriaType.SpiderEggKills,
			AdminApi.CustomLeaderboardCriteriaType.Squid1Kills => CustomLeaderboardCriteriaType.Squid1Kills,
			AdminApi.CustomLeaderboardCriteriaType.Squid2Kills => CustomLeaderboardCriteriaType.Squid2Kills,
			AdminApi.CustomLeaderboardCriteriaType.Squid3Kills => CustomLeaderboardCriteriaType.Squid3Kills,
			AdminApi.CustomLeaderboardCriteriaType.CentipedeKills => CustomLeaderboardCriteriaType.CentipedeKills,
			AdminApi.CustomLeaderboardCriteriaType.GigapedeKills => CustomLeaderboardCriteriaType.GigapedeKills,
			AdminApi.CustomLeaderboardCriteriaType.GhostpedeKills => CustomLeaderboardCriteriaType.GhostpedeKills,
			AdminApi.CustomLeaderboardCriteriaType.Spider1Kills => CustomLeaderboardCriteriaType.Spider1Kills,
			AdminApi.CustomLeaderboardCriteriaType.Spider2Kills => CustomLeaderboardCriteriaType.Spider2Kills,
			AdminApi.CustomLeaderboardCriteriaType.LeviathanKills => CustomLeaderboardCriteriaType.LeviathanKills,
			AdminApi.CustomLeaderboardCriteriaType.OrbKills => CustomLeaderboardCriteriaType.OrbKills,
			AdminApi.CustomLeaderboardCriteriaType.ThornKills => CustomLeaderboardCriteriaType.ThornKills,
			AdminApi.CustomLeaderboardCriteriaType.Skull1sAlive => CustomLeaderboardCriteriaType.Skull1sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Skull2sAlive => CustomLeaderboardCriteriaType.Skull2sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Skull3sAlive => CustomLeaderboardCriteriaType.Skull3sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Skull4sAlive => CustomLeaderboardCriteriaType.Skull4sAlive,
			AdminApi.CustomLeaderboardCriteriaType.SpiderlingsAlive => CustomLeaderboardCriteriaType.SpiderlingsAlive,
			AdminApi.CustomLeaderboardCriteriaType.SpiderEggsAlive => CustomLeaderboardCriteriaType.SpiderEggsAlive,
			AdminApi.CustomLeaderboardCriteriaType.Squid1sAlive => CustomLeaderboardCriteriaType.Squid1sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Squid2sAlive => CustomLeaderboardCriteriaType.Squid2sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Squid3sAlive => CustomLeaderboardCriteriaType.Squid3sAlive,
			AdminApi.CustomLeaderboardCriteriaType.CentipedesAlive => CustomLeaderboardCriteriaType.CentipedesAlive,
			AdminApi.CustomLeaderboardCriteriaType.GigapedesAlive => CustomLeaderboardCriteriaType.GigapedesAlive,
			AdminApi.CustomLeaderboardCriteriaType.GhostpedesAlive => CustomLeaderboardCriteriaType.GhostpedesAlive,
			AdminApi.CustomLeaderboardCriteriaType.Spider1sAlive => CustomLeaderboardCriteriaType.Spider1sAlive,
			AdminApi.CustomLeaderboardCriteriaType.Spider2sAlive => CustomLeaderboardCriteriaType.Spider2sAlive,
			AdminApi.CustomLeaderboardCriteriaType.LeviathansAlive => CustomLeaderboardCriteriaType.LeviathansAlive,
			AdminApi.CustomLeaderboardCriteriaType.OrbsAlive => CustomLeaderboardCriteriaType.OrbsAlive,
			AdminApi.CustomLeaderboardCriteriaType.ThornsAlive => CustomLeaderboardCriteriaType.ThornsAlive,
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
			MainApi.CustomLeaderboardCriteriaType.Skull1Kills => CustomLeaderboardCriteriaType.Skull1Kills,
			MainApi.CustomLeaderboardCriteriaType.Skull2Kills => CustomLeaderboardCriteriaType.Skull2Kills,
			MainApi.CustomLeaderboardCriteriaType.Skull3Kills => CustomLeaderboardCriteriaType.Skull3Kills,
			MainApi.CustomLeaderboardCriteriaType.Skull4Kills => CustomLeaderboardCriteriaType.Skull4Kills,
			MainApi.CustomLeaderboardCriteriaType.SpiderlingKills => CustomLeaderboardCriteriaType.SpiderlingKills,
			MainApi.CustomLeaderboardCriteriaType.SpiderEggKills => CustomLeaderboardCriteriaType.SpiderEggKills,
			MainApi.CustomLeaderboardCriteriaType.Squid1Kills => CustomLeaderboardCriteriaType.Squid1Kills,
			MainApi.CustomLeaderboardCriteriaType.Squid2Kills => CustomLeaderboardCriteriaType.Squid2Kills,
			MainApi.CustomLeaderboardCriteriaType.Squid3Kills => CustomLeaderboardCriteriaType.Squid3Kills,
			MainApi.CustomLeaderboardCriteriaType.CentipedeKills => CustomLeaderboardCriteriaType.CentipedeKills,
			MainApi.CustomLeaderboardCriteriaType.GigapedeKills => CustomLeaderboardCriteriaType.GigapedeKills,
			MainApi.CustomLeaderboardCriteriaType.GhostpedeKills => CustomLeaderboardCriteriaType.GhostpedeKills,
			MainApi.CustomLeaderboardCriteriaType.Spider1Kills => CustomLeaderboardCriteriaType.Spider1Kills,
			MainApi.CustomLeaderboardCriteriaType.Spider2Kills => CustomLeaderboardCriteriaType.Spider2Kills,
			MainApi.CustomLeaderboardCriteriaType.LeviathanKills => CustomLeaderboardCriteriaType.LeviathanKills,
			MainApi.CustomLeaderboardCriteriaType.OrbKills => CustomLeaderboardCriteriaType.OrbKills,
			MainApi.CustomLeaderboardCriteriaType.ThornKills => CustomLeaderboardCriteriaType.ThornKills,
			MainApi.CustomLeaderboardCriteriaType.Skull1sAlive => CustomLeaderboardCriteriaType.Skull1sAlive,
			MainApi.CustomLeaderboardCriteriaType.Skull2sAlive => CustomLeaderboardCriteriaType.Skull2sAlive,
			MainApi.CustomLeaderboardCriteriaType.Skull3sAlive => CustomLeaderboardCriteriaType.Skull3sAlive,
			MainApi.CustomLeaderboardCriteriaType.Skull4sAlive => CustomLeaderboardCriteriaType.Skull4sAlive,
			MainApi.CustomLeaderboardCriteriaType.SpiderlingsAlive => CustomLeaderboardCriteriaType.SpiderlingsAlive,
			MainApi.CustomLeaderboardCriteriaType.SpiderEggsAlive => CustomLeaderboardCriteriaType.SpiderEggsAlive,
			MainApi.CustomLeaderboardCriteriaType.Squid1sAlive => CustomLeaderboardCriteriaType.Squid1sAlive,
			MainApi.CustomLeaderboardCriteriaType.Squid2sAlive => CustomLeaderboardCriteriaType.Squid2sAlive,
			MainApi.CustomLeaderboardCriteriaType.Squid3sAlive => CustomLeaderboardCriteriaType.Squid3sAlive,
			MainApi.CustomLeaderboardCriteriaType.CentipedesAlive => CustomLeaderboardCriteriaType.CentipedesAlive,
			MainApi.CustomLeaderboardCriteriaType.GigapedesAlive => CustomLeaderboardCriteriaType.GigapedesAlive,
			MainApi.CustomLeaderboardCriteriaType.GhostpedesAlive => CustomLeaderboardCriteriaType.GhostpedesAlive,
			MainApi.CustomLeaderboardCriteriaType.Spider1sAlive => CustomLeaderboardCriteriaType.Spider1sAlive,
			MainApi.CustomLeaderboardCriteriaType.Spider2sAlive => CustomLeaderboardCriteriaType.Spider2sAlive,
			MainApi.CustomLeaderboardCriteriaType.LeviathansAlive => CustomLeaderboardCriteriaType.LeviathansAlive,
			MainApi.CustomLeaderboardCriteriaType.OrbsAlive => CustomLeaderboardCriteriaType.OrbsAlive,
			MainApi.CustomLeaderboardCriteriaType.ThornsAlive => CustomLeaderboardCriteriaType.ThornsAlive,
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
			CustomLeaderboardCriteriaType.GemsCollected => "#f00",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.GemsDespawned => "#888",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.GemsEaten => "#0f0",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.EnemiesKilled => "#f80",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.DaggersFired => "#f80",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.DaggersHit => "#f80",// TODO: Use same color as graph
			CustomLeaderboardCriteriaType.HomingStored => UpgradesV3_2.Level4.Color.HexCode,
			CustomLeaderboardCriteriaType.HomingEaten => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull1Kills => EnemiesV3_2.Skull1.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull2Kills => EnemiesV3_2.Skull2.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull3Kills => EnemiesV3_2.Skull3.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull4Kills => EnemiesV3_2.Skull4.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderlingKills => EnemiesV3_2.Spiderling.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderEggKills => EnemiesV3_2.SpiderEgg1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid1Kills => EnemiesV3_2.Squid1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid2Kills => EnemiesV3_2.Squid2.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid3Kills => EnemiesV3_2.Squid3.Color.HexCode,
			CustomLeaderboardCriteriaType.CentipedeKills => EnemiesV3_2.Centipede.Color.HexCode,
			CustomLeaderboardCriteriaType.GigapedeKills => EnemiesV3_2.Gigapede.Color.HexCode,
			CustomLeaderboardCriteriaType.GhostpedeKills => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider1Kills => EnemiesV3_2.Spider1.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider2Kills => EnemiesV3_2.Spider2.Color.HexCode,
			CustomLeaderboardCriteriaType.LeviathanKills => EnemiesV3_2.Leviathan.Color.HexCode,
			CustomLeaderboardCriteriaType.OrbKills => EnemiesV3_2.TheOrb.Color.HexCode,
			CustomLeaderboardCriteriaType.ThornKills => EnemiesV3_2.Thorn.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull1sAlive => EnemiesV3_2.Skull1.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull2sAlive => EnemiesV3_2.Skull2.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull3sAlive => EnemiesV3_2.Skull3.Color.HexCode,
			CustomLeaderboardCriteriaType.Skull4sAlive => EnemiesV3_2.Skull4.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderlingsAlive => EnemiesV3_2.Spiderling.Color.HexCode,
			CustomLeaderboardCriteriaType.SpiderEggsAlive => EnemiesV3_2.SpiderEgg1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid1sAlive => EnemiesV3_2.Squid1.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid2sAlive => EnemiesV3_2.Squid2.Color.HexCode,
			CustomLeaderboardCriteriaType.Squid3sAlive => EnemiesV3_2.Squid3.Color.HexCode,
			CustomLeaderboardCriteriaType.CentipedesAlive => EnemiesV3_2.Centipede.Color.HexCode,
			CustomLeaderboardCriteriaType.GigapedesAlive => EnemiesV3_2.Gigapede.Color.HexCode,
			CustomLeaderboardCriteriaType.GhostpedesAlive => EnemiesV3_2.Ghostpede.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider1sAlive => EnemiesV3_2.Spider1.Color.HexCode,
			CustomLeaderboardCriteriaType.Spider2sAlive => EnemiesV3_2.Spider2.Color.HexCode,
			CustomLeaderboardCriteriaType.LeviathansAlive => EnemiesV3_2.Leviathan.Color.HexCode,
			CustomLeaderboardCriteriaType.OrbsAlive => EnemiesV3_2.TheOrb.Color.HexCode,
			CustomLeaderboardCriteriaType.ThornsAlive => EnemiesV3_2.Thorn.Color.HexCode,
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
