using DevilDaggersInfo.Web.Server.RewriteRules;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

[TestClass]
public class RewriteRulesUtilsTests
{
	[TestMethod]
	public void TestTrimStart()
	{
		Assert.AreEqual("Test", RewriteRulesUtils.TrimStart("AudioTest", "Audio"));
		Assert.AreEqual("Audio", RewriteRulesUtils.TrimStart("AudioAudio", "Audio"));
		Assert.AreEqual("Test", RewriteRulesUtils.TrimStart("AudioTest", "Audio", "Test"));
		Assert.AreEqual("Test", RewriteRulesUtils.TrimStart("AudioTest", "Test", "Audio"));
	}
}
