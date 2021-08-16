namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV3_1
{
	public static readonly Enemy Squid1 = new(GameVersions.V3_1, "Squid I", EnemyColors.Squid1, 10, 1, 2, Deaths.Squid1_Purged, new(1, 1), 3);
	public static readonly Enemy Squid2 = new(GameVersions.V3_1, "Squid II", EnemyColors.Squid2, 20, 2, 3, Deaths.Squid2_Desecrated, new(2, 1), 39);
	public static readonly Enemy Squid3 = new(GameVersions.V3_1, "Squid III", EnemyColors.Squid3, 90, 3, 3, Deaths.Squid3_Sacrificed, new(3, 9), 244);
	public static readonly Enemy Centipede = new(GameVersions.V3_1, "Centipede", EnemyColors.Centipede, 75, 25, 25, Deaths.Centipede_Eviscerated, new(25, 25), 114);
	public static readonly Enemy Gigapede = new(GameVersions.V3_1, "Gigapede", EnemyColors.Gigapede, 250, 50, 50, Deaths.Gigapede_Annihilated, new(50, 50), 259);
	public static readonly Enemy Ghostpede = new(GameVersions.V3_1, "Ghostpede", EnemyColors.Ghostpede, 500, 10, 10, Deaths.Ghostpede_Haunted, new(null, null), 442);
	public static readonly Enemy Leviathan = new(GameVersions.V3_1, "Leviathan", EnemyColors.Leviathan, 1500, 6, 6, Deaths.Leviathan_Incarnated, new(1500, 1500), 350);
	public static readonly Enemy TheOrb = new(GameVersions.V3_1, "The Orb", EnemyColors.TheOrb, 2400, 0, 0, Deaths.TheOrb_Discarnated, new(2400, 2400), null, Leviathan);
	public static readonly Enemy Thorn = new(GameVersions.V3_1, "Thorn", EnemyColors.Thorn, 120, 0, 0, Deaths.Thorn_Entangled, new(12, 12), 447);
	public static readonly Enemy Spider1 = new(GameVersions.V3_1, "Spider I", EnemyColors.Spider1, 25, 1, 1, Deaths.Spider1_Intoxicated, new(3, 3), 39);
	public static readonly Enemy Spider2 = new(GameVersions.V3_1, "Spider II", EnemyColors.Spider2, 200, 1, 1, Deaths.Spider2_Envenomated, new(20, 20), 274);

	public static readonly Enemy Skull1 = new(GameVersions.V3_1, "Skull I", EnemyColors.Skull1, 1, 0, 0, Deaths.Skull1_Swarmed, new(0.25f, 0.25f), null, Squid1, Squid2, Squid3);
	public static readonly Enemy Skull2 = new(GameVersions.V3_1, "Skull II", EnemyColors.Skull2, 5, 1, 1, Deaths.Skull2_Impaled, new(1, 1), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersions.V3_1, "Skull III", EnemyColors.Skull3, 10, 1, 1, Deaths.Skull3_Gored, new(1, 1), null, Squid2);
	public static readonly Enemy Skull4 = new(GameVersions.V3_1, "Skull IV", EnemyColors.Skull4, 100, 0, 0, Deaths.Skull4_Opened, new(10, 10), null, Squid3);

	public static readonly Enemy TransmutedSkull1 = new(GameVersions.V3_1, "Transmuted Skull I", EnemyColors.TransmutedSkull1, 10, 0, 0, Deaths.Skull1_Swarmed, new(0.25f, 10), null, Leviathan);
	public static readonly Enemy TransmutedSkull2 = new(GameVersions.V3_1, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 20, 1, 1, Deaths.Skull2_Impaled, new(2, 2), null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersions.V3_1, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 100, 1, 1, Deaths.Skull3_Gored, new(10, 10), null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersions.V3_1, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 300, 0, 0, Deaths.Skull4_Opened, new(30, 30), null, Leviathan);

	public static readonly Enemy SpiderEgg1 = new(GameVersions.V3_1, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, Deaths.SpiderEgg1_Intoxicated, new(3, 3), null, Spider1);
	public static readonly Enemy SpiderEgg2 = new(GameVersions.V3_1, "Spider Egg II", EnemyColors.SpiderEgg2, 3, 0, 0, Deaths.SpiderEgg2_Envenomated, new(3, 3), null, Spider2);
	public static readonly Enemy Spiderling = new(GameVersions.V3_1, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, Deaths.Spiderling_Infested, new(1, 1), null, SpiderEgg1, SpiderEgg2);

	public static readonly IEnumerable<Enemy> All = typeof(EnemiesV3_1).GetFields().Where(f => f.FieldType == typeof(Enemy)).Select(f => (Enemy)f.GetValue(null)!);
}
