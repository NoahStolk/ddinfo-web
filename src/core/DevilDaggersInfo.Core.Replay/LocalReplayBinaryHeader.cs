using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Wiki;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace DevilDaggersInfo.Core.Replay;

public class LocalReplayBinaryHeader : IReplayBinaryHeader<LocalReplayBinaryHeader>
{
	private const int _unknownLength = 10;

	public LocalReplayBinaryHeader(
		int version,
		long timestampSinceGameRelease,
		float time,
		float startTime,
		int daggersFired,
		int deathType,
		int gems,
		int daggersHit,
		int kills,
		int playerId,
		string username,
		byte[] unknown,
		byte[] spawnsetBuffer)
	{
		if (unknown.Length != _unknownLength)
			throw new ArgumentException($"Unknown byte array must be {_unknownLength} bytes long.", nameof(unknown));

		Version = version;
		TimestampSinceGameRelease = timestampSinceGameRelease;
		Time = time;
		StartTime = startTime;
		DaggersFired = daggersFired;
		DeathType = deathType;
		Gems = gems;
		DaggersHit = daggersHit;
		Kills = kills;
		PlayerId = playerId;
		Username = username;
		Unknown = unknown;
		SpawnsetMd5 = MD5.HashData(spawnsetBuffer);
		SpawnsetBuffer = spawnsetBuffer;

		Spawnset = SpawnsetBinary.Parse(spawnsetBuffer);
	}

	private static ReadOnlySpan<byte> Identifier => "ddrpl."u8;

	public int Version { get; }
	public long TimestampSinceGameRelease { get; }
	public float Time { get; }
	public float StartTime { get; }
	public int DaggersFired { get; }
	public int DeathType { get; }
	public int Gems { get; }
	public int DaggersHit { get; }
	public int Kills { get; }
	public int PlayerId { get; }
	public string Username { get; }
	public byte[] Unknown { get; }
	public byte[] SpawnsetMd5 { get; }
	public byte[] SpawnsetBuffer { get; }

	public float Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (float)DaggersFired;

	public SpawnsetBinary Spawnset { get; }

	public static bool UsesLengthPrefixedEvents => true;

	public static int IdentifierLength => Identifier.Length;

	public static LocalReplayBinaryHeader CreateFromByteArray(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		return CreateFromBinaryReader(br);
	}

	public static LocalReplayBinaryHeader CreateFromBinaryReader(BinaryReader br)
	{
		if (!IdentifierIsValid(br, out byte[]? identifier))
		{
			if (identifier == null)
				throw new InvalidReplayBinaryException("Local replay identifier could not be determined.");

			throw new InvalidReplayBinaryException($"'{Encoding.UTF8.GetString(identifier)}' / '{BitConverter.ToString(identifier)}' is not a valid local replay identifier.");
		}

		int version = br.ReadInt32();
		long timestampSinceGameRelease = br.ReadInt64();
		float time = br.ReadSingle();
		float startTime = br.ReadSingle();
		int daggersFired = br.ReadInt32();
		int deathType = br.ReadInt32();
		int gems = br.ReadInt32();
		int daggersHit = br.ReadInt32();
		int kills = br.ReadInt32();
		int playerId = br.ReadInt32();
		int usernameLength = br.ReadInt32();
		byte[] usernameBytes = br.ReadBytes(usernameLength);
		string username = Encoding.UTF8.GetString(usernameBytes);
		byte[] unknown = br.ReadBytes(_unknownLength);
		byte[] spawnsetMd5 = br.ReadBytes(16);
		int spawnsetLength = br.ReadInt32();
		byte[] spawnsetBuffer = br.ReadBytes(spawnsetLength);

		if (!spawnsetMd5.SequenceEqual(MD5.HashData(spawnsetBuffer)))
			throw new InvalidReplayBinaryException("Hashed spawnset data does not match the spawnset buffer.");

		return new(
			version: version,
			timestampSinceGameRelease: timestampSinceGameRelease,
			time: time,
			startTime: startTime,
			daggersFired: daggersFired,
			deathType: deathType,
			gems: gems,
			daggersHit: daggersHit,
			kills: kills,
			playerId: playerId,
			username: username,
			unknown: unknown,
			spawnsetBuffer: spawnsetBuffer);
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

	public static LocalReplayBinaryHeader CreateDefault()
	{
		SpawnsetBinary spawnset = SpawnsetBinary.CreateDefault();
		byte[] spawnsetBuffer = spawnset.ToBytes();
		return new(
			version: 1,
			timestampSinceGameRelease: GetTimestampSinceGameReleaseFromDateTimeOffset(DateTimeOffset.UtcNow),
			time: 0,
			startTime: 0,
			daggersFired: 0,
			deathType: 0,
			gems: 0,
			daggersHit: 0,
			kills: 0,
			playerId: 0,
			username: string.Empty,
			unknown: new byte[_unknownLength],
			spawnsetBuffer: spawnsetBuffer);
	}

	public static DateTimeOffset GetDateTimeOffsetFromTimestampSinceGameRelease(long timestampSinceGameRelease)
		=> GameVersions.GetReleaseDate(GameVersion.V1_0) + TimeSpan.FromSeconds(timestampSinceGameRelease);

	public static long GetTimestampSinceGameReleaseFromDateTimeOffset(DateTimeOffset dateTimeOffset)
		=> (long)(dateTimeOffset - GameVersions.GetReleaseDate(GameVersion.V1_0)).TotalSeconds;

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Identifier);
		bw.Write(Version);
		bw.Write(TimestampSinceGameRelease);
		bw.Write(Time);
		bw.Write(StartTime);
		bw.Write(DaggersFired);
		bw.Write(DeathType);
		bw.Write(Gems);
		bw.Write(DaggersHit);
		bw.Write(Kills);
		bw.Write(PlayerId);
		bw.Write(Username.Length);
		bw.Write(Encoding.UTF8.GetBytes(Username));
		bw.Write(Unknown);
		bw.Write(SpawnsetMd5);
		bw.Write(SpawnsetBuffer.Length);
		bw.Write(SpawnsetBuffer);

		return ms.ToArray();
	}
}
