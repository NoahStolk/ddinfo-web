using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Core.CustomLeaderboards;
using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCriteriaTypeExtensions
{
	public static CustomLeaderboardCriteriaType ToCore(this AdminApi.CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
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

	public static string GetColor(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => "#f00", // TODO: Use same color as graph
		CustomLeaderboardCriteriaType.GemsDespawned => "#888", // TODO: Use same color as graph
		CustomLeaderboardCriteriaType.GemsEaten => "#0f0", // TODO: Use same color as graph
		CustomLeaderboardCriteriaType.EnemiesKilled => "#f80", // TODO: Use same color as graph
		CustomLeaderboardCriteriaType.DaggersFired => "#f80", // TODO: Use same color as graph
		CustomLeaderboardCriteriaType.DaggersHit => "#f80", // TODO: Use same color as graph
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
