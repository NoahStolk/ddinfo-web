using DevilDaggersInfo.Web.Server.RewriteRules;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

internal sealed class RewriteRulesUtilsTests
{
	[Test]
	public async Task TestTrimStart()
	{
		await Assert.That(RewriteRulesUtils.TrimStart("AudioTest", "Audio")).IsEqualTo("Test");
		await Assert.That(RewriteRulesUtils.TrimStart("AudioAudio", "Audio")).IsEqualTo("Audio");
		await Assert.That(RewriteRulesUtils.TrimStart("AudioTest", "Audio", "Test")).IsEqualTo("Test");
		await Assert.That(RewriteRulesUtils.TrimStart("AudioTest", "Test", "Audio")).IsEqualTo("Test");
	}
}
