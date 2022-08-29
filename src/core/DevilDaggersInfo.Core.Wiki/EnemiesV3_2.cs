namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV3_2
{
	public static readonly Enemy Squid1 = new(GameVersion.V3_2, "Squid I", EnemyColors.Squid1, 10, 1, 2, DeathsV3_2.Purged, new(10, 10, 0, 4, 4), 3);
	public static readonly Enemy Squid2 = new(GameVersion.V3_2, "Squid II", EnemyColors.Squid2, 20, 2, 3, DeathsV3_2.Desecrated, new(10, 10, 0, 4, 4), 39);
	public static readonly Enemy Squid3 = new(GameVersion.V3_2, "Squid III", EnemyColors.Squid3, 90, 3, 3, DeathsV3_2.Sacrificed, new(30, 10, 0, 4, 4), 244);
	public static readonly Enemy Centipede = new(GameVersion.V3_2, "Centipede", EnemyColors.Centipede, 75, 25, 25, DeathsV3_2.Eviscerated, new(10, 0, 10, 4, 4), 114);
	public static readonly Enemy Gigapede = new(GameVersion.V3_2, "Gigapede", EnemyColors.Gigapede, 250, 50, 50, DeathsV3_2.Annihilated, new(10, 0, 10, 4, 4), 259);
	public static readonly Enemy Ghostpede = new(GameVersion.V3_2, "Ghostpede", EnemyColors.Ghostpede, 500, 10, 10, DeathsV3_2.Haunted, new(0, 0, 10, 0, 0), 442);
	public static readonly Enemy Leviathan = new(GameVersion.V3_2, "Leviathan", EnemyColors.Leviathan, 1500, 6, 6, DeathsV3_2.Incarnated, new(1, 1, 0, 4, 4), 350);
	public static readonly Enemy Spider1 = new(GameVersion.V3_2, "Spider I", EnemyColors.Spider1, 25, 1, 1, DeathsV3_2.Intoxicated, new(10, 10, 0, 4, 4), 39);
	public static readonly Enemy Spider2 = new(GameVersion.V3_2, "Spider II", EnemyColors.Spider2, 200, 1, 1, DeathsV3_2.Envenomated, new(10, 10, 0, 4, 4), 274);
	public static readonly Enemy Thorn = new(GameVersion.V3_2, "Thorn", EnemyColors.Thorn, 120, 0, 0, DeathsV3_2.Entangled, new(10, 10, 1, 4, 4), 447);
	public static readonly Enemy TheOrb = new(GameVersion.V3_2, "The Orb", EnemyColors.TheOrb, 2400, 0, 0, DeathsV3_2.Discarnated, new(1, 1, 0, 4, 4), null, Leviathan);
	public static readonly Enemy Skull1 = new(GameVersion.V3_2, "Skull I", EnemyColors.Skull1, 1, 0, 0, DeathsV3_2.Swarmed, new(10, 10, 0, 1, 1), null, Squid1, Squid2, Squid3);
	public static readonly Enemy Skull2 = new(GameVersion.V3_2, "Skull II", EnemyColors.Skull2, 5, 1, 1, DeathsV3_2.Impaled, new(10, 10, 0, 4, 4), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersion.V3_2, "Skull III", EnemyColors.Skull3, 10, 1, 1, DeathsV3_2.Gored, new(10, 10, 0, 4, 4), null, Squid2);
	public static readonly Enemy Skull4 = new(GameVersion.V3_2, "Skull IV", EnemyColors.Skull4, 100, 0, 0, DeathsV3_2.Opened, new(10, 10, 0, 4, 4), null, Squid3);
	public static readonly Enemy TransmutedSkull1 = new(GameVersion.V3_2, "Transmuted Skull I", EnemyColors.TransmutedSkull1, 10, 0, 0, DeathsV3_2.Swarmed, new(10, 10, 0, 1, 1), null, Leviathan, TheOrb);
	public static readonly Enemy TransmutedSkull2 = new(GameVersion.V3_2, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 20, 1, 1, DeathsV3_2.Impaled, new(10, 10, 0, 4, 4), null, Leviathan, TheOrb);
	public static readonly Enemy TransmutedSkull3 = new(GameVersion.V3_2, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 100, 1, 1, DeathsV3_2.Gored, new(10, 10, 0, 4, 4), null, Leviathan, TheOrb);
	public static readonly Enemy TransmutedSkull4 = new(GameVersion.V3_2, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 300, 0, 0, DeathsV3_2.Opened, new(10, 10, 0, 4, 4), null, Leviathan, TheOrb);
	public static readonly Enemy SpiderEgg1 = new(GameVersion.V3_2, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, DeathsV3_2.Intoxicated, new(10, 10, 0, 4, 4), null, Spider1);
	public static readonly Enemy SpiderEgg2 = new(GameVersion.V3_2, "Spider Egg II", EnemyColors.SpiderEgg2, 3, 0, 0, DeathsV3_2.Envenomated, new(10, 10, 0, 4, 4), null, Spider2);
	public static readonly Enemy Spiderling = new(GameVersion.V3_2, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, DeathsV3_2.Infested, new(10, 10, 0, 4, 4), null, SpiderEgg1, SpiderEgg2);

	internal static readonly IReadOnlyList<Enemy> All = new List<Enemy>
	{
		Squid1,
		Squid2,
		Squid3,
		Centipede,
		Gigapede,
		Ghostpede,
		Leviathan,
		Spider1,
		Spider2,
		Thorn,
		TheOrb,
		Skull1,
		Skull2,
		Skull3,
		Skull4,
		TransmutedSkull1,
		TransmutedSkull2,
		TransmutedSkull3,
		TransmutedSkull4,
		SpiderEgg1,
		SpiderEgg2,
		Spiderling,
	};
}
