using System.Security.Cryptography;

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
		ReplayBinary replayBinary = new(replayBuffer, ReplayBinaryReadComprehensiveness.All);
		Assert.IsNotNull(replayBinary.SpawnsetBuffer);

		TestUtils.AssertArrayContentsEqual(replayBinary.SpawnsetMd5, MD5.HashData(replayBinary.SpawnsetBuffer));
		TestUtils.AssertArrayContentsEqual(File.ReadAllBytes(spawnsetFilePath), replayBinary.SpawnsetBuffer);
	}
}
