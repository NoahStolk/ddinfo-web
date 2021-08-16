namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV1
{
	public static readonly Enemy Squid1 = new(GameVersions.V1, "Squid I", EnemyColors.Squid1, 10, 1, 2, Deaths.Squid1Purged, 1, 0, 3);
	public static readonly Enemy Squid2 = new(GameVersions.V1, "Squid II", EnemyColors.Squid2, 20, 2, 3, Deaths.Squid2Sacrificed, 2, 0, 39);
	public static readonly Enemy Centipede = new(GameVersions.V1, "Centipede", EnemyColors.Centipede, 75, 25, 25, Deaths.CentipedeEviscerated, 25, 0, 114);
	public static readonly Enemy Gigapede = new(GameVersions.V1, "Gigapede", EnemyColors.GigapedeV1V2, 250, 50, 50, Deaths.GigapedeV1V2Eviscerated, 50, 0, 264);
	public static readonly Enemy Leviathan = new(GameVersions.V1, "Leviathan", EnemyColors.Leviathan, 600, 6, 6, Deaths.LeviathanDevastated, 600, 0, 397);
	public static readonly Enemy Spider1 = new(GameVersions.V1, "Spider I", EnemyColors.Spider1, 25, 1, 1, Deaths.Spider1Infested, 3, 0, 39);

	public static readonly Enemy Skull1 = new(GameVersions.V1, "Skull I", EnemyColors.Skull1, 1, 0, 0, Deaths.Skull1Swarmed, 0.25f, 0, null, Squid1, Squid2);
	public static readonly Enemy Skull2 = new(GameVersions.V1, "Skull II", EnemyColors.Skull2, 5, 1, 1, Deaths.Skull2Impaled, 1, 0, null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersions.V1, "Skull III", EnemyColors.Skull3, 10, 1, 1, Deaths.Skull3Dismembered, 1, 0, null, Squid2);

	public static readonly Enemy TransmutedSkull2 = new(GameVersions.V1, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 10, 1, 1, Deaths.Skull2Impaled, 1, 0, null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersions.V1, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 20, 1, 1, Deaths.Skull3Dismembered, 2, 0, null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersions.V1, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 100, 0, 0, Deaths.TransmutedSkull4Annihilated, 10, 0, null, Leviathan);

	public static readonly Enemy SpiderEgg1 = new(GameVersions.V1, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, Deaths.SpiderEgg1Infested, 3, 0, null, Spider1);
	public static readonly Enemy Spiderling = new(GameVersions.V1, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, Deaths.SpiderlingStricken, 1, 0, null, SpiderEgg1);
}
