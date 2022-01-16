using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

[TestClass]
public class AuditLoggerExtensionTests
{
	[TestMethod]
	public void TestAddSpawnsetLog()
	{
		AddSpawnset dto = new()
		{
			Name = "super spawnset",
			PlayerId = 1,
			FileContents = new byte[] { 0x00, 0x01 },
			HtmlDescription = "the super spawnset",
			IsPractice = true,
			MaxDisplayWaves = 80,
		};

		Dictionary<string, string> log = dto.GetLog();
		Assert.AreEqual(6, log.Count);

		Assert.IsTrue(log.ContainsKey("Name"));
		Assert.AreEqual("super spawnset", log["Name"]);
	}
}
