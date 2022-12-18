using DevilDaggersInfo.Core.Mod.Extensions;
using DevilDaggersInfo.Types.Core.Assets;
using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class ModBinaryTests
{
	[DataTestMethod]
	[DataRow("dd-mesh")]
	[DataRow("dd-mesh-shader-texture")]
	[DataRow("dd-shader")]
	[DataRow("dd-skull-1-2-same-texture-copied")]
	[DataRow("dd-texture")]
	public void CompareBinaryOutput(string fileName)
	{
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(originalBytes, ModBinaryReadComprehensiveness.All);
		byte[] bytes = modBinary.Compile();

		CollectionAssert.AreEqual(originalBytes, bytes);
	}

	[TestMethod]
	public void ValidateTocSingleAsset()
	{
		const string fileName = "dd-single-asset";
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(originalBytes, ModBinaryReadComprehensiveness.TocOnly);

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
		ModBinary modBinary = new(originalBytes, ModBinaryReadComprehensiveness.TocOnly);

		Assert.AreEqual(8, modBinary.Chunks.Count);

		string[] names = { "hand", "hand2", "hand2left", "hand3", "hand3left", "hand4", "hand4left", "handleft" };
		int[] sizes = { 166, 166, 198, 166, 198, 262, 390, 198 };
		for (int i = 0; i < 8; i++)
		{
			ModBinaryChunk chunk = modBinary.Chunks[i];
			Assert.AreEqual(names[i], chunk.Name);
			Assert.AreEqual(AssetType.Mesh, chunk.AssetType);
			Assert.AreEqual(sizes[i], chunk.Size);
		}
	}

	[TestMethod]
	public void ValidateAssetTypes()
	{
		Assert.IsTrue(ModBinaryType.Audio.IsAssetTypeValid(AssetType.Audio));
		Assert.IsFalse(ModBinaryType.Audio.IsAssetTypeValid(AssetType.Mesh));
		Assert.IsFalse(ModBinaryType.Audio.IsAssetTypeValid(AssetType.ObjectBinding));
		Assert.IsFalse(ModBinaryType.Audio.IsAssetTypeValid(AssetType.Shader));
		Assert.IsFalse(ModBinaryType.Audio.IsAssetTypeValid(AssetType.Texture));

		Assert.IsFalse(ModBinaryType.Dd.IsAssetTypeValid(AssetType.Audio));
		Assert.IsTrue(ModBinaryType.Dd.IsAssetTypeValid(AssetType.Mesh));
		Assert.IsTrue(ModBinaryType.Dd.IsAssetTypeValid(AssetType.ObjectBinding));
		Assert.IsTrue(ModBinaryType.Dd.IsAssetTypeValid(AssetType.Shader));
		Assert.IsTrue(ModBinaryType.Dd.IsAssetTypeValid(AssetType.Texture));
	}
}
