namespace DevilDaggersInfo.Core.Wiki.Test;

[TestClass]
public class UpgradeTests
{
	[TestMethod]
	public void TestGetUpgrades()
	{
		Assert.AreEqual(3, Upgrades.GetUpgrades(GameVersion.V1_0).Count);
		Assert.AreEqual(4, Upgrades.GetUpgrades(GameVersion.V2_0).Count);
		Assert.AreEqual(4, Upgrades.GetUpgrades(GameVersion.V3_0).Count);
		Assert.AreEqual(4, Upgrades.GetUpgrades(GameVersion.V3_1).Count);
	}

	[TestMethod]
	public void TestLevels()
	{
		Assert.AreEqual(1, UpgradesV1_0.Level1.Level);
		Assert.AreEqual(2, UpgradesV1_0.Level2.Level);
		Assert.AreEqual(3, UpgradesV1_0.Level3.Level);

		Assert.AreEqual(1, UpgradesV2_0.Level1.Level);
		Assert.AreEqual(2, UpgradesV2_0.Level2.Level);
		Assert.AreEqual(3, UpgradesV2_0.Level3.Level);
		Assert.AreEqual(4, UpgradesV2_0.Level4.Level);

		Assert.AreEqual(1, UpgradesV3_0.Level1.Level);
		Assert.AreEqual(2, UpgradesV3_0.Level2.Level);
		Assert.AreEqual(3, UpgradesV3_0.Level3.Level);
		Assert.AreEqual(4, UpgradesV3_0.Level4.Level);

		Assert.AreEqual(1, UpgradesV3_1.Level1.Level);
		Assert.AreEqual(2, UpgradesV3_1.Level2.Level);
		Assert.AreEqual(3, UpgradesV3_1.Level3.Level);
		Assert.AreEqual(4, UpgradesV3_1.Level4.Level);
	}

	[TestMethod]
	public void TestDefaultDamage()
	{
		Assert.AreEqual(new(10, 20f), UpgradesV1_0.Level1.DefaultDamage);
		Assert.AreEqual(new(10, 20f), UpgradesV2_0.Level1.DefaultDamage);
		Assert.AreEqual(new(10, 20f), UpgradesV3_0.Level1.DefaultDamage);
		Assert.AreEqual(new(10, 20f), UpgradesV3_1.Level1.DefaultDamage);

		Assert.AreEqual(new(20, 40f), UpgradesV1_0.Level2.DefaultDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV2_0.Level2.DefaultDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV3_0.Level2.DefaultDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV3_1.Level2.DefaultDamage);

		Assert.AreEqual(new(40, 80f), UpgradesV1_0.Level3.DefaultDamage);
		Assert.AreEqual(new(40, 80f), UpgradesV2_0.Level3.DefaultDamage);
		Assert.AreEqual(new(40, 80f), UpgradesV3_0.Level3.DefaultDamage);
		Assert.AreEqual(new(40, 80f), UpgradesV3_1.Level3.DefaultDamage);

		Assert.AreEqual(new(60, 106.666f), UpgradesV2_0.Level4.DefaultDamage);
		Assert.AreEqual(new(60, 106.666f), UpgradesV3_0.Level4.DefaultDamage);
		Assert.AreEqual(new(60, 106.666f), UpgradesV3_1.Level4.DefaultDamage);
	}

	[TestMethod]
	public void TestHomingDamage()
	{
		Assert.AreEqual(new(null, null), UpgradesV1_0.Level1.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV2_0.Level1.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV3_0.Level1.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV3_1.Level1.HomingDamage);

		Assert.AreEqual(new(null, null), UpgradesV1_0.Level2.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV2_0.Level2.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV3_0.Level2.HomingDamage);
		Assert.AreEqual(new(null, null), UpgradesV3_1.Level2.HomingDamage);

		Assert.AreEqual(new(40, 40f), UpgradesV1_0.Level3.HomingDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV2_0.Level3.HomingDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV3_0.Level3.HomingDamage);
		Assert.AreEqual(new(20, 40f), UpgradesV3_1.Level3.HomingDamage);

		Assert.AreEqual(new(30, 40f), UpgradesV2_0.Level4.HomingDamage);
		Assert.AreEqual(new(30, 40f), UpgradesV3_0.Level4.HomingDamage);
		Assert.AreEqual(new(30, 40f), UpgradesV3_1.Level4.HomingDamage);
	}

	[TestMethod]
	public void TestUpgradeUnlock()
	{
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 0), UpgradesV1_0.Level1.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 0), UpgradesV2_0.Level1.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 0), UpgradesV3_0.Level1.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 0), UpgradesV3_1.Level1.UpgradeUnlock);

		Assert.AreEqual(new(UpgradeUnlockType.Gems, 10), UpgradesV1_0.Level2.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 10), UpgradesV2_0.Level2.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 10), UpgradesV3_0.Level2.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 10), UpgradesV3_1.Level2.UpgradeUnlock);

		Assert.AreEqual(new(UpgradeUnlockType.Gems, 70), UpgradesV1_0.Level3.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 70), UpgradesV2_0.Level3.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 70), UpgradesV3_0.Level3.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 70), UpgradesV3_1.Level3.UpgradeUnlock);

		Assert.AreEqual(new(UpgradeUnlockType.Homing, 150), UpgradesV2_0.Level4.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Homing, 150), UpgradesV3_0.Level4.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Homing, 150), UpgradesV3_1.Level4.UpgradeUnlock);
	}
}
