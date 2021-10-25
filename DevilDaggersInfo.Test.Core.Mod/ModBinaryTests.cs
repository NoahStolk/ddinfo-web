namespace DevilDaggersInfo.Test.Core.Mod;

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
		ModBinary modBinary = new(fileName, originalBytes, true);
		byte[] bytes = modBinary.ToBytes();

		TestUtils.AssertArrayContentsEqual(originalBytes, bytes);
	}
}
