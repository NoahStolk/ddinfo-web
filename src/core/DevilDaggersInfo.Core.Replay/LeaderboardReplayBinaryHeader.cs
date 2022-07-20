using System.Text;

namespace DevilDaggersInfo.Core.Replay;

public class LeaderboardReplayBinaryHeader : IReplayBinaryHeader<LeaderboardReplayBinaryHeader>
{
	private const string _header = "DF_RPL2";

	public LeaderboardReplayBinaryHeader(string username, byte[] unknownBuffer)
	{
		Username = username;
		UnknownBuffer = unknownBuffer;
	}

	public string Username { get; }
	public byte[] UnknownBuffer { get; }

	public static bool UsesLengthPrefixedEvents => false;

	public static LeaderboardReplayBinaryHeader CreateFromByteArray(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		return CreateFromBinaryReader(br);
	}

	public static LeaderboardReplayBinaryHeader CreateFromBinaryReader(BinaryReader br)
	{
		byte[] headerBytes = br.ReadBytes(7);
		string header = Encoding.Default.GetString(headerBytes);
		if (header != _header)
			throw new InvalidReplayBinaryException($"'{header}' / '{headerBytes.ByteArrayToHexString()}' is not a valid leaderboard replay header.");

		short usernameLength = br.ReadInt16();
		byte[] usernameBytes = br.ReadBytes(usernameLength);
		string username = Encoding.Default.GetString(usernameBytes);

		short unknownLength = br.ReadInt16();
		byte[] unknownBuffer = br.ReadBytes(unknownLength);

		return new(
			username: username,
			unknownBuffer: unknownBuffer);
	}

	public static LeaderboardReplayBinaryHeader CreateDefault()
	{
		return new(string.Empty, Array.Empty<byte>());
	}

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Encoding.Default.GetBytes(_header));
		bw.Write((short)Username.Length);
		bw.Write(Encoding.Default.GetBytes(Username));
		bw.Write((short)UnknownBuffer.Length);
		bw.Write(UnknownBuffer);

		return ms.ToArray();
	}
}
