namespace DevilDaggersInfo.Core.Shared.Test;

[TestClass]
public class StringTests
{
	[TestMethod]
	public void TestTrimStart()
	{
		Assert.AreEqual("Test", "AudioTest".TrimStart("Audio"));
		Assert.AreEqual("Audio", "AudioAudio".TrimStart("Audio"));
		Assert.AreEqual("Test", "AudioTest".TrimStart("Audio", "Test"));
		Assert.AreEqual("Test", "AudioTest".TrimStart("Test", "Audio"));
	}
}
