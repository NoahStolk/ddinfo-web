namespace DevilDaggersInfo.Core.Wiki.Test;

[TestClass]
public class DeathTests
{
	[TestMethod]
	public void TestFallen()
	{
		Assert.AreEqual(DeathsV1_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 0));
		Assert.AreEqual(DeathsV2_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 0));
		Assert.AreEqual(DeathsV3_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 0));
		Assert.AreEqual(DeathsV3_1.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 0));
	}

	[TestMethod]
	public void TestSwarmed()
	{
		Assert.AreEqual(DeathsV1_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 1));
		Assert.AreEqual(DeathsV2_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 1));
		Assert.AreEqual(DeathsV3_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 1));
		Assert.AreEqual(DeathsV3_1.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 1));
		Assert.AreEqual(DeathsV1_0.Swarmed, EnemiesV1_0.Skull1.Death);
		Assert.AreEqual(DeathsV2_0.Swarmed, EnemiesV2_0.Skull1.Death);
		Assert.AreEqual(DeathsV3_0.Swarmed, EnemiesV3_0.Skull1.Death);
		Assert.AreEqual(DeathsV3_1.Swarmed, EnemiesV3_1.Skull1.Death);
	}
}
