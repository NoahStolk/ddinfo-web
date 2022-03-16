namespace DevilDaggersInfo.Test.Core.Spawnset;

[TestClass]
public class SpawnsetUtilityTests
{
	[DataTestMethod]
	[DataRow("V0", true)]
	[DataRow("V1", true)]
	[DataRow("V2", true)]
	[DataRow("V3", true)]
	[DataRow("V3_229", true)]
	[DataRow("V3_451", true)]
	[DataRow("Empty", false)]
	[DataRow("EmptySpawn", false)]
	[DataRow("NoEndLoop", false)]
	[DataRow("TimeAttack", false)]
	[DataRow("Scanner", true)]
	public void TestHasEndLoop(string fileName, bool hasEndLoop)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName)));
		Assert.AreEqual(hasEndLoop, spawnset.HasEndLoop());
	}

	[DataTestMethod]
	[DataRow("V0", true)]
	[DataRow("V1", true)]
	[DataRow("V2", true)]
	[DataRow("V3", true)]
	[DataRow("V3_229", true)]
	[DataRow("V3_451", true)]
	[DataRow("Empty", false)]
	[DataRow("EmptySpawn", false)]
	[DataRow("NoEndLoop", true)]
	[DataRow("TimeAttack", true)]
	[DataRow("Scanner", true)]
	public void TestHasSpawns(string fileName, bool hasSpawns)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName)));
		Assert.AreEqual(hasSpawns, spawnset.HasSpawns());
	}

	[DataTestMethod]
	[DataRow(50f, 20f, 0.025f, 1200f)]
	[DataRow(30f, 20f, 1f, 10f)]
	[DataRow(30f, 5f, 1f, 25f)]
	[DataRow(30f, 0f, 1f, 30f)]
	[DataRow(30f, -1f, 1f, 30f)]
	[DataRow(26f, 15f, 0.025f, 440f)]
	[DataRow(50f, 20f, 0f, 0f)]
	[DataRow(50f, 20f, -1f, 0f)]
	[DataRow(30f, 40f, 1f, 0f)]
	[DataRow(6105.9f, 27f, 11.5f, 528.6f)]
	public void TestFinalShrinkSecond(float start, float end, float rate, float expectedFinalShrinkSecond)
	{
		Assert.AreEqual(expectedFinalShrinkSecond, SpawnsetBinary.GetFinalShrinkStateSecond(start, end, rate), 0.0001);
	}
}
