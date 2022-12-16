namespace DevilDaggersInfo.Core.Wiki;

public static class DeathsV3_0
{
	public static readonly Death Fallen = new(GameVersion.V3_0, "FALLEN", EnemyColors.Void, 0);
	public static readonly Death Swarmed = new(GameVersion.V3_0, "SWARMED", EnemyColors.Skull1, 1);
	public static readonly Death Impaled = new(GameVersion.V3_0, "IMPALED", EnemyColors.Skull2, 2);
	public static readonly Death Gored = new(GameVersion.V3_0, "GORED", EnemyColors.Skull3, 3);
	public static readonly Death Infested = new(GameVersion.V3_0, "INFESTED", EnemyColors.Spiderling, 4);
	public static readonly Death Opened = new(GameVersion.V3_0, "OPENED", EnemyColors.Skull4, 5);
	public static readonly Death Purged = new(GameVersion.V3_0, "PURGED", EnemyColors.Squid1, 6);
	public static readonly Death Desecrated = new(GameVersion.V3_0, "DESECRATED", EnemyColors.Squid2, 7);
	public static readonly Death Sacrificed = new(GameVersion.V3_0, "SACRIFICED", EnemyColors.Squid3, 8);
	public static readonly Death Eviscerated = new(GameVersion.V3_0, "EVISCERATED", EnemyColors.Centipede, 9);
	public static readonly Death Annihilated = new(GameVersion.V3_0, "ANNIHILATED", EnemyColors.Gigapede, 10);
	public static readonly Death Intoxicated = new(GameVersion.V3_0, "INTOXICATED", EnemyColors.SpiderEgg1, 11);
	public static readonly Death Envenomated = new(GameVersion.V3_0, "ENVENOMATED", EnemyColors.SpiderEgg2, 12);
	public static readonly Death Incarnated = new(GameVersion.V3_0, "INCARNATED", EnemyColors.Leviathan, 13);
	public static readonly Death Discarnated = new(GameVersion.V3_0, "DISCARNATED", EnemyColors.TheOrb, 14);
	public static readonly Death Barbed = new(GameVersion.V3_0, "BARBED", EnemyColors.Thorn, 15);
	public static readonly Death Unknown = new(GameVersion.V3_0, "UNKNOWN", EnemyColors.Unknown, 255);

	internal static readonly IReadOnlyList<Death> All = new List<Death>
	{
		Fallen,
		Swarmed,
		Impaled,
		Gored,
		Infested,
		Opened,
		Purged,
		Desecrated,
		Sacrificed,
		Eviscerated,
		Annihilated,
		Intoxicated,
		Envenomated,
		Incarnated,
		Discarnated,
		Barbed,
		Unknown,
	};
}
