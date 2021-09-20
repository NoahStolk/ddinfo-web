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
	}
}
