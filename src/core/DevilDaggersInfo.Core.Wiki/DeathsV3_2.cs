namespace DevilDaggersInfo.Core.Wiki;

public static class DeathsV3_2
{
	public static readonly Death Fallen = new(GameVersion.V3_2, "FALLEN", EnemyColors.Void, 0);
	public static readonly Death Swarmed = new(GameVersion.V3_2, "SWARMED", EnemyColors.Skull1, 1);
	public static readonly Death Impaled = new(GameVersion.V3_2, "IMPALED", EnemyColors.Skull2, 2);
	public static readonly Death Gored = new(GameVersion.V3_2, "GORED", EnemyColors.Skull3, 3);
	public static readonly Death Infested = new(GameVersion.V3_2, "INFESTED", EnemyColors.Spiderling, 4);
	public static readonly Death Opened = new(GameVersion.V3_2, "OPENED", EnemyColors.Skull4, 5);
	public static readonly Death Purged = new(GameVersion.V3_2, "PURGED", EnemyColors.Squid1, 6);
	public static readonly Death Desecrated = new(GameVersion.V3_2, "DESECRATED", EnemyColors.Squid2, 7);
	public static readonly Death Sacrificed = new(GameVersion.V3_2, "SACRIFICED", EnemyColors.Squid3, 8);
	public static readonly Death Eviscerated = new(GameVersion.V3_2, "EVISCERATED", EnemyColors.Centipede, 9);
	public static readonly Death Annihilated = new(GameVersion.V3_2, "ANNIHILATED", EnemyColors.Gigapede, 10);
	public static readonly Death Intoxicated = new(GameVersion.V3_2, "INTOXICATED", EnemyColors.SpiderEgg1, 11);
	public static readonly Death Envenomated = new(GameVersion.V3_2, "ENVENOMATED", EnemyColors.SpiderEgg2, 12);
	public static readonly Death Incarnated = new(GameVersion.V3_2, "INCARNATED", EnemyColors.Leviathan, 13);
	public static readonly Death Discarnated = new(GameVersion.V3_2, "DISCARNATED", EnemyColors.TheOrb, 14);
	public static readonly Death Entangled = new(GameVersion.V3_2, "ENTANGLED", EnemyColors.Thorn, 15);
	public static readonly Death Haunted = new(GameVersion.V3_2, "HAUNTED", EnemyColors.Ghostpede, 16);
	public static readonly Death Unknown = new(GameVersion.V3_2, "UNKNOWN", EnemyColors.Unknown, 255);

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
