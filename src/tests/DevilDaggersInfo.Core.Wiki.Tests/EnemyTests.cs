using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Core.Wiki.Tests;

[TestClass]
public class EnemyTests
{
	[TestMethod]
	public void TestGetEnemies()
	{
		Assert.AreEqual(14, Enemies.GetEnemies(GameVersion.V1_0).Count);
		Assert.AreEqual(20, Enemies.GetEnemies(GameVersion.V2_0).Count);
		Assert.AreEqual(22, Enemies.GetEnemies(GameVersion.V3_0).Count);
		Assert.AreEqual(22, Enemies.GetEnemies(GameVersion.V3_1).Count);
		Assert.AreEqual(22, Enemies.GetEnemies(GameVersion.V3_2).Count);
	}

	[TestMethod]
	public void TestTransmutedSkull1HomingDamage()
	{
		foreach (GameVersion gameVersion in Enum.GetValues<GameVersion>())
		{
			if (gameVersion is GameVersion.V1_0 or GameVersion.V3_2)
				continue;

			Enemy? originalTransmutedSkull1 = Enemies.GetEnemyByName(gameVersion, "Transmuted Skull I");
			Assert.IsNotNull(originalTransmutedSkull1);
			Assert.AreEqual(1, originalTransmutedSkull1.HomingDamage.FromLevel3);
			Assert.AreEqual(1, originalTransmutedSkull1.HomingDamage.FromLevel4);
		}

		Enemy? fixedTransmutedSkull1 = Enemies.GetEnemyByName(GameVersion.V3_2, "Transmuted Skull I");
		Assert.IsNotNull(fixedTransmutedSkull1);
		Assert.AreEqual(10, fixedTransmutedSkull1.HomingDamage.FromLevel3);
		Assert.AreEqual(10, fixedTransmutedSkull1.HomingDamage.FromLevel4);
	}
}
