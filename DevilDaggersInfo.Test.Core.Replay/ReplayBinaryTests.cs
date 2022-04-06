namespace DevilDaggersInfo.Test.Core.Replay;

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
		byte[] spawnsetBuffer = ReplayBinary.GetSpawnsetBuffer(replayBuffer);
		TestUtils.AssertArrayContentsEqual(File.ReadAllBytes(spawnsetFilePath), spawnsetBuffer);
	}
}
