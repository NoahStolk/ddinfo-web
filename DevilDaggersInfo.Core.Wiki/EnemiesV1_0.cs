namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV1_0
{
	public static readonly Enemy Squid1 = new(GameVersions.V1_0, "Squid I", EnemyColors.Squid1, 10, 1, 2, Deaths.Squid1_Purged, new(1, null), 3);
	public static readonly Enemy Squid2 = new(GameVersions.V1_0, "Squid II", EnemyColors.Squid2, 20, 2, 3, Deaths.Squid2_Sacrificed, new(2, null), 39);
	public static readonly Enemy Centipede = new(GameVersions.V1_0, "Centipede", EnemyColors.Centipede, 75, 25, 25, Deaths.Centipede_Eviscerated, new(25, null), 114);
	public static readonly Enemy Gigapede = new(GameVersions.V1_0, "Gigapede", EnemyColors.Gigapede_V1_0_V2_0, 250, 50, 50, Deaths.Gigapede_V1_0_V2_0_Eviscerated, new(50, null), 264);
	public static readonly Enemy Leviathan = new(GameVersions.V1_0, "Leviathan", EnemyColors.Leviathan, 600, 6, 6, Deaths.Leviathan_Devastated, new(600, null), 397);
	public static readonly Enemy Spider1 = new(GameVersions.V1_0, "Spider I", EnemyColors.Spider1, 25, 1, 1, Deaths.Spider1_Infested, new(3, null), 39);

	public static readonly Enemy Skull1 = new(GameVersions.V1_0, "Skull I", EnemyColors.Skull1, 1, 0, 0, Deaths.Skull1_Swarmed, new(0.25f, null), null, Squid1, Squid2);
	public static readonly Enemy Skull2 = new(GameVersions.V1_0, "Skull II", EnemyColors.Skull2, 5, 1, 1, Deaths.Skull2_Impaled, new(1, null), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersions.V1_0, "Skull III", EnemyColors.Skull3, 10, 1, 1, Deaths.Skull3_Dismembered, new(1, null), null, Squid2);

	public static readonly Enemy TransmutedSkull2 = new(GameVersions.V1_0, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 10, 1, 1, Deaths.Skull2_Impaled, new(1, null), null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersions.V1_0, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 20, 1, 1, Deaths.Skull3_Dismembered, new(2, null), null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersions.V1_0, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 100, 0, 0, Deaths.TransmutedSkull4_Annihilated, new(10, null), null, Leviathan);

	public static readonly Enemy SpiderEgg1 = new(GameVersions.V1_0, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, Deaths.SpiderEgg1_Infested, new(3, null), null, Spider1);
	public static readonly Enemy Spiderling = new(GameVersions.V1_0, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, Deaths.Spiderling_Stricken, new(1, null), null, SpiderEgg1);

	public static readonly IEnumerable<Enemy> All = typeof(EnemiesV1_0).GetFields().Where(f => f.FieldType == typeof(Enemy)).Select(f => (Enemy)f.GetValue(null)!);
}
