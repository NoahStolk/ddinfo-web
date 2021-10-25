namespace DevilDaggersInfo.Test.Core.Spawnset;

[TestClass]
public class SpawnsViewTests
{
	[TestMethod]
	public void TestSpawnsView_V0()
	{
		SpawnsView spawnsView = Parse("V0", GameVersion.V1_0, 3, 57, 18);
		Assert.AreEqual(new SpawnView(EnemyType.Squid1, 3, 2, 2), spawnsView.PreLoop[0]);
		Assert.AreEqual(new SpawnView(EnemyType.Squid1, 14, 2, 4), spawnsView.PreLoop[1]);
	}

	[AssertionMethod]
	private static SpawnsView Parse(string fileName, GameVersion gameVersion, int waveCount, int expectedPreLoopSpawnCount, int expectedWaveSpawnCount)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine("Data", fileName)));
		SpawnsView spawnsView = new(spawnset, gameVersion, waveCount);
		Assert.AreEqual(waveCount, spawnsView.Waves.Length);
		Assert.AreEqual(expectedPreLoopSpawnCount, spawnsView.PreLoop.Count);
		Assert.IsTrue(spawnsView.Waves.All(l => l.Count == expectedWaveSpawnCount));
		return spawnsView;
	}
}
