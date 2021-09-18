namespace DevilDaggersInfo.Core.Spawnset.Test;

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
		Spawnset spawnset = Spawnset.Parse(File.ReadAllBytes(Path.Combine("Data", fileName)));
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
		Spawnset spawnset = Spawnset.Parse(File.ReadAllBytes(Path.Combine("Data", fileName)));
		Assert.AreEqual(hasSpawns, spawnset.HasSpawns());
	}
}
