namespace DevilDaggersInfo.Core.Wiki;

public static class DeathsV2_0
{
	public static readonly Death Fallen = new(GameVersion.V2_0, "FALLEN", EnemyColors.Void, 0);
	public static readonly Death Swarmed = new(GameVersion.V2_0, "SWARMED", EnemyColors.Skull1, 1);
	public static readonly Death Impaled = new(GameVersion.V2_0, "IMPALED", EnemyColors.Skull2, 2);
	public static readonly Death Gored = new(GameVersion.V2_0, "GORED", EnemyColors.Skull3, 3);
	public static readonly Death Infested = new(GameVersion.V2_0, "INFESTED", EnemyColors.SpiderEgg1, 4);
	public static readonly Death Opened = new(GameVersion.V2_0, "OPENED", EnemyColors.Skull4, 5);
	public static readonly Death Purged = new(GameVersion.V2_0, "PURGED", EnemyColors.Squid1, 6);
	public static readonly Death Desecrated = new(GameVersion.V2_0, "DESECRATED", EnemyColors.Squid2, 7);
	public static readonly Death Sacrificed = new(GameVersion.V2_0, "SACRIFICED", EnemyColors.Squid3, 8);
	public static readonly Death Eviscerated = new(GameVersion.V2_0, "EVISCERATED", EnemyColors.Centipede, 9);
	public static readonly Death Annihilated = new(GameVersion.V2_0, "ANNIHILATED", EnemyColors.GigapedeRed, 10);
	public static readonly Death Envenomated = new(GameVersion.V2_0, "ENVENOMATED", EnemyColors.SpiderEgg2, 12);
	public static readonly Death Stricken = new(GameVersion.V2_0, "STRICKEN", EnemyColors.Spiderling, 16);
	public static readonly Death Devastated = new(GameVersion.V2_0, "DEVASTATED", EnemyColors.Leviathan, 17);
	public static readonly Death None = new(GameVersion.V2_0, "NONE", EnemyColors.Andras, 200);
	public static readonly Death Unknown = new(GameVersion.V2_0, "UNKNOWN", EnemyColors.Unknown, 255);

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
		Envenomated,
		Stricken,
		Devastated,
		None,
		Unknown,
	};
}
