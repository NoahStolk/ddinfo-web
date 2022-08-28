namespace DevilDaggersInfo.Core.Wiki;

public static class EnemiesV1_0
{
	public static readonly Enemy Squid1 = new(GameVersion.V1_0, "Squid I", EnemyColors.Squid1, 10, 1, 2, DeathsV1_0.Purged, new(1f, null), 3);
	public static readonly Enemy Squid2 = new(GameVersion.V1_0, "Squid II", EnemyColors.Squid2, 20, 2, 3, DeathsV1_0.Sacrificed, new(2f, null), 39);
	public static readonly Enemy Centipede = new(GameVersion.V1_0, "Centipede", EnemyColors.Centipede, 75, 25, 25, DeathsV1_0.Eviscerated, new(25f, null), 114);
	public static readonly Enemy Gigapede = new(GameVersion.V1_0, "Gigapede", EnemyColors.GigapedeRed, 250, 50, 50, DeathsV1_0.Eviscerated, new(50f, null), 264);
	public static readonly Enemy Leviathan = new(GameVersion.V1_0, "Leviathan", EnemyColors.Leviathan, 600, 6, 6, DeathsV1_0.Devastated, new(600f, null), 397);
	public static readonly Enemy Spider1 = new(GameVersion.V1_0, "Spider I", EnemyColors.Spider1, 25, 1, 1, DeathsV1_0.Infested, new(3f, null), 39);
	public static readonly Enemy Skull1 = new(GameVersion.V1_0, "Skull I", EnemyColors.Skull1, 1, 0, 0, DeathsV1_0.Swarmed, new(0.25f, null), null, Squid1, Squid2);
	public static readonly Enemy Skull2 = new(GameVersion.V1_0, "Skull II", EnemyColors.Skull2, 5, 1, 1, DeathsV1_0.Impaled, new(1f, null), null, Squid1);
	public static readonly Enemy Skull3 = new(GameVersion.V1_0, "Skull III", EnemyColors.Skull3, 10, 1, 1, DeathsV1_0.Dismembered, new(1f, null), null, Squid2);
	public static readonly Enemy TransmutedSkull2 = new(GameVersion.V1_0, "Transmuted Skull II", EnemyColors.TransmutedSkull2, 10, 1, 1, DeathsV1_0.Impaled, new(1f, null), null, Leviathan);
	public static readonly Enemy TransmutedSkull3 = new(GameVersion.V1_0, "Transmuted Skull III", EnemyColors.TransmutedSkull3, 20, 1, 1, DeathsV1_0.Dismembered, new(2f, null), null, Leviathan);
	public static readonly Enemy TransmutedSkull4 = new(GameVersion.V1_0, "Transmuted Skull IV", EnemyColors.TransmutedSkull4, 100, 0, 0, DeathsV1_0.Annihilated, new(10f, null), null, Leviathan);
	public static readonly Enemy SpiderEgg1 = new(GameVersion.V1_0, "Spider Egg I", EnemyColors.SpiderEgg1, 3, 0, 0, DeathsV1_0.Infested, new(3f, null), null, Spider1);
	public static readonly Enemy Spiderling = new(GameVersion.V1_0, "Spiderling", EnemyColors.Spiderling, 3, 0, 0, DeathsV1_0.Stricken, new(1f, null), null, SpiderEgg1);

	internal static readonly IReadOnlyList<Enemy> All = new List<Enemy>
	{
		Squid1,
		Squid2,
		Centipede,
		Gigapede,
		Leviathan,
		Spider1,
		Skull1,
		Skull2,
		Skull3,
		TransmutedSkull2,
		TransmutedSkull3,
		TransmutedSkull4,
		SpiderEgg1,
		Spiderling,
	};
}
