namespace DevilDaggersInfo.Core.Wiki;

public static class UpgradesV1_0
{
	public static readonly Upgrade Level1 = new(GameVersion.V1_0, "Level 1", UpgradeColors.Level1, 1, new(10, 20f), null, new(UpgradeUnlockType.Gems, 0));
	public static readonly Upgrade Level2 = new(GameVersion.V1_0, "Level 2", UpgradeColors.Level2, 2, new(20, 40f), null, new(UpgradeUnlockType.Gems, 10));
	public static readonly Upgrade Level3 = new(GameVersion.V1_0, "Level 3", UpgradeColors.Level3, 3, new(40, 80f), new(40, 40), new(UpgradeUnlockType.Gems, 70));

	internal static readonly IReadOnlyList<Upgrade> All = new List<Upgrade>
	{
		Level1,
		Level2,
		Level3,
	};
}
