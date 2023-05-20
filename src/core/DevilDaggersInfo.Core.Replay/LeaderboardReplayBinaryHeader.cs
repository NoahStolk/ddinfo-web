using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DevilDaggersInfo.Core.Replay;

public class LeaderboardReplayBinaryHeader : IReplayBinaryHeader<LeaderboardReplayBinaryHeader>
{
	public LeaderboardReplayBinaryHeader(string username, byte[] unknownBuffer)
	{
		Username = username;
		UnknownBuffer = unknownBuffer;
	}

	private static ReadOnlySpan<byte> Identifier => "DF_RPL2"u8;

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
		if (!IdentifierIsValid(br, out byte[]? identifier))
		{
			if (identifier == null)
				throw new InvalidReplayBinaryException("Leaderboard replay identifier could not be determined.");

			throw new InvalidReplayBinaryException($"'{Encoding.UTF8.GetString(identifier)}' / '{identifier.ByteArrayToHexString()}' is not a valid leaderboard replay identifier.");
		}

		short usernameLength = br.ReadInt16();
		byte[] usernameBytes = br.ReadBytes(usernameLength);
		string username = Encoding.UTF8.GetString(usernameBytes);

		short unknownLength = br.ReadInt16();
		byte[] unknownBuffer = br.ReadBytes(unknownLength);

		return new(
			username: username,
			unknownBuffer: unknownBuffer);
	}

	public static bool IdentifierIsValid(byte[] contents, [MaybeNullWhen(false)] out byte[]? identifier)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		return IdentifierIsValid(br, out identifier);
	}

	public static bool IdentifierIsValid(BinaryReader br, [MaybeNullWhen(false)] out byte[]? identifier)
	{
		identifier = null;
		if (br.BaseStream.Position > br.BaseStream.Length - Identifier.Length)
			return false;

		identifier = br.ReadBytes(Identifier.Length);
		return Identifier.SequenceEqual(identifier);
	}

	public static LeaderboardReplayBinaryHeader CreateDefault()
	{
		return new(string.Empty, Array.Empty<byte>());
	}

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Identifier);
		bw.Write((short)Username.Length);
		bw.Write(Encoding.UTF8.GetBytes(Username));
		bw.Write((short)UnknownBuffer.Length);
		bw.Write(UnknownBuffer);

		return ms.ToArray();
	}
}
