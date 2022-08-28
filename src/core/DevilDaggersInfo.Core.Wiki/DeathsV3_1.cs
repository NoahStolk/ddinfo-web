namespace DevilDaggersInfo.Core.Wiki;

public static class DeathsV3_1
{
	public static readonly Death Fallen = new(GameVersion.V3_1, "FALLEN", EnemyColors.Void, 0);
	public static readonly Death Swarmed = new(GameVersion.V3_1, "SWARMED", EnemyColors.Skull1, 1);
	public static readonly Death Impaled = new(GameVersion.V3_1, "IMPALED", EnemyColors.Skull2, 2);
	public static readonly Death Gored = new(GameVersion.V3_1, "GORED", EnemyColors.Skull3, 3);
	public static readonly Death Infested = new(GameVersion.V3_1, "INFESTED", EnemyColors.Spiderling, 4);
	public static readonly Death Opened = new(GameVersion.V3_1, "OPENED", EnemyColors.Skull4, 5);
	public static readonly Death Purged = new(GameVersion.V3_1, "PURGED", EnemyColors.Squid1, 6);
	public static readonly Death Desecrated = new(GameVersion.V3_1, "DESECRATED", EnemyColors.Squid2, 7);
	public static readonly Death Sacrificed = new(GameVersion.V3_1, "SACRIFICED", EnemyColors.Squid3, 8);
	public static readonly Death Eviscerated = new(GameVersion.V3_1, "EVISCERATED", EnemyColors.Centipede, 9);
	public static readonly Death Annihilated = new(GameVersion.V3_1, "ANNIHILATED", EnemyColors.Gigapede, 10);
	public static readonly Death Intoxicated = new(GameVersion.V3_1, "INTOXICATED", EnemyColors.SpiderEgg1, 11);
	public static readonly Death Envenomated = new(GameVersion.V3_1, "ENVENOMATED", EnemyColors.SpiderEgg2, 12);
	public static readonly Death Incarnated = new(GameVersion.V3_1, "INCARNATED", EnemyColors.Leviathan, 13);
	public static readonly Death Discarnated = new(GameVersion.V3_1, "DISCARNATED", EnemyColors.TheOrb, 14);
	public static readonly Death Entangled = new(GameVersion.V3_1, "ENTANGLED", EnemyColors.Thorn, 15);
	public static readonly Death Haunted = new(GameVersion.V3_1, "HAUNTED", EnemyColors.Ghostpede, 16);
	public static readonly Death Unknown = new(GameVersion.V3_1, "UNKNOWN", EnemyColors.Unknown, 255);

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
		Entangled,
		Haunted,
		Unknown,
	};
}
