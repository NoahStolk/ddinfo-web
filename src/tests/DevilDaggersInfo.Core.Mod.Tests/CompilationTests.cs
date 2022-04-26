namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class CompilationTests
{
	[DataTestMethod]
	[DataRow("iconmaskhoming.png", "iconmaskhoming", "dd-iconmaskhoming")]
	public void CompileTextureIntoModBinary(string sourcePngFileName, string assetName, string modFileName)
	{
		byte[] sourcePngContents = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, "Texture", sourcePngFileName));

		ModBinary modBinary = new(ModBinaryType.Dd);
		modBinary.AddAsset(assetName, AssetType.Texture, sourcePngContents);

		byte[] compiledModBinary = modBinary.Compile();

		byte[] sourceModBinary = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, modFileName));
		TestUtils.AssertArrayContentsEqual(compiledModBinary, sourceModBinary);
	}
}
