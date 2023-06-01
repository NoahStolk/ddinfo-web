using DevilDaggersInfo.Core.Wiki;

namespace DevilDaggersInfo.Core.Spawnset.Test;

[TestClass]
public class SpawnsViewTests
{
	[TestMethod]
	public void TestSpawnsView_V0()
	{
		SpawnsView spawnsView = Parse("V0", GameVersion.V1_0, 3, 57, 18);

		AreEqual(new(EnemyType.Squid1, 3, 2, new(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new(EnemyType.Squid1, 14, 2, new(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new(EnemyType.Squid2, 300, 3, new(HandLevel.Level3, 111, 181)), spawnsView.Waves[0][0]);
		AreEqual(new(EnemyType.Squid2, 300, 3, new(HandLevel.Level3, 114, 184)), spawnsView.Waves[0][1]);
	}

	[TestMethod]
	public void TestSpawnsView_V2()
	{
		SpawnsView spawnsView = Parse("V2", GameVersion.V2_0, 3, 71, 7);
		AreEqual(new(EnemyType.Squid1, 3, 2, new(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new(EnemyType.Squid1, 14, 2, new(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new(EnemyType.Squid3, 397, 3, new(HandLevel.Level4, 132, 352)), spawnsView.Waves[0][0]);
		AreEqual(new(EnemyType.Squid2, 403, 3, new(HandLevel.Level4, 135, 355)), spawnsView.Waves[0][1]);

		AreEqual(new(EnemyType.Gigapede, 415, 50, new(HandLevel.Level4, 187, 407)), spawnsView.Waves[0][3]);
		AreEqual(new(EnemyType.Gigapede, 468.5667, 50, new(HandLevel.Level4, 272, 492)), spawnsView.Waves[1][3]);
		AreEqual(new(EnemyType.Gigapede, 516.5667, 50, new(HandLevel.Level4, 357, 577)), spawnsView.Waves[2][3]);
	}

	[TestMethod]
	public void TestSpawnsView_V2_In_V3()
	{
		SpawnsView spawnsView = Parse("V2", GameVersion.V3_0, 3, 71, 7);
		AreEqual(new(EnemyType.Squid1, 3, 2, new(HandLevel.Level1, 2, 2)), spawnsView.PreLoop[0]);
		AreEqual(new(EnemyType.Squid1, 14, 2, new(HandLevel.Level1, 4, 4)), spawnsView.PreLoop[1]);

		AreEqual(new(EnemyType.Squid3, 397, 3, new(HandLevel.Level4, 132, 352)), spawnsView.Waves[0][0]);
		AreEqual(new(EnemyType.Squid2, 403, 3, new(HandLevel.Level4, 135, 355)), spawnsView.Waves[0][1]);

		AreEqual(new(EnemyType.Gigapede, 415, 50, new(HandLevel.Level4, 187, 407)), spawnsView.Waves[0][3]);
		AreEqual(new(EnemyType.Gigapede, 468.5667, 50, new(HandLevel.Level4, 272, 492)), spawnsView.Waves[1][3]);
		AreEqual(new(EnemyType.Ghostpede, 516.5667, 10, new(HandLevel.Level4, 317, 537)), spawnsView.Waves[2][3]);
	}

	[TestMethod]
	public void TestSpawns_PracticeSettings()
	{
		const string fileName = "V3";
		const GameVersion gameVersion = GameVersion.V3_0;
		const int waveCount = 1;
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine("Resources", fileName)));
		SpawnsView spawnsView = new(spawnset, gameVersion, waveCount);
		Assert.AreEqual(3, spawnsView.PreLoop[0].Seconds);
		Assert.AreEqual(new(HandLevel.Level1, 2, 2), spawnsView.PreLoop[0].GemState);

		spawnset = spawnset with
		{
			TimerStart = 10,
			HandLevel = HandLevel.Level2,
			AdditionalGems = 5,
		};
		spawnsView = new(spawnset, gameVersion, waveCount);

		// Settings should not be effective for spawn version 4.
		Assert.AreEqual(3, spawnsView.PreLoop[0].Seconds);
		Assert.AreEqual(new(HandLevel.Level1, 2, 2), spawnsView.PreLoop[0].GemState);

		spawnset = spawnset with { SpawnVersion = 5 };
		spawnsView = new(spawnset, gameVersion, waveCount);

		// Only hand and gem settings should be effective for spawn version 5.
		Assert.AreEqual(3, spawnsView.PreLoop[0].Seconds);
		Assert.AreEqual(new(HandLevel.Level2, 17, 2), spawnsView.PreLoop[0].GemState);

		spawnset = spawnset with { SpawnVersion = 6 };
		spawnsView = new(spawnset, gameVersion, waveCount);

		// All settings should be effective for spawn version 6.
		Assert.AreEqual(13, spawnsView.PreLoop[0].Seconds);
		Assert.AreEqual(new(HandLevel.Level2, 17, 2), spawnsView.PreLoop[0].GemState);
	}

	[AssertionMethod]
	private static SpawnsView Parse(string fileName, GameVersion gameVersion, int waveCount, int expectedPreLoopSpawnCount, int expectedWaveSpawnCount)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine("Resources", fileName)));
		SpawnsView spawnsView = new(spawnset, gameVersion, waveCount);
		Assert.AreEqual(waveCount, spawnsView.Waves.Length);
		Assert.AreEqual(expectedPreLoopSpawnCount, spawnsView.PreLoop.Count);
		Assert.IsTrue(Array.TrueForAll(spawnsView.Waves, l => l.Count == expectedWaveSpawnCount));
		return spawnsView;
	}

	[AssertionMethod]
	private static void AreEqual(SpawnView a, SpawnView b)
	{
		Assert.AreEqual(a.EnemyType, b.EnemyType);
		Assert.AreEqual(a.Seconds, b.Seconds, 0.0166); // Allow 1 frame difference.
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
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine("Resources", fileName)));
		SpawnsView spawnsView = new(spawnset, GameVersion.V3_2);
		Assert.AreEqual(expectedHasPreLoopSpawns, spawnsView.HasPreLoopSpawns);
		Assert.AreEqual(expectedHasLoopSpawns, spawnsView.HasLoopSpawns);
	}
}
