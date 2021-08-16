namespace DevilDaggersInfo.Core.Wiki;

public static class Upgrades
{
	// public static readonly Upgrade V1Level3 = new(GameVersions.V1, "Level 3", "00FFFF", 80, 40, 40, 40, "70 gems"); // TODO: Figure out Level 3 homing spray.
	public static readonly Upgrade Level1 = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Level 1", UpgradeColors.Level1, 1, new(10, 20), default, new(UpgradeUnlockType.Gems, 0));
	public static readonly Upgrade Level2 = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Level 2", UpgradeColors.Level2, 2, new(20, 40), default, new(UpgradeUnlockType.Gems, 10));
	public static readonly Upgrade Level3 = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Level 3", UpgradeColors.Level3, 3, new(40, 80), new(20, 40), new(UpgradeUnlockType.Gems, 70));
	public static readonly Upgrade Level4 = new(GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Level 4", UpgradeColors.Level4, 4, new(60, 106f + 2f / 3f), new(30, 40), new(UpgradeUnlockType.Homing, 150));

	private static readonly IEnumerable<Upgrade> _all = typeof(Upgrades).GetFields().Where(f => f.FieldType == typeof(Upgrade)).Select(f => (Upgrade)f.GetValue(null)!);
	private static readonly IEnumerable<Upgrade> _allV1_0 = _all.Where(ddo => ddo != Level4);

	public static IEnumerable<Upgrade> GetUpgrades(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => _allV1_0,
		GameVersion.V2_0 or GameVersion.V3_0 or GameVersion.V3_1 => _all,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};
}
