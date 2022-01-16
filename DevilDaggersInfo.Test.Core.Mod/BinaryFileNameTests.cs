namespace DevilDaggersInfo.Test.Core.Mod;

[TestClass]
public class BinaryFileNameTests
{
	private const string _modName = "testmod";

	// TODO: Test empty mod name.
	[DataRow($"audio-{_modName}-main", _modName, $"audio-{_modName}-main")]
	[DataRow($"_audio-{_modName}-main", _modName, $"audio-{_modName}-main")]
	[DataRow($"_dd_{_modName}_main", _modName, $"dd-{_modName}-main")]
	[DataRow("_audiodiporg", "dip", "audio-dip-org")]
	[DataTestMethod]
	public void TestBinaryFileNameSanitization(string fileName, string modName, string expectedSanitizedBinaryFileName)
	{
		Assert.AreEqual(expectedSanitizedBinaryFileName, BinaryFileNameUtils.SanitizeModBinaryFileName(fileName, modName));
	}
}
