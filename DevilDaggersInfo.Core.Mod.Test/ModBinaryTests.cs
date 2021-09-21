namespace DevilDaggersInfo.Core.Mod.Test;

[TestClass]
public class ModBinaryTests
{
	[DataTestMethod]
	[DataRow("dd-model")]
	[DataRow("dd-shader")]
	[DataRow("dd-texture")]
	public void CompareBinaryOutput(string fileName)
	{
		byte[] originalBytes = File.ReadAllBytes(Path.Combine("Data", fileName));
		ModBinary modBinary = GetModBinary(fileName);
		byte[] bytes = modBinary.ToBytes();

		Assert.AreEqual(originalBytes.Length, bytes.Length);
		for (int i = 0; i < originalBytes.Length; i++)
			Assert.AreEqual(originalBytes[i], bytes[i]);
	}

	private static ModBinary GetModBinary(string fileName)
	{
		string filePath = Path.Combine("Data", fileName);
		byte[] fileContents = File.ReadAllBytes(filePath);
		return new(fileName, fileContents, true);
	}
}
