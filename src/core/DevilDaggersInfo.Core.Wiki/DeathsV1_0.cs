namespace DevilDaggersInfo.Core.Wiki;

public static class DeathsV1_0
{
	public static readonly Death Fallen = new(GameVersion.V1_0, "FALLEN", EnemyColors.Void, 0);
	public static readonly Death Swarmed = new(GameVersion.V1_0, "SWARMED", EnemyColors.Skull1, 1);
	public static readonly Death Impaled = new(GameVersion.V1_0, "IMPALED", EnemyColors.Skull2, 2);
	public static readonly Death Infested = new(GameVersion.V1_0, "INFESTED", EnemyColors.SpiderEgg1, 4);
	public static readonly Death Purged = new(GameVersion.V1_0, "PURGED", EnemyColors.Squid1, 6);
	public static readonly Death Sacrificed = new(GameVersion.V1_0, "SACRIFICED", EnemyColors.Squid2, 8);
	public static readonly Death Eviscerated = new(GameVersion.V1_0, "EVISCERATED", EnemyColors.Centipede, 9);
	public static readonly Death Annihilated = new(GameVersion.V1_0, "ANNIHILATED", EnemyColors.TransmutedSkull4, 10);
	public static readonly Death Stricken = new(GameVersion.V1_0, "STRICKEN", EnemyColors.Spiderling, 16);
	public static readonly Death Devastated = new(GameVersion.V1_0, "DEVASTATED", EnemyColors.Leviathan, 17);
	public static readonly Death Dismembered = new(GameVersion.V1_0, "DISMEMBERED", EnemyColors.Skull3, 18);
	public static readonly Death Unknown = new(GameVersion.V1_0, "UNKNOWN", EnemyColors.Unknown, 255);

	internal static readonly IReadOnlyList<Death> All = new List<Death>
	{
		Fallen,
		Swarmed,
		Impaled,
		Infested,
		Purged,
		Sacrificed,
		Eviscerated,
		Annihilated,
		Stricken,
		Devastated,
		Dismembered,
		Unknown,
	};
}
