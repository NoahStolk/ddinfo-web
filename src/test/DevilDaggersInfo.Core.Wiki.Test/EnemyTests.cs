namespace DevilDaggersInfo.Core.Wiki.Test;

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
			Assert.IsNotNull(originalTransmutedSkull1.HomingDamage.Level3HomingDaggers);
			Assert.AreEqual(0.25f, originalTransmutedSkull1.HomingDamage.Level3HomingDaggers.Value, 0.00001f);
			Assert.AreEqual(10, originalTransmutedSkull1.HomingDamage.Level4HomingDaggers);
		}

		Enemy? fixedTransmutedSkull1 = Enemies.GetEnemyByName(GameVersion.V3_2, "Transmuted Skull I");
		Assert.IsNotNull(fixedTransmutedSkull1);
		Assert.AreEqual(1, fixedTransmutedSkull1.HomingDamage.Level3HomingDaggers);
		Assert.AreEqual(1, fixedTransmutedSkull1.HomingDamage.Level4HomingDaggers);
	}
}
