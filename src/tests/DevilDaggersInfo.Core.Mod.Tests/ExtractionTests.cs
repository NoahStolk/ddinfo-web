using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class ExtractionTests
{
	[DataTestMethod]
	[DataRow(ModBinaryType.Dd, "dd-texture", "pedeblackbody", "pedeblackbody.png")]
	[DataRow(ModBinaryType.Dd, "dd-iconmaskhoming", "iconmaskhoming", "iconmaskhoming.png")]
	public void ExtractTextureAndCompareToSourcePng(ModBinaryType expectedBinaryType, string modFileName, string assetName, string sourcePngFileName)
	{
		string filePath = Path.Combine(TestUtils.ResourcePath, modFileName);
		ModBinary modBinary = new(File.ReadAllBytes(filePath), ModBinaryReadFilter.AllAssets);
		Assert.AreEqual(expectedBinaryType, modBinary.Toc.Type);
		Assert.AreEqual(expectedBinaryType, BinaryFileNameUtils.GetBinaryTypeBasedOnFileName(modFileName));

		KeyValuePair<AssetKey, AssetData> asset = modBinary.AssetMap.First(kvp => kvp.Key.AssetName == assetName);

		byte[] extractedPngContents = modBinary.ExtractAsset(asset.Key);
		byte[] sourcePngContents = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Texture", sourcePngFileName));

		CollectionAssert.AreEqual(extractedPngContents, sourcePngContents);
	}
}
