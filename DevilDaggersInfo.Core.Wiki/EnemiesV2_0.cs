﻿namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV2_0
{
	public static readonly Enemy Squid1 = new(GameVersions.V2_0, "Squid I", EnemyColors.Squid1, 10, 1, 2, Deaths.Squid1_Purged, new(1, 1), 3);
	public static readonly Enemy Squid2 = new(GameVersions.V2_0, "Squid II", EnemyColors.Squid2, 20, 2, 3, Deaths.Squid2_Desecrated, new(2, 1), 39);
	public static readonly Enemy Squid3 = new(GameVersions.V2_0, "Squid III", EnemyColors.Squid3, 90, 3, 3, Deaths.Squid3_Sacrificed, new(3, 9), 244);
	public static readonly Enemy Centipede = new(GameVersions.V2_0, "Centipede", EnemyColors.Centipede, 75, 25, 25, Deaths.Centipede_Eviscerated, new(25, 25), 114);
	public static readonly Enemy Gigapede = new(GameVersions.V2_0, "Gigapede", EnemyColors.Gigapede_V1_0_V2_0, 250, 50, 50, Deaths.Gigapede_V1_0_V2_0_Annihilated, new(50, 50), 259);
	public static readonly Enemy Leviathan = new(GameVersions.V2_0, "Leviathan", EnemyColors.Leviathan, 1500, 6, 6, Deaths.Leviathan_Devastated, new(1500, 1500), 350);
	public static readonly Enemy Spider1 = new(GameVersions.V2_0, "Spider I", EnemyColors.Spider1, 25, 1, 1, Deaths.Spider1_Infested, new(3, 3), 39);
	public static readonly Enemy Spider2 = new(GameVersions.V2_0, "Spider II", EnemyColors.Spider2, 200, 1, 1, Deaths.Spider2_Envenomated, new(20, 20), 274);

	public static readonly Enemy Skull1 = new(GameVersions.V2_0, "Skull I", EnemyColors.Skull1, 1, 0, 0, Deaths.Skull1_Swarmed, new(0.25f, 0.25f), null, Squid1, Squid2, Squid3);
	public static readonly Enemy Skull2 = new(GameVersions.V2_0, "Skull II", EnemyColors.Skull2, 5, 1, 1, Deaths.Skull2_Impaled, new(1, 1), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersions.V2_0, "Skull III", EnemyColors.Skull3, 10, 1, 1, Deaths.Skull3_Gored, new(1, 1), null, Squid2);
	public static readonly Enemy Skull4 = new(GameVersions.V2_0, "Skull IV", EnemyColors.Skull4, 100, 0, 0, Deaths.Skull4_Opened, new(10, 10), null, Squid3);

	public static readonly Enemy TransmutedSkull1 = new(GameVersions.V2_0, "Transmuted Skull I", EnemyColors.TransmutedSkull1, 10, 0, 0, Deaths.Skull1_Swarmed, new(0.25f, 10), null, Leviathan);
	public static readonly Enemy TransmutedSkull2 = new(GameVersions.V2_0, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 20, 1, 1, Deaths.Skull2_Impaled, new(2, 2), null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersions.V2_0, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 100, 1, 1, Deaths.Skull3_Gored, new(10, 10), null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersions.V2_0, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 300, 0, 0, Deaths.Skull4_Opened, new(30, 30), null, Leviathan);

	public static readonly Enemy SpiderEgg1 = new(GameVersions.V2_0, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, Deaths.SpiderEgg1_Infested, new(3, 3), null, Spider1);
	public static readonly Enemy SpiderEgg2 = new(GameVersions.V2_0, "Spider Egg II", EnemyColors.SpiderEgg2, 3, 0, 0, Deaths.SpiderEgg2_Envenomated, new(3, 3), null, Spider2);
	public static readonly Enemy Spiderling = new(GameVersions.V2_0, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, Deaths.Spiderling_Stricken, new(1, 1), null, SpiderEgg1, SpiderEgg2);

	public static readonly Enemy Andras = new(GameVersions.V2_0, "Andras", EnemyColors.Andras, 25, 1, 1, Deaths.None, new(null, null), null);

	public static readonly IEnumerable<Enemy> All = typeof(EnemiesV2_0).GetFields().Where(f => f.FieldType == typeof(Enemy)).Select(f => (Enemy)f.GetValue(null)!);
}
