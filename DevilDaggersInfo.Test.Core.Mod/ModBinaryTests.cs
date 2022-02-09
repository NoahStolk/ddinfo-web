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
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(fileName, originalBytes, BinaryReadComprehensiveness.All);
		byte[] bytes = modBinary.Compile();

		TestUtils.AssertArrayContentsEqual(originalBytes, bytes);
	}

	[TestMethod]
	public void ValidateTocSingleAsset()
	{
		const string fileName = "dd-maken-main";
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(fileName, originalBytes, BinaryReadComprehensiveness.TocOnly);

		ModBinaryChunk chunk = modBinary.Chunks[0];
		Assert.AreEqual("dagger6", chunk.Name);
		Assert.AreEqual(AssetType.Texture, chunk.AssetType);
		Assert.AreEqual(21855, chunk.Size);
	}
}
