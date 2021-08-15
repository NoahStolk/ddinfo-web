namespace DevilDaggersInfo.Core.Wiki;

public static class Upgrades
{
	// public static readonly Upgrade V1Level3 = new(GameVersions.V1, "Level 3", "00FFFF", 80, 40, 40, 40, "70 gems"); // TODO: Figure out Level 3 homing spray.
	public static readonly Upgrade Level1 = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Level 1", new(0xBB, 0x55, 0x00), 1, new(10, 20), default, new(UpgradeUnlockType.Gems, 0));
	public static readonly Upgrade Level2 = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Level 2", new(0xFF, 0xAA, 0x00), 2, new(20, 40), default, new(UpgradeUnlockType.Gems, 10));
	public static readonly Upgrade Level3 = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Level 3", new(0x00, 0xFF, 0xFF), 3, new(40, 80), new(20, 40), new(UpgradeUnlockType.Gems, 70));
	public static readonly Upgrade Level4 = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Level 4", new(0xFF, 0x00, 0x99), 4, new(60, 106f + 2f / 3f), new(30, 40), new(UpgradeUnlockType.Homing, 150));
}
