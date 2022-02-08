namespace DevilDaggersInfo.Test.Core.Mod;

[TestClass]
public class ExtractionTests
{
	[DataTestMethod]
	[DataRow("dd-texture", "pedeblackbody", "pedeblackbody.png")]
	[DataRow("dd-iconmaskhoming", "iconmaskhoming", "iconmaskhoming.png")]
	public void ExtractTextureAndCompareToSourcePng(string modFileName, string assetName, string sourcePngFileName)
	{
		string filePath = Path.Combine(TestUtils.ResourcePath, modFileName);
		ModBinary modBinary = new(modFileName, File.ReadAllBytes(filePath), BinaryReadComprehensiveness.All);
		Assert.IsNotNull(modBinary.AssetMap);
		KeyValuePair<ModBinaryChunk, AssetData> asset = modBinary.AssetMap.First(kvp => kvp.Key.Name == assetName);

		byte[] extractedPng = AssetConverter.Extract(asset.Key.AssetType, asset.Value);
		byte[] pngContents = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, sourcePngFileName));

		TestUtils.AssertArrayContentsEqual(extractedPng, pngContents);
	}
}
