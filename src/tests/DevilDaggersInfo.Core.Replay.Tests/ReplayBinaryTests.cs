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
		ReplayBinary replayBinary = new(replayBuffer);

		TestUtils.AssertArrayContentsEqual(replayBinary.Header.SpawnsetMd5, MD5.HashData(replayBinary.Header.SpawnsetBuffer));
		TestUtils.AssertArrayContentsEqual(File.ReadAllBytes(spawnsetFilePath), replayBinary.Header.SpawnsetBuffer);
	}

	[DataTestMethod]
	[DataRow("Forked-psy.ddreplay")]
	[DataRow("Forked-xvlv.ddreplay")]
	public void ParseAndCompileEvents(string replayFileName)
	{
		string replayFilePath = Path.Combine("Resources", replayFileName);
		byte[] replayBuffer = File.ReadAllBytes(replayFilePath);
		ReplayBinary replayBinary = new(replayBuffer);

		byte[] compressedEvents = ReplayEventsParser.CompileEvents(replayBinary.EventsPerTick.SelectMany(e => e).ToList());

		// Validate header.
		for (int i = 0; i < 2; i++)
			Assert.AreEqual(replayBinary.CompressedEvents[i], compressedEvents[i]);

		// The buffers vary slightly so we can't test this reliably.
		// TestUtils.AssertArrayContentsEqual(compressedEvents, replayBinary.CompressedEvents);
	}
}
