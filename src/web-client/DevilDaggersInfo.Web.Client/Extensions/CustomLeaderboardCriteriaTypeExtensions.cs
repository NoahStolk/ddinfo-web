using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardCriteriaTypeExtensions
{
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
		_ => throw new InvalidEnumConversionException(criteriaType),
	};
}