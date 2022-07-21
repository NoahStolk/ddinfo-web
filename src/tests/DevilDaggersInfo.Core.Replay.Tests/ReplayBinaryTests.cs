using System.Security.Cryptography;
using System.Text;

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
		ReplayBinary<LocalReplayBinaryHeader> replayBinary = new(replayBuffer);

		TestUtils.AssertArrayContentsEqual(replayBinary.Header.SpawnsetMd5, MD5.HashData(replayBinary.Header.SpawnsetBuffer));
		TestUtils.AssertArrayContentsEqual(File.ReadAllBytes(spawnsetFilePath), replayBinary.Header.SpawnsetBuffer);
	}

	[TestMethod]
	public void ParseAndCompileEvents()
	{
		ReplayBinary<LocalReplayBinaryHeader> replayBinary = ReplayBinary<LocalReplayBinaryHeader>.CreateDefault();
		replayBinary.EventsPerTick.Clear();
		replayBinary.EventsPerTick.Add(new() { new InitialInputsEvent(true, false, false, false, JumpType.None, ShootType.Hold, ShootType.None, 0, 0, 0.2f) });

		for (int i = 0; i < 30; i++)
			replayBinary.EventsPerTick.Add(new() { new BoidSpawnEvent(i + 1, 0, BoidType.Skull4, default, default, default, default, default, 10), new InputsEvent(true, false, false, false, JumpType.None, ShootType.None, ShootType.None, 10, 0) });

		replayBinary.EventsPerTick.Add(new() { new EndEvent() });

		byte[] replayBuffer = replayBinary.Compile();

		ReplayBinary<LocalReplayBinaryHeader> replayBinaryFromBuffer = new(replayBuffer);

		Assert.AreEqual(replayBinary.EventsPerTick.Count, replayBinaryFromBuffer.EventsPerTick.Count);
		for (int i = 0; i < replayBinary.EventsPerTick.Count; i++)
		{
			Assert.AreEqual(replayBinary.EventsPerTick[i].Count, replayBinaryFromBuffer.EventsPerTick[i].Count);
			for (int j = 0; j < replayBinary.EventsPerTick[i].Count; j++)
				Assert.AreEqual(replayBinary.EventsPerTick[i][j], replayBinaryFromBuffer.EventsPerTick[i][j]);
		}
	}

	[DataTestMethod]
	[DataRow("ddrpl.", true)]
	[DataRow("ddrpl..", true)]
	[DataRow("ddrpl..abc", true)]
	[DataRow("ddRpl.", false)]
	[DataRow("ddrpl", false)]
	[DataRow("dd", false)]
	[DataRow("DF_RPL2", false)]
	[DataRow("", false)]
	[DataRow("drdpl.", false)]
	public void TestValidateLocalReplayHeaderIdentifier(string identifier, bool isValid)
	{
		byte[] identifierBytes = Encoding.UTF8.GetBytes(identifier);
		Assert.AreEqual(isValid, LocalReplayBinaryHeader.IdentifierIsValid(identifierBytes, out _));
	}

	[DataTestMethod]
	[DataRow("DF_RPL2", true)]
	[DataRow("DF_RPL22", true)]
	[DataRow("DF_RPL22abc", true)]
	[DataRow("Df_RPL2", false)]
	[DataRow("DF_RPL", false)]
	[DataRow("DF", false)]
	[DataRow("ddrpl.", false)]
	[DataRow("", false)]
	[DataRow("D_FRPL2", false)]
	public void TestValidateLeaderboardReplayHeaderIdentifier(string identifier, bool isValid)
	{
		byte[] identifierBytes = Encoding.UTF8.GetBytes(identifier);
		Assert.AreEqual(isValid, LeaderboardReplayBinaryHeader.IdentifierIsValid(identifierBytes, out _));
	}
}
