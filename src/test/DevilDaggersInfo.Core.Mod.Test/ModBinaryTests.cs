using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod.Extensions;

namespace DevilDaggersInfo.Core.Mod.Test;

[TestClass]
public class ModBinaryTests
{
	[DataTestMethod]
	[DataRow(ModBinaryType.Audio, "audio-empty")]
	[DataRow(ModBinaryType.Dd, "dd-mesh")]
	[DataRow(ModBinaryType.Dd, "dd-mesh-shader-texture")]
	[DataRow(ModBinaryType.Dd, "dd-shader")]
	[DataRow(ModBinaryType.Dd, "dd-skull-1-2-same-texture-copied", true)] // Cannot compare exact texture bytes, because the resizing algorithm is different. TODO: Compile using the same code instead of legacy DDAE.
	[DataRow(ModBinaryType.Dd, "dd-texture")] // This works because the textures are 1x1.
	public void CompareBinaryOutput(ModBinaryType type, string fileName, bool ignoreExactAssetData = false)
	{
		string filePath = Path.Combine("Resources", fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);

		ModBinary modBinary = new(originalBytes, ModBinaryReadFilter.AllAssets);
		ModBinaryBuilder builder = new(type);

		foreach (ModBinaryChunk chunk in modBinary.Toc.Chunks)
		{
			byte[] extractedAsset = modBinary.ExtractAsset(chunk.Name, chunk.AssetType);
			builder.AddAsset(chunk.Name, chunk.AssetType, extractedAsset);
		}

		CollectionAssert.AreEqual(modBinary.Toc.Chunks.ToList(), builder.Chunks.ToList());

		Assert.AreEqual(modBinary.AssetMap.Count, builder.AssetMap.Count);

		if (ignoreExactAssetData)
			return;

		foreach (KeyValuePair<AssetKey, AssetData> asset in modBinary.AssetMap)
			CollectionAssert.AreEqual(asset.Value.Buffer, builder.AssetMap[asset.Key].Buffer);
	}

	[TestMethod]
	public void ValidateTocSingleAsset()
	{
		const string fileName = "dd-single-asset";
		string filePath = Path.Combine("Resources", fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(originalBytes, ModBinaryReadFilter.NoAssets);

		Assert.AreEqual(1, modBinary.Toc.Chunks.Count);
		ModBinaryChunk chunk = modBinary.Toc.Chunks[0];
		Assert.AreEqual("dagger6", chunk.Name);
		Assert.AreEqual(AssetType.Texture, chunk.AssetType);
		Assert.AreEqual(21855, chunk.Size);
	}

	[TestMethod]
	public void ValidateTocMultipleAssets()
	{
		const string fileName = "dd-nohand";
		string filePath = Path.Combine("Resources", fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);
		ModBinary modBinary = new(originalBytes, ModBinaryReadFilter.NoAssets);

		Assert.AreEqual(8, modBinary.Toc.Chunks.Count);

		string[] names = { "hand", "hand2", "hand2left", "hand3", "hand3left", "hand4", "hand4left", "handleft" };
		int[] sizes = { 166, 166, 198, 166, 198, 262, 390, 198 };
		for (int i = 0; i < 8; i++)
		{
			ModBinaryChunk chunk = modBinary.Toc.Chunks[i];
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
