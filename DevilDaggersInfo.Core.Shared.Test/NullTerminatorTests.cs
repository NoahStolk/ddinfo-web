namespace DevilDaggersInfo.Core.Shared.Test;

[TestClass]
public class NullTerminatorTests
{
	[TestMethod]
	public void TestNullTerminatedStrings()
	{
		byte[] buffer = new byte[] { 0x68, 0x61, 0x6E, 0x64, 0, 0x64, 0x64, 0 };

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => buffer.ReadNullTerminatedString(-1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => buffer.ReadNullTerminatedString(buffer.Length));

		Assert.AreEqual("hand", buffer.ReadNullTerminatedString(0));
		Assert.AreEqual("nd", buffer.ReadNullTerminatedString(2));
		Assert.AreEqual("dd", buffer.ReadNullTerminatedString(5));
		Assert.AreEqual(string.Empty, buffer.ReadNullTerminatedString(buffer.Length - 1));

		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);
		string hand = br.ReadNullTerminatedString();
		Assert.AreEqual("hand", hand);
		string dd = br.ReadNullTerminatedString();
		Assert.AreEqual("dd", dd);
	}
}
