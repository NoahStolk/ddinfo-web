// ReSharper disable StringLiteralTypo
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

	public static string GetIdentifier(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => "gems",
		CustomLeaderboardCriteriaType.GemsDespawned => "gemsdespawned",
		CustomLeaderboardCriteriaType.GemsEaten => "gemseaten",
		CustomLeaderboardCriteriaType.EnemiesKilled => "kills",
		CustomLeaderboardCriteriaType.DaggersFired => "daggers",
		CustomLeaderboardCriteriaType.DaggersHit => "hits",
		CustomLeaderboardCriteriaType.HomingStored => "homing",
		CustomLeaderboardCriteriaType.HomingEaten => "homingeaten",
		CustomLeaderboardCriteriaType.Skull1Kills => "skull1kills",
		CustomLeaderboardCriteriaType.Skull2Kills => "skull2kills",
		CustomLeaderboardCriteriaType.Skull3Kills => "skull3kills",
		CustomLeaderboardCriteriaType.Skull4Kills => "skull4kills",
		CustomLeaderboardCriteriaType.SpiderlingKills => "spiderlingkills",
		CustomLeaderboardCriteriaType.SpiderEggKills => "eggkills",
		CustomLeaderboardCriteriaType.Squid1Kills => "squid1kills",
		CustomLeaderboardCriteriaType.Squid2Kills => "squid2kills",
		CustomLeaderboardCriteriaType.Squid3Kills => "squid3kills",
		CustomLeaderboardCriteriaType.CentipedeKills => "centikills",
		CustomLeaderboardCriteriaType.GigapedeKills => "gigakills",
		CustomLeaderboardCriteriaType.GhostpedeKills => "ghostkills",
		CustomLeaderboardCriteriaType.Spider1Kills => "spider1kills",
		CustomLeaderboardCriteriaType.Spider2Kills => "spider2kills",
		CustomLeaderboardCriteriaType.LeviathanKills => "levikills",
		CustomLeaderboardCriteriaType.OrbKills => "orbkills",
		CustomLeaderboardCriteriaType.ThornKills => "thornkills",
		_ => throw new InvalidEnumConversionException(criteriaType),
	};

	public static string ToStringFast(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => nameof(CustomLeaderboardCriteriaType.GemsCollected),
		CustomLeaderboardCriteriaType.GemsDespawned => nameof(CustomLeaderboardCriteriaType.GemsDespawned),
		CustomLeaderboardCriteriaType.GemsEaten => nameof(CustomLeaderboardCriteriaType.GemsEaten),
		CustomLeaderboardCriteriaType.EnemiesKilled => nameof(CustomLeaderboardCriteriaType.EnemiesKilled),
		CustomLeaderboardCriteriaType.DaggersFired => nameof(CustomLeaderboardCriteriaType.DaggersFired),
		CustomLeaderboardCriteriaType.DaggersHit => nameof(CustomLeaderboardCriteriaType.DaggersHit),
		CustomLeaderboardCriteriaType.HomingStored => nameof(CustomLeaderboardCriteriaType.HomingStored),
		CustomLeaderboardCriteriaType.HomingEaten => nameof(CustomLeaderboardCriteriaType.HomingEaten),
		CustomLeaderboardCriteriaType.Skull1Kills => nameof(CustomLeaderboardCriteriaType.Skull1Kills),
		CustomLeaderboardCriteriaType.Skull2Kills => nameof(CustomLeaderboardCriteriaType.Skull2Kills),
		CustomLeaderboardCriteriaType.Skull3Kills => nameof(CustomLeaderboardCriteriaType.Skull3Kills),
		CustomLeaderboardCriteriaType.Skull4Kills => nameof(CustomLeaderboardCriteriaType.Skull4Kills),
		CustomLeaderboardCriteriaType.SpiderlingKills => nameof(CustomLeaderboardCriteriaType.SpiderlingKills),
		CustomLeaderboardCriteriaType.SpiderEggKills => nameof(CustomLeaderboardCriteriaType.SpiderEggKills),
		CustomLeaderboardCriteriaType.Squid1Kills => nameof(CustomLeaderboardCriteriaType.Squid1Kills),
		CustomLeaderboardCriteriaType.Squid2Kills => nameof(CustomLeaderboardCriteriaType.Squid2Kills),
		CustomLeaderboardCriteriaType.Squid3Kills => nameof(CustomLeaderboardCriteriaType.Squid3Kills),
		CustomLeaderboardCriteriaType.CentipedeKills => nameof(CustomLeaderboardCriteriaType.CentipedeKills),
		CustomLeaderboardCriteriaType.GigapedeKills => nameof(CustomLeaderboardCriteriaType.GigapedeKills),
		CustomLeaderboardCriteriaType.GhostpedeKills => nameof(CustomLeaderboardCriteriaType.GhostpedeKills),
		CustomLeaderboardCriteriaType.Spider1Kills => nameof(CustomLeaderboardCriteriaType.Spider1Kills),
		CustomLeaderboardCriteriaType.Spider2Kills => nameof(CustomLeaderboardCriteriaType.Spider2Kills),
		CustomLeaderboardCriteriaType.LeviathanKills => nameof(CustomLeaderboardCriteriaType.LeviathanKills),
		CustomLeaderboardCriteriaType.OrbKills => nameof(CustomLeaderboardCriteriaType.OrbKills),
		CustomLeaderboardCriteriaType.ThornKills => nameof(CustomLeaderboardCriteriaType.ThornKills),
		_ => throw new InvalidEnumConversionException(criteriaType),
	};
}
