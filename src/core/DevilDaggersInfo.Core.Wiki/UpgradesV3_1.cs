namespace DevilDaggersInfo.Core.Wiki;

public static class UpgradesV3_1
{
	public static readonly Upgrade Level1 = new(GameVersion.V3_1, "Level 1", UpgradeColors.Level1, 1, new(10, 20f), null, new(UpgradeUnlockType.Gems, 0));
	public static readonly Upgrade Level2 = new(GameVersion.V3_1, "Level 2", UpgradeColors.Level2, 2, new(20, 40f), null, new(UpgradeUnlockType.Gems, 10));
	public static readonly Upgrade Level3 = new(GameVersion.V3_1, "Level 3", UpgradeColors.Level3, 3, new(40, 80f), new(20, 40), new(UpgradeUnlockType.Gems, 70));
	public static readonly Upgrade Level4 = new(GameVersion.V3_1, "Level 4", UpgradeColors.Level4, 4, new(60, 106.666f), new(30, 40), new(UpgradeUnlockType.Homing, 150));

	internal static readonly IReadOnlyList<Upgrade> All = new List<Upgrade>
	{
		Level1,
		Level2,
		Level3,
		Level4,
	};
}
