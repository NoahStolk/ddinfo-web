namespace DevilDaggersInfo.Core.Wiki.Extensions;

public static class DeathTypeExtensions
{
	public static readonly Color Fallen = new(0xDD, 0xDD, 0xDD);
	public static readonly Color Unknown = new(0x66, 0x66, 0x66);

	public static Color GetColor(this DeathTypeV1 deathType) => deathType switch
	{
		DeathTypeV1.Fallen => Fallen,
		DeathTypeV1.Swarmed => EnemyColors.Skull1,
		DeathTypeV1.Impaled => EnemyColors.Skull2,
		DeathTypeV1.Infested => EnemyColors.SpiderEgg1,
		DeathTypeV1.Purged => EnemyColors.Squid1,
		DeathTypeV1.Sacrificed => EnemyColors.Squid2,
		DeathTypeV1.Eviscerated => EnemyColors.Centipede,
		DeathTypeV1.Annihilated => EnemyColors.TransmutedSkull4,
		DeathTypeV1.Stricken => EnemyColors.Spiderling,
		DeathTypeV1.Devastated => EnemyColors.Leviathan,
		DeathTypeV1.Dismembered => EnemyColors.Skull3,
		_ => Unknown,
	};

	public static Color GetColor(this DeathTypeV2 deathType) => deathType switch
	{
		DeathTypeV2.Fallen => Fallen,
		DeathTypeV2.Swarmed => EnemyColors.Skull1,
		DeathTypeV2.Impaled => EnemyColors.Skull2,
		DeathTypeV2.Gored => EnemyColors.Skull3,
		DeathTypeV2.Infested => EnemyColors.SpiderEgg1,
		DeathTypeV2.Opened => EnemyColors.Skull4,
		DeathTypeV2.Purged => EnemyColors.Squid1,
		DeathTypeV2.Desecrated => EnemyColors.Squid2,
		DeathTypeV2.Sacrificed => EnemyColors.Squid3,
		DeathTypeV2.Eviscerated => EnemyColors.Centipede,
		DeathTypeV2.Annihilated => EnemyColors.GigapedeOld,
		DeathTypeV2.Envenomated => EnemyColors.SpiderEgg2,
		DeathTypeV2.Stricken => EnemyColors.Spiderling,
		DeathTypeV2.Devastated => EnemyColors.Leviathan,
		_ => Unknown,
	};

	public static Color GetColor(this DeathTypeV3 deathType) => deathType switch
	{
		DeathTypeV3.Fallen => Fallen,
		DeathTypeV3.Swarmed => EnemyColors.Skull1,
		DeathTypeV3.Impaled => EnemyColors.Skull2,
		DeathTypeV3.Gored => EnemyColors.Skull3,
		DeathTypeV3.Infested => EnemyColors.Spiderling,
		DeathTypeV3.Opened => EnemyColors.Skull4,
		DeathTypeV3.Purged => EnemyColors.Squid1,
		DeathTypeV3.Desecrated => EnemyColors.Squid2,
		DeathTypeV3.Sacrificed => EnemyColors.Squid3,
		DeathTypeV3.Eviscerated => EnemyColors.Centipede,
		DeathTypeV3.Annihilated => EnemyColors.GigapedeOld,
		DeathTypeV3.Intoxicated => EnemyColors.SpiderEgg1,
		DeathTypeV3.Envenomated => EnemyColors.SpiderEgg2,
		DeathTypeV3.Incarnated => EnemyColors.Leviathan,
		DeathTypeV3.Discarnated => EnemyColors.TheOrb,
		DeathTypeV3.Barbed => EnemyColors.Thorn,
		_ => Unknown,
	};

	public static Color GetColor(this DeathTypeV31 deathType) => deathType switch
	{
		DeathTypeV31.Fallen => Fallen,
		DeathTypeV31.Swarmed => EnemyColors.Skull1,
		DeathTypeV31.Impaled => EnemyColors.Skull2,
		DeathTypeV31.Gored => EnemyColors.Skull3,
		DeathTypeV31.Infested => EnemyColors.Spiderling,
		DeathTypeV31.Opened => EnemyColors.Skull4,
		DeathTypeV31.Purged => EnemyColors.Squid1,
		DeathTypeV31.Desecrated => EnemyColors.Squid2,
		DeathTypeV31.Sacrificed => EnemyColors.Squid3,
		DeathTypeV31.Eviscerated => EnemyColors.Centipede,
		DeathTypeV31.Annihilated => EnemyColors.GigapedeOld,
		DeathTypeV31.Intoxicated => EnemyColors.SpiderEgg1,
		DeathTypeV31.Envenomated => EnemyColors.SpiderEgg2,
		DeathTypeV31.Incarnated => EnemyColors.Leviathan,
		DeathTypeV31.Discarnated => EnemyColors.TheOrb,
		DeathTypeV31.Entangled => EnemyColors.Thorn,
		DeathTypeV31.Haunted => EnemyColors.Ghostpede,
		_ => Unknown,
	};
}
