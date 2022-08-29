namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV2_0
{
	public static readonly Enemy Squid1 = new(GameVersion.V2_0, "Squid I", EnemyColors.Squid1, 10, 1, 2, DeathsV2_0.Purged, new(10, 10, 0, 4, 4), 3);
	public static readonly Enemy Squid2 = new(GameVersion.V2_0, "Squid II", EnemyColors.Squid2, 20, 2, 3, DeathsV2_0.Desecrated, new(10, 10, 0, 4, 4), 39);
	public static readonly Enemy Squid3 = new(GameVersion.V2_0, "Squid III", EnemyColors.Squid3, 90, 3, 3, DeathsV2_0.Sacrificed, new(30, 10, 0, 4, 4), 244);
	public static readonly Enemy Centipede = new(GameVersion.V2_0, "Centipede", EnemyColors.Centipede, 75, 25, 25, DeathsV2_0.Eviscerated, new(10, 0, 10, 4, 4), 114);
	public static readonly Enemy Gigapede = new(GameVersion.V2_0, "Gigapede", EnemyColors.GigapedeRed, 250, 50, 50, DeathsV2_0.Annihilated, new(10, 0, 10, 4, 4), 259);
	public static readonly Enemy Leviathan = new(GameVersion.V2_0, "Leviathan", EnemyColors.Leviathan, 1500, 6, 6, DeathsV2_0.Devastated, new(1, 1, 0, 4, 4), 350);
	public static readonly Enemy Spider1 = new(GameVersion.V2_0, "Spider I", EnemyColors.Spider1, 25, 1, 1, DeathsV2_0.Infested, new(10, 10, 0, 4, 4), 39);
	public static readonly Enemy Spider2 = new(GameVersion.V2_0, "Spider II", EnemyColors.Spider2, 200, 1, 1, DeathsV2_0.Envenomated, new(10, 10, 0, 4, 4), 274);
	public static readonly Enemy Skull1 = new(GameVersion.V2_0, "Skull I", EnemyColors.Skull1, 1, 0, 0, DeathsV2_0.Swarmed, new(10, 10, 0, 1, 1), null, Squid1, Squid2, Squid3);
	public static readonly Enemy Skull2 = new(GameVersion.V2_0, "Skull II", EnemyColors.Skull2, 5, 1, 1, DeathsV2_0.Impaled, new(10, 10, 0, 4, 4), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersion.V2_0, "Skull III", EnemyColors.Skull3, 10, 1, 1, DeathsV2_0.Gored, new(10, 10, 0, 4, 4), null, Squid2);
	public static readonly Enemy Skull4 = new(GameVersion.V2_0, "Skull IV", EnemyColors.Skull4, 100, 0, 0, DeathsV2_0.Opened, new(10, 10, 0, 4, 4), null, Squid3);
	public static readonly Enemy TransmutedSkull1 = new(GameVersion.V2_0, "Transmuted Skull I", EnemyColors.TransmutedSkull1, 10, 0, 0, DeathsV2_0.Swarmed, new(1, 1, 0, 4, 4), null, Leviathan);
	public static readonly Enemy TransmutedSkull2 = new(GameVersion.V2_0, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 20, 1, 1, DeathsV2_0.Impaled, new(10, 10, 0, 4, 4), null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersion.V2_0, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 100, 1, 1, DeathsV2_0.Gored, new(10, 10, 0, 4, 4), null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersion.V2_0, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 300, 0, 0, DeathsV2_0.Opened, new(10, 10, 0, 4, 4), null, Leviathan);
	public static readonly Enemy SpiderEgg1 = new(GameVersion.V2_0, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, DeathsV2_0.Infested, new(1, 1, 0, 4, 4), null, Spider1);
	public static readonly Enemy SpiderEgg2 = new(GameVersion.V2_0, "Spider Egg II", EnemyColors.SpiderEgg2, 3, 0, 0, DeathsV2_0.Envenomated, new(1, 1, 0, 4, 4), null, Spider2);
	public static readonly Enemy Spiderling = new(GameVersion.V2_0, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, DeathsV2_0.Stricken, new(1, 1, 0, 4, 4), null, SpiderEgg1, SpiderEgg2);
	public static readonly Enemy Andras = new(GameVersion.V2_0, "Andras", EnemyColors.Andras, 25, 1, 1, DeathsV2_0.None, new(0, 0, 0, 0, 0), null);

	internal static readonly IReadOnlyList<Enemy> All = new List<Enemy>
	{
		Squid1,
		Squid2,
		Squid3,
		Centipede,
		Gigapede,
		Leviathan,
		Spider1,
		Spider2,
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
		Andras,
	};
}
