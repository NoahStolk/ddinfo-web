namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV31
{
	public static readonly Enemy Squid1 = new(GameVersions.V31, "Squid I", EnemyColors.Squid1, 10, 1, 2, Deaths.Squid1Purged, 1, 1, 3);
	public static readonly Enemy Squid2 = new(GameVersions.V31, "Squid II", EnemyColors.Squid2, 20, 2, 3, Deaths.Squid2Desecrated, 2, 1, 39);
	public static readonly Enemy Squid3 = new(GameVersions.V31, "Squid III", EnemyColors.Squid3, 90, 3, 3, Deaths.Squid3Sacrificed, 3, 9, 244);
	public static readonly Enemy Centipede = new(GameVersions.V31, "Centipede", EnemyColors.Centipede, 75, 25, 25, Deaths.CentipedeEviscerated, 25, 25, 114);
	public static readonly Enemy Gigapede = new(GameVersions.V31, "Gigapede", EnemyColors.Gigapede, 250, 50, 50, Deaths.GigapedeAnnihilated, 50, 50, 259);
	public static readonly Enemy Ghostpede = new(GameVersions.V31, "Ghostpede", EnemyColors.Ghostpede, 500, 10, 10, Deaths.GhostpedeHaunted, 0, 0, 442);
	public static readonly Enemy Leviathan = new(GameVersions.V31, "Leviathan", EnemyColors.Leviathan, 1500, 6, 6, Deaths.LeviathanIncarnated, 1500, 1500, 350);
	public static readonly Enemy TheOrb = new(GameVersions.V31, "The Orb", EnemyColors.TheOrb, 2400, 0, 0, Deaths.TheOrbDiscarnated, 2400, 2400, null, Leviathan);
	public static readonly Enemy Thorn = new(GameVersions.V31, "Thorn", EnemyColors.Thorn, 120, 0, 0, Deaths.ThornEntangled, 12, 12, 447);
	public static readonly Enemy Spider1 = new(GameVersions.V31, "Spider I", EnemyColors.Spider1, 25, 1, 1, Deaths.Spider1Intoxicated, 3, 3, 39);
	public static readonly Enemy Spider2 = new(GameVersions.V31, "Spider II", EnemyColors.Spider2, 200, 1, 1, Deaths.Spider2Envenomated, 20, 20, 274);

	public static readonly Enemy Skull1 = new(GameVersions.V31, "Skull I", EnemyColors.Skull1, 1, 0, 0, Deaths.Skull1Swarmed, 0.25f, 0.25f, null, Squid1, Squid2, Squid3);
	public static readonly Enemy Skull2 = new(GameVersions.V31, "Skull II", EnemyColors.Skull2, 5, 1, 1, Deaths.Skull2Impaled, 1, 1, null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersions.V31, "Skull III", EnemyColors.Skull3, 10, 1, 1, Deaths.Skull3Gored, 1, 1, null, Squid2);
	public static readonly Enemy Skull4 = new(GameVersions.V31, "Skull IV", EnemyColors.Skull4, 100, 0, 0, Deaths.Skull4Opened, 10, 10, null, Squid3);

	public static readonly Enemy TransmutedSkull1 = new(GameVersions.V31, "Transmuted Skull I", EnemyColors.TransmutedSkull1, 10, 0, 0, Deaths.Skull1Swarmed, 0.25f, 10, null, Leviathan);
	public static readonly Enemy TransmutedSkull2 = new(GameVersions.V31, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 20, 1, 1, Deaths.Skull2Impaled, 2, 2, null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersions.V31, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 100, 1, 1, Deaths.Skull3Gored, 10, 10, null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersions.V31, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 300, 0, 0, Deaths.Skull4Opened, 30, 30, null, Leviathan);

	public static readonly Enemy SpiderEgg1 = new(GameVersions.V31, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, Deaths.SpiderEgg1Intoxicated, 3, 3, null, Spider1);
	public static readonly Enemy SpiderEgg2 = new(GameVersions.V31, "Spider Egg II", EnemyColors.SpiderEgg2, 3, 0, 0, Deaths.SpiderEgg2Envenomated, 3, 3, null, Spider2);
	public static readonly Enemy Spiderling = new(GameVersions.V31, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, Deaths.SpiderlingInfested, 1, 1, null, SpiderEgg1, SpiderEgg2);
}
