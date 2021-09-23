namespace DevilDaggersInfo.Test.Core.Spawnset;

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
		byte[] originalBytes = File.ReadAllBytes(Path.Combine("Data", fileName));
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(originalBytes);
		byte[] bytes = spawnset.ToBytes();

		TestUtils.AssertArrayContentsEqual(originalBytes, bytes);
	}
}
