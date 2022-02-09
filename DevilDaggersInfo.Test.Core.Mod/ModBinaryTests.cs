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
		const string fileName = "dd-maken";
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(fileName, originalBytes, BinaryReadComprehensiveness.TocOnly);

		Assert.AreEqual(1, modBinary.Chunks.Count);
		ModBinaryChunk chunk = modBinary.Chunks[0];
		Assert.AreEqual("dagger6", chunk.Name);
		Assert.AreEqual(AssetType.Texture, chunk.AssetType);
		Assert.AreEqual(21855, chunk.Size);
	}

	[TestMethod]
	public void ValidateTocMultipleAssets()
	{
		const string fileName = "dd-nohand";
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(fileName, originalBytes, BinaryReadComprehensiveness.TocOnly);

		Assert.AreEqual(8, modBinary.Chunks.Count);

		string[] names = new[] { "hand", "hand2", "hand2left", "hand3", "hand3left", "hand4", "hand4left", "handleft" };
		int[] sizes = new[] { 166, 166, 198, 166, 198, 262, 390, 198 };
		for (int i = 0; i < 8; i++)
		{
			ModBinaryChunk chunk = modBinary.Chunks[i];
			Assert.AreEqual(names[i], chunk.Name);
			Assert.AreEqual(AssetType.Model, chunk.AssetType);
			Assert.AreEqual(sizes[i], chunk.Size);
		}
	}
}
