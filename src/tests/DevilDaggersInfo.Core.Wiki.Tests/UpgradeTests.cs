using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Core.Wiki.Tests;

[TestClass]
public class UpgradeTests
{
	[TestMethod]
	public void TestLevels()
	{
		Assert.AreEqual(1, Upgrades.Level1.Level);
		Assert.AreEqual(2, Upgrades.Level2.Level);
		Assert.AreEqual(3, Upgrades.Level3.Level);
		Assert.AreEqual(4, Upgrades.Level4.Level);
	}

	[TestMethod]
	public void TestDefaultDamage()
	{
		Assert.AreEqual(new(10, 20f), Upgrades.Level1.DefaultDamage);
		Assert.AreEqual(new(20, 40f), Upgrades.Level2.DefaultDamage);
		Assert.AreEqual(new(40, 80f), Upgrades.Level3.DefaultDamage);
		Assert.AreEqual(new(60, 106.666f), Upgrades.Level4.DefaultDamage);
	}

	[TestMethod]
	public void TestHomingDamage()
	{
		Assert.AreEqual(null, Upgrades.Level1.HomingDamage);
		Assert.AreEqual(null, Upgrades.Level2.HomingDamage);
		Assert.AreEqual(new(20, 40f), Upgrades.Level3.HomingDamage);
		Assert.AreEqual(new(30, 40f), Upgrades.Level4.HomingDamage);
	}

	[TestMethod]
	public void TestUpgradeUnlock()
	{
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 0), Upgrades.Level1.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 10), Upgrades.Level2.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Gems, 70), Upgrades.Level3.UpgradeUnlock);
		Assert.AreEqual(new(UpgradeUnlockType.Homing, 150), Upgrades.Level4.UpgradeUnlock);
	}
}
