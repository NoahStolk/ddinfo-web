namespace DevilDaggersInfo.Test.Core.Wiki;

[TestClass]
public class ColorTests
{
	[TestMethod]
	public void TestHexCode()
	{
		Assert.AreEqual("#FFFFFF", new Color(0xFF, 0xFF, 0xFF).HexCode);
		Assert.AreEqual("#000000", new Color(0x00, 0x00, 0x00).HexCode);
		Assert.AreEqual("#00A000", new Color(0x00, 0xA0, 0x00).HexCode);
		Assert.AreEqual("#64AEB1", new Color(0x64, 0xAE, 0xB1).HexCode);
	}
}
