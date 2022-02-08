namespace DevilDaggersInfo.Test.Core.Mod;

[TestClass]
public class CompilationTests
{
	[DataTestMethod]
	[DataRow("pedeblackbody.png", "pedeblackbody", "dd-texture")]
	[DataRow("iconmaskhoming.png", "iconmaskhoming", "dd-iconmaskhoming")]
	public void CompileTextureIntoModBinary(string sourcePngFileName, string assetName, string modFileName)
	{
		byte[] sourcePngContents = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, sourcePngFileName));

		ModBinaryCompiler mbc = new(ModBinaryType.Dd);
		mbc.AddAsset(assetName, AssetType.Texture, sourcePngContents);

		byte[] compiledModBinary = mbc.Compile();

		byte[] sourceModBinary = File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, modFileName));
		TestUtils.AssertArrayContentsEqual(compiledModBinary, sourceModBinary);
	}
}
