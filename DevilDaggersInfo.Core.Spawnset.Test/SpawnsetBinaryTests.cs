namespace DevilDaggersInfo.Core.Spawnset.Test;

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
		Spawnset spawnset = Spawnset.Parse(originalBytes);
		byte[] bytes = spawnset.ToBytes();

		Assert.AreEqual(originalBytes.Length, bytes.Length);
		for (int i = 0; i < originalBytes.Length; i++)
			Assert.AreEqual(originalBytes[i], bytes[i]);
	}
}
