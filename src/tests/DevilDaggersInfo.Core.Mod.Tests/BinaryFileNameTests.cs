namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class BinaryFileNameTests
{
	private const string _modName = "testmod";

	[DataRow(ModBinaryType.Audio, $"audio-{_modName}-main", _modName, $"audio-{_modName}-main")]
	[DataRow(ModBinaryType.Audio, $"_audio-{_modName}-main", _modName, $"audio-{_modName}-main")]
	[DataRow(ModBinaryType.Dd, $"_dd_{_modName}_main", _modName, $"dd-{_modName}-main")]
	[DataRow(ModBinaryType.Audio, "_audiodiporg", "dip", "audio-dip-org")]
	[DataRow(ModBinaryType.Audio, "_dddiporg", "dip", "audio-dip-org")]
	[DataRow(ModBinaryType.Dd, "main", "", "dd--main")]
	[DataTestMethod]
	public void TestBinaryFileNameSanitization(ModBinaryType modBinaryType, string fileName, string modName, string expectedSanitizedBinaryFileName)
	{
		Assert.AreEqual(expectedSanitizedBinaryFileName, BinaryFileNameUtils.SanitizeModBinaryFileName(modBinaryType, fileName, modName));
	}
}
