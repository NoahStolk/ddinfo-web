namespace DevilDaggersInfo.Test.Core.Mod;

[TestClass]
public class ExtractionTests
{
	[TestMethod]
	public void ExtractTexture()
	{
		const string modFileName = "dd-texture";
		string filePath = Path.Combine(TestUtils.ResourcePath, modFileName);
		ModBinary modBinary = new(modFileName, File.ReadAllBytes(filePath), BinaryReadComprehensiveness.All);
		KeyValuePair<ModBinaryChunk, AssetData> asset = modBinary.AssetMap!.First(kvp => kvp.Key.Name == "pedeblackbody");

		byte[] extractedPng = AssetConverter.Extract(asset.Key.AssetType, asset.Value);
		byte[] pngContents = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "pedeblackbody.png"));

		TestUtils.AssertArrayContentsEqual(extractedPng, pngContents);
	}
}
