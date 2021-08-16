namespace DevilDaggersInfo.Core.Wiki;

public static class Deaths
{
	public static readonly Death None = new(GameVersions.V2_0, "None", EnemyColors.Andras, 255);

	public static readonly Death Fallen = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "FALLEN", new(0xDD, 0xDD, 0xDD), 0);

	public static readonly Death Skull1_Swarmed = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "SWARMED", EnemyColors.Skull1, 1);

	public static readonly Death Skull2_Impaled = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "IMPALED", EnemyColors.Skull2, 2);

	public static readonly Death Skull3_Dismembered = new(GameVersions.V1_0, "DISMEMBERED", EnemyColors.Skull3, 18);
	public static readonly Death Skull3_Gored = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "GORED", EnemyColors.Skull3, 3);

	public static readonly Death Skull4_Opened = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "OPENED", EnemyColors.Skull4, 5);

	public static readonly Death TransmutedSkull4_Annihilated = new(GameVersions.V1_0, "ANNIHILATED", EnemyColors.TransmutedSkull4, 10);

	public static readonly Death Spiderling_Stricken = new(GameVersions.V1_0 | GameVersions.V2_0, "STRICKEN", EnemyColors.Spiderling, 16);
	public static readonly Death Spiderling_Infested = new(GameVersions.V3_0 | GameVersions.V3_1, "INFESTED", EnemyColors.Spiderling, 4);

	public static readonly Death SpiderEgg1_Infested = new(GameVersions.V1_0 | GameVersions.V2_0, "INFESTED", EnemyColors.SpiderEgg1, 4);
	public static readonly Death SpiderEgg1_Intoxicated = new(GameVersions.V3_0 | GameVersions.V3_1, "INTOXICATED", EnemyColors.SpiderEgg1, 11);

	public static readonly Death SpiderEgg2_Envenomated = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "ENVENOMATED", EnemyColors.SpiderEgg2, 12);

	public static readonly Death Spider1_Infested = new(GameVersions.V1_0 | GameVersions.V2_0, "INFESTED", EnemyColors.Spider1, 4);
	public static readonly Death Spider1_Intoxicated = new(GameVersions.V3_0 | GameVersions.V3_1, "INTOXICATED", EnemyColors.Spider1, 11);

	public static readonly Death Spider2_Envenomated = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "ENVENOMATED", EnemyColors.Spider2, 12);

	public static readonly Death Squid1_Purged = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "PURGED", EnemyColors.Squid1, 6);

	public static readonly Death Squid2_Sacrificed = new(GameVersions.V1_0, "SACRIFICED", EnemyColors.Squid2, 8);
	public static readonly Death Squid2_Desecrated = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "DESECRATED", EnemyColors.Squid2, 7);

	public static readonly Death Squid3_Sacrificed = new(GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "SACRIFICED", EnemyColors.Squid3, 8);

	public static readonly Death Centipede_Eviscerated = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "EVISCERATED", EnemyColors.Centipede, 9);

	public static readonly Death Gigapede_V1_0_V2_0_Eviscerated = new(GameVersions.V1_0, "EVISCERATED", EnemyColors.Gigapede_V1_0_V2_0, 9);
	public static readonly Death Gigapede_V1_0_V2_0_Annihilated = new(GameVersions.V2_0, "ANNIHILATED", EnemyColors.Gigapede_V1_0_V2_0, 10);
	public static readonly Death Gigapede_Annihilated = new(GameVersions.V3_0 | GameVersions.V3_1, "ANNIHILATED", EnemyColors.Gigapede, 10);

	public static readonly Death Leviathan_Devastated = new(GameVersions.V1_0 | GameVersions.V2_0, "DEVASTATED", EnemyColors.Leviathan, 17);
	public static readonly Death Leviathan_Incarnated = new(GameVersions.V3_0 | GameVersions.V3_1, "INCARNATED", EnemyColors.Leviathan, 13);

	public static readonly Death TheOrb_Discarnated = new(GameVersions.V3_0 | GameVersions.V3_1, "DISCARNATED", EnemyColors.TheOrb, 14);

	public static readonly Death Thorn_Barbed = new(GameVersions.V3_0, "BARBED", EnemyColors.Thorn, 15);
	public static readonly Death Thorn_Entangled = new(GameVersions.V3_1, "ENTANGLED", EnemyColors.Thorn, 15);

	public static readonly Death Ghostpede_Intoxicated = new(GameVersions.V3_0, "INTOXICATED", EnemyColors.Ghostpede, 11);
	public static readonly Death Ghostpede_Haunted = new(GameVersions.V3_1, "HAUNTED", EnemyColors.Ghostpede, 16);

	private static readonly IEnumerable<Death> _all = typeof(Deaths).GetFields().Where(f => f.FieldType == typeof(Death)).Select(f => (Death)f.GetValue(null)!);
	private static readonly IEnumerable<Death> _allV1_0 = _all.Where(ddo => (ddo.GameVersions & GameVersions.V1_0) != 0);
	private static readonly IEnumerable<Death> _allV2_0 = _all.Where(ddo => (ddo.GameVersions & GameVersions.V2_0) != 0);
	private static readonly IEnumerable<Death> _allV3_0 = _all.Where(ddo => (ddo.GameVersions & GameVersions.V3_0) != 0);
	private static readonly IEnumerable<Death> _allV3_1 = _all.Where(ddo => (ddo.GameVersions & GameVersions.V3_1) != 0);

	public static IEnumerable<Death> GetDeaths(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => _allV1_0,
		GameVersion.V2_0 => _allV2_0,
		GameVersion.V3_0 => _allV3_0,
		GameVersion.V3_1 => _allV3_1,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Death? GetDeathByType(GameVersion gameVersion, byte leaderboardDeathType)
		=> GetDeaths(gameVersion).FirstOrDefault(e => e.LeaderboardDeathType == leaderboardDeathType);
}
