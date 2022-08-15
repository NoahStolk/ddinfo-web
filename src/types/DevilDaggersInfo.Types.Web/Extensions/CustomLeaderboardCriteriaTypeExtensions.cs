using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Types.Web.Extensions;

public static class CustomLeaderboardCriteriaTypeExtensions
{
	public static string Display(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => "Gems collected",
		CustomLeaderboardCriteriaType.GemsDespawned => "Gems despawned",
		CustomLeaderboardCriteriaType.GemsEaten => "Gems eaten",
		CustomLeaderboardCriteriaType.EnemiesKilled => "Total kills",
		CustomLeaderboardCriteriaType.DaggersFired => "Daggers fired",
		CustomLeaderboardCriteriaType.DaggersHit => "Daggers hit",
		CustomLeaderboardCriteriaType.HomingStored => "Homing stored",
		CustomLeaderboardCriteriaType.HomingEaten => "Homing eaten",
		CustomLeaderboardCriteriaType.Skull1Kills => "Skull I kills",
		CustomLeaderboardCriteriaType.Skull2Kills => "Skull II kills",
		CustomLeaderboardCriteriaType.Skull3Kills => "Skull III kills",
		CustomLeaderboardCriteriaType.Skull4Kills => "Skull IV kills",
		CustomLeaderboardCriteriaType.SpiderlingKills => "Spiderling kills",
		CustomLeaderboardCriteriaType.SpiderEggKills => "Spider Egg kills",
		CustomLeaderboardCriteriaType.Squid1Kills => "Squid I kills",
		CustomLeaderboardCriteriaType.Squid2Kills => "Squid II kills",
		CustomLeaderboardCriteriaType.Squid3Kills => "Squid III kills",
		CustomLeaderboardCriteriaType.CentipedeKills => "Centipede kills",
		CustomLeaderboardCriteriaType.GigapedeKills => "Gigapede kills",
		CustomLeaderboardCriteriaType.GhostpedeKills => "Ghostpede kills",
		CustomLeaderboardCriteriaType.Spider1Kills => "Spider I kills",
		CustomLeaderboardCriteriaType.Spider2Kills => "Spider II kills",
		CustomLeaderboardCriteriaType.LeviathanKills => "Leviathan kills",
		CustomLeaderboardCriteriaType.OrbKills => "Orb kills",
		CustomLeaderboardCriteriaType.ThornKills => "Thorn kills",
		_ => throw new InvalidEnumConversionException(criteriaType),
	};
}
