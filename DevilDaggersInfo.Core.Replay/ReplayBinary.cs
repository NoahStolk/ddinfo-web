using DevilDaggersInfo.Core.Extensions;
using DevilDaggersInfo.Core.Replay.Enums;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinary
{
	public ReplayBinary(byte[] contents, ReplayBinaryReadComprehensiveness binaryReadComprehensiveness)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);

		string header = br.ReadFixedLengthString(6);
		if (header != "ddrpl.")
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
		br.BaseStream.Seek(2, SeekOrigin.Current);
		_ = br.ReadInt64(); // Unknown value
		SpawnsetMd5 = br.ReadBytes(16);

		if (binaryReadComprehensiveness == ReplayBinaryReadComprehensiveness.Header)
			return;

		int spawnsetLength = br.ReadInt32();
		SpawnsetBuffer = br.ReadBytes(spawnsetLength);
		int dataLength = br.ReadInt32();
		ZLibCompressedTicks = br.ReadBytes(dataLength);
	}

	public int Version { get; init; }
	public long TimestampSinceGameRelease { get; init; }
	public float Time { get; init; }
	public float StartTime { get; init; }
	public int DaggersFired { get; init; }
	public int DeathType { get; init; }
	public int Gems { get; init; }
	public int DaggersHit { get; init; }
	public int Kills { get; init; }
	public int PlayerId { get; init; }
	public string Username { get; init; }
	public byte[] SpawnsetMd5 { get; init; }

	public byte[]? SpawnsetBuffer { get; init; }
	public byte[]? ZLibCompressedTicks { get; init; }
}
