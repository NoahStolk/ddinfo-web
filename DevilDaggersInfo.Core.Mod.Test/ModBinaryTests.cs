namespace DevilDaggersInfo.Core.Mod.Test;

[TestClass]
public class ModBinaryTests
{
	[DataTestMethod]
	[DataRow("dd-model")]
	[DataRow("dd-model-shader-texture")]
	[DataRow("dd-shader")]
	[DataRow("dd-skull-1-2-same-texture-copied")]
	[DataRow("dd-texture")]
	public void CompareBinaryOutput(string fileName)
	{
		string filePath = Path.Combine("Data", fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		byte[] fileContents = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(fileName, fileContents, true);
		byte[] bytes = modBinary.ToBytes();

		Assert.AreEqual(originalBytes.Length, bytes.Length);
		for (int i = 0; i < originalBytes.Length; i++)
			Assert.AreEqual(originalBytes[i], bytes[i]);
	}
}
