using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Core.Spawnset.Tests;

[TestClass]
public class SpawnsetBinaryTests
{
	[DataTestMethod]
	[DataRow("V0")]
	[DataRow("V1")]
	[DataRow("V2")]
	[DataRow("V3")]
	[DataRow("V3_229")]
	[DataRow("V3_451")]
	[DataRow("Empty")]
	[DataRow("EmptySpawn")]
	[DataRow("NoEndLoop")]
	[DataRow("TimeAttack")]
	[DataRow("Scanner")]
	public void CompareBinaryOutput(string fileName)
	{
		byte[] originalBytes = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName));
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(originalBytes);
		byte[] bytes = spawnset.ToBytes();

		CollectionAssert.AreEqual(originalBytes, bytes);
	}

	[TestMethod]
	public void TestEffectivePlayerSettings()
	{
		byte[] originalBytes = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "V3"));
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(originalBytes) with
		{
			HandLevel = HandLevel.Level2,
			AdditionalGems = 40,
		};

		// The effective player setting should be default when using the default spawnset (or any spawnset with spawn version 4).
		EffectivePlayerSettings settings = spawnset.GetEffectivePlayerSettings();
		Assert.AreEqual(HandLevel.Level1, settings.HandLevel);
		Assert.AreEqual(0, settings.GemsOrHoming);
		Assert.AreEqual(HandLevel.Level1, settings.HandMesh);

		spawnset = spawnset with
		{
			SpawnVersion = 5, // Specified player settings should be effective from version 5.
		};
		settings = spawnset.GetEffectivePlayerSettings();
		Assert.AreEqual(HandLevel.Level2, settings.HandLevel);
		Assert.AreEqual(50, settings.GemsOrHoming);
		Assert.AreEqual(HandLevel.Level2, settings.HandMesh);
	}

	[TestMethod]
	public void TestEffectiveTimerStart()
	{
		byte[] originalBytes = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "V3"));
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(originalBytes) with
		{
			TimerStart = 10,
		};

		// The effective timer start should be default when using the default spawnset (or any spawnset with spawn version 5 or lower).
		float timerStart = spawnset.GetEffectiveTimerStart();
		Assert.AreEqual(0, timerStart);

		spawnset = spawnset with
		{
			SpawnVersion = 6, // Specified timer start should be effective from version 6.
		};
		timerStart = spawnset.GetEffectiveTimerStart();
		Assert.AreEqual(10, timerStart);
	}
}
