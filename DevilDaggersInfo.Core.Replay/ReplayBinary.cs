using DevilDaggersInfo.Core.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinary
{
	private const string _header = "ddrpl.";

	public ReplayBinary(byte[] contents, ReplayBinaryReadComprehensiveness readComprehensiveness)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);

		string header = br.ReadFixedLengthString(6);
		if (header != _header)
			throw new InvalidReplayBinaryException($"'{header}' is not a valid replay header.");

		Version = br.ReadInt32();
		TimestampSinceGameRelease = br.ReadInt64();
		Time = br.ReadSingle();
		StartTime = br.ReadSingle();
		DaggersFired = br.ReadInt32();
		DeathType = br.ReadInt32();
		Gems = br.ReadInt32();
		DaggersHit = br.ReadInt32();
		Kills = br.ReadInt32();
		PlayerId = br.ReadInt32();
		int usernameLength = br.ReadInt32();
		Username = br.ReadFixedLengthString(usernameLength);
		br.BaseStream.Seek(10, SeekOrigin.Current);
		SpawnsetMd5 = br.ReadBytes(16);

		if (readComprehensiveness == ReplayBinaryReadComprehensiveness.Header)
			return;

		int spawnsetLength = br.ReadInt32();
		SpawnsetBuffer = br.ReadBytes(spawnsetLength);
		int compressedDataLength = br.ReadInt32();
		CompressedEvents = br.ReadBytes(compressedDataLength);
	}

	public ReplayBinary(
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
		byte[] spawnsetBuffer,
		byte[] compressedEvents)
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
		CompressedEvents = compressedEvents;
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

	public byte[]? SpawnsetBuffer { get; }
	public byte[]? CompressedEvents { get; }

	public byte[] Compile()
	{
		if (SpawnsetBuffer == null || CompressedEvents == null)
			throw new InvalidOperationException($"Cannot compile a replay that has not been read entirely. Consider using {nameof(ReplayBinaryReadComprehensiveness)}.{ReplayBinaryReadComprehensiveness.All} when reading from a file or buffer.");

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
		bw.Write(CompressedEvents.Length);
		bw.Write(CompressedEvents);

		return ms.ToArray();
	}
}
