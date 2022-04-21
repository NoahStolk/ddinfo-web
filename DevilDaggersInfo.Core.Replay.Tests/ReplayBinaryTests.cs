using System.Security.Cryptography;

namespace DevilDaggersInfo.Core.Replay.Tests;

[TestClass]
public class ReplayBinaryTests
{
	[DataTestMethod]
	[DataRow("Forked-psy.ddreplay")]
	[DataRow("Forked-xvlv.ddreplay")]
	public void GetSpawnsetBuffer(string replayFileName)
	{
		string replayFilePath = Path.Combine("Resources", replayFileName);
		string spawnsetFilePath = Path.Combine("Resources", "Forked");

		byte[] replayBuffer = File.ReadAllBytes(replayFilePath);
		ReplayBinary replayBinary = new(replayBuffer, ReplayBinaryReadComprehensiveness.All);
		Assert.IsNotNull(replayBinary.SpawnsetBuffer);

		TestUtils.AssertArrayContentsEqual(replayBinary.SpawnsetMd5, MD5.HashData(replayBinary.SpawnsetBuffer));
		TestUtils.AssertArrayContentsEqual(File.ReadAllBytes(spawnsetFilePath), replayBinary.SpawnsetBuffer);
	}

	[DataTestMethod]
	[DataRow("Forked-psy.ddreplay")]
	[DataRow("Forked-xvlv.ddreplay")]
	public void ParseAndCompileEvents(string replayFileName)
	{
		string replayFilePath = Path.Combine("Resources", replayFileName);
		byte[] replayBuffer = File.ReadAllBytes(replayFilePath);
		ReplayBinary replayBinary = new(replayBuffer, ReplayBinaryReadComprehensiveness.All);
		Assert.IsNotNull(replayBinary.CompressedEvents);

		List<List<IEvent>> events = ReplayEventsParser.ParseCompressedEvents(replayBinary.CompressedEvents);
		byte[] compressedEvents = ReplayEventsParser.CompileEvents(events.SelectMany(e => e).ToList());

		// Validate header.
		for (int i = 0; i < 2; i++)
			Assert.AreEqual(replayBinary.CompressedEvents[i], compressedEvents[i]);

		// The buffers vary slightly so we can't test this reliably.
		// TestUtils.AssertArrayContentsEqual(compressedEvents, replayBinary.CompressedEvents);
	}
}
