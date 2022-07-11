using DevilDaggersInfo.Common.Utils;
using System.Security.Cryptography;
using System.Text;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinaryHeader
{
	private const string _header = "ddrpl.";

	public ReplayBinaryHeader(
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
		byte[] spawnsetBuffer)
	{
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
		SpawnsetMd5 = MD5.HashData(spawnsetBuffer);
		SpawnsetBuffer = spawnsetBuffer;
	}

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
	public byte[] SpawnsetMd5 { get; }
	public byte[] SpawnsetBuffer { get; }

	public static ReplayBinaryHeader CreateFromByteArray(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		return CreateFromBinaryReader(br);
	}

	public static ReplayBinaryHeader CreateFromBinaryReader(BinaryReader br)
	{
		byte[] headerBytes = br.ReadBytes(6);
		string header = Encoding.Default.GetString(headerBytes);
		if (header != _header)
			throw new InvalidReplayBinaryException($"'{header}' / '{headerBytes.ByteArrayToHexString()}' is not a valid replay header.");

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
		string username = Encoding.Default.GetString(usernameBytes);
		br.BaseStream.Seek(10, SeekOrigin.Current);
		byte[] spawnsetMd5 = br.ReadBytes(16);
		int spawnsetLength = br.ReadInt32();
		byte[] spawnsetBuffer = br.ReadBytes(spawnsetLength);

		if (!ArrayUtils.AreEqual(spawnsetMd5, MD5.HashData(spawnsetBuffer)))
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
			spawnsetBuffer: spawnsetBuffer);
	}

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Encoding.Default.GetBytes(_header));
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
		bw.Write(Encoding.Default.GetBytes(Username));
		bw.Seek(10, SeekOrigin.Current);
		bw.Write(SpawnsetMd5);
		bw.Write(SpawnsetBuffer.Length);
		bw.Write(SpawnsetBuffer);

		return ms.ToArray();
	}
}
