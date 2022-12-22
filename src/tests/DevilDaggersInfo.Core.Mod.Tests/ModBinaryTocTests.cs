using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class ModBinaryTocTests
{
	[DataTestMethod]
	[DataRow(ModBinaryType.Audio, "audio-empty")]
	[DataRow(ModBinaryType.Dd, "dd-mesh")]
	[DataRow(ModBinaryType.Dd, "dd-mesh-shader-texture")]
	[DataRow(ModBinaryType.Dd, "dd-shader")]
	[DataRow(ModBinaryType.Dd, "dd-skull-1-2-same-texture-copied")]
	[DataRow(ModBinaryType.Dd, "dd-texture")]
	public void DetermineModBinaryType(ModBinaryType expectedType, string fileName)
	{
		string filePath = Path.Combine(TestUtils.ResourcePath, fileName);
		byte[] originalBytes = File.ReadAllBytes(filePath);

		ModBinaryType type = ModBinaryToc.DetermineType(originalBytes);
		Assert.AreEqual(expectedType, type);
	}
}
