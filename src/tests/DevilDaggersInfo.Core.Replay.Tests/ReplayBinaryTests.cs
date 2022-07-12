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

	[TestMethod]
	public void ParseAndCompileEvents()
	{
		ReplayBinary replayBinary = ReplayBinary.CreateDefault();
		replayBinary.EventsPerTick.Clear();
		replayBinary.EventsPerTick.Add(new() { new InitialInputsEvent(true, false, false, false, JumpType.None, true, false, 0, 0, 0.2f) });

		for (int i = 0; i < 30; i++)
			replayBinary.EventsPerTick.Add(new() { new BoidSpawnEvent(i + 1, 0, BoidType.Skull4, default, default, default, default, default, 10), new InputsEvent(true, false, false, false, JumpType.None, false, false, 10, 0) });

		replayBinary.EventsPerTick.Add(new() { new EndEvent() });

		byte[] replayBuffer = replayBinary.Compile();

		ReplayBinary replayBinaryFromBuffer = new(replayBuffer);

		Assert.AreEqual(replayBinary.EventsPerTick.Count, replayBinaryFromBuffer.EventsPerTick.Count);
		for (int i = 0; i < replayBinary.EventsPerTick.Count; i++)
		{
			Assert.AreEqual(replayBinary.EventsPerTick[i].Count, replayBinaryFromBuffer.EventsPerTick[i].Count);
			for (int j = 0; j < replayBinary.EventsPerTick[i].Count; j++)
				Assert.AreEqual(replayBinary.EventsPerTick[i][j], replayBinaryFromBuffer.EventsPerTick[i][j]);
		}
	}
}
