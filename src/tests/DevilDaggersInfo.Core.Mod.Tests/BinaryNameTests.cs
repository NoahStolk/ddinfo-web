namespace DevilDaggersInfo.Core.Mod.Tests;

[TestClass]
public class BinaryNameTests
{
	[DataTestMethod]
	[DataRow(ModBinaryType.Audio, "mod", "main", "audio-mod-main")]
	[DataRow(ModBinaryType.Dd, "mod", "testbinary", "dd-mod-testbinary")]
	[DataRow(ModBinaryType.Dd, "mod", "mod", "dd-mod-mod")]
	[DataRow(ModBinaryType.Dd, "", "main", "dd--main")]
	[DataRow(ModBinaryType.Dd, "mod", "m", "dd-mod-m")]
	[DataRow(ModBinaryType.Dd, "m", "mod", "dd-m-mod")]
	public void TestBinaryNames(ModBinaryType modBinaryType, string modName, string name, string expectedFullName)
	{
		BinaryName binaryName = new(modBinaryType, name);
		string fullName = binaryName.ToFullName(modName);

		Assert.AreEqual(fullName, expectedFullName);
		Assert.AreEqual(binaryName, BinaryName.Parse(fullName, modName));
	}
}
