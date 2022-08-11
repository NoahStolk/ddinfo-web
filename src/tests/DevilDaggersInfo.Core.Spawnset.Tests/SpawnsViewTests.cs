using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Core.Spawnset.Tests;

[TestClass]
public class SpawnsViewTests
{
	[TestMethod]
	public void TestSpawnsView_V0()
	{
		SpawnsView spawnsView = Parse("V0", GameVersion.V1_0, 3, 57, 18);

		AreEqual(new SpawnView(EnemyType.Squid1, 3, 2, new GemState(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new SpawnView(EnemyType.Squid1, 14, 2, new GemState(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new SpawnView(EnemyType.Squid2, 300, 3, new GemState(HandLevel.Level3, 111, 181)), spawnsView.Waves[0][0]);
		AreEqual(new SpawnView(EnemyType.Squid2, 300, 3, new GemState(HandLevel.Level3, 114, 184)), spawnsView.Waves[0][1]);
	}

	[TestMethod]
	public void TestSpawnsView_V2()
	{
		SpawnsView spawnsView = Parse("V2", GameVersion.V2_0, 3, 71, 7);
		AreEqual(new SpawnView(EnemyType.Squid1, 3, 2, new GemState(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new SpawnView(EnemyType.Squid1, 14, 2, new GemState(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new SpawnView(EnemyType.Squid3, 397, 3, new GemState(HandLevel.Level4, 132, 352)), spawnsView.Waves[0][0]);
		AreEqual(new SpawnView(EnemyType.Squid2, 403, 3, new GemState(HandLevel.Level4, 135, 355)), spawnsView.Waves[0][1]);

		AreEqual(new SpawnView(EnemyType.Gigapede, 415, 50, new GemState(HandLevel.Level4, 187, 407)), spawnsView.Waves[0][3]);
		AreEqual(new SpawnView(EnemyType.Gigapede, 468.5667, 50, new GemState(HandLevel.Level4, 272, 492)), spawnsView.Waves[1][3]);
		AreEqual(new SpawnView(EnemyType.Gigapede, 516.5667, 50, new GemState(HandLevel.Level4, 357, 577)), spawnsView.Waves[2][3]);
	}

	[TestMethod]
	public void TestSpawnsView_V2_In_V3()
	{
		SpawnsView spawnsView = Parse("V2", GameVersion.V3_0, 3, 71, 7);
		AreEqual(new SpawnView(EnemyType.Squid1, 3, 2, new GemState(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new SpawnView(EnemyType.Squid1, 14, 2, new GemState(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new SpawnView(EnemyType.Squid3, 397, 3, new GemState(HandLevel.Level4, 132, 352)), spawnsView.Waves[0][0]);
		AreEqual(new SpawnView(EnemyType.Squid2, 403, 3, new GemState(HandLevel.Level4, 135, 355)), spawnsView.Waves[0][1]);

		AreEqual(new SpawnView(EnemyType.Gigapede, 415, 50, new GemState(HandLevel.Level4, 187, 407)), spawnsView.Waves[0][3]);
		AreEqual(new SpawnView(EnemyType.Gigapede, 468.5667, 50, new GemState(HandLevel.Level4, 272, 492)), spawnsView.Waves[1][3]);
		AreEqual(new SpawnView(EnemyType.Ghostpede, 516.5667, 10, new GemState(HandLevel.Level4, 317, 537)), spawnsView.Waves[2][3]);
	}

	[AssertionMethod]
	private static SpawnsView Parse(string fileName, GameVersion gameVersion, int waveCount, int expectedPreLoopSpawnCount, int expectedWaveSpawnCount)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName)));
		SpawnsView spawnsView = new(spawnset, gameVersion, waveCount);
		Assert.AreEqual(waveCount, spawnsView.Waves.Length);
		Assert.AreEqual(expectedPreLoopSpawnCount, spawnsView.PreLoop.Count);
		Assert.IsTrue(spawnsView.Waves.All(l => l.Count == expectedWaveSpawnCount));
		return spawnsView;
	}

	[AssertionMethod]
	private static void AreEqual(SpawnView a, SpawnView b)
	{
		Assert.AreEqual(a.EnemyType, b.EnemyType);
		Assert.AreEqual(a.Seconds, b.Seconds, 0.0001);
		Assert.AreEqual(a.NoFarmGems, b.NoFarmGems);
		Assert.AreEqual(a.GemState, b.GemState);
	}

	[DataTestMethod]
	[DataRow("Empty", false, false)]
	[DataRow("EmptySpawn", false, false)]
	[DataRow("NoEndLoop", true, false)]
	[DataRow("LoopOnly", false, true)]
	[DataRow("Scanner", true, true)]
	[DataRow("V3", true, true)]
	[DataRow("TimeAttack", true, false)]
	[DataRow("RacePede", true, false)]
	[DataRow("Race", false, false)]
	public void TestHasSpawns(string fileName, bool expectedHasPreLoopSpawns, bool expectedHasLoopSpawns)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName)));
		SpawnsView spawnsView = new(spawnset, GameVersion.V3_2);
		Assert.AreEqual(expectedHasPreLoopSpawns, spawnsView.HasPreLoopSpawns);
		Assert.AreEqual(expectedHasLoopSpawns, spawnsView.HasLoopSpawns);
	}
}
