using DevilDaggersInfo.Core.Extensions;
using DevilDaggersInfo.Core.Replay.Enums;
using System.IO.Compression;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinary
{
	public ReplayBinary(byte[] contents, ReplayBinaryReadComprehensiveness readComprehensiveness)
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

		if (readComprehensiveness == ReplayBinaryReadComprehensiveness.Header)
			return;

		int spawnsetLength = br.ReadInt32();
		SpawnsetBuffer = br.ReadBytes(spawnsetLength);
		int dataLength = br.ReadInt32();
		ZLibCompressedEvents = br.ReadBytes(dataLength);
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
	public byte[]? ZLibCompressedEvents { get; }

	public void ParseEvents()
	{
		if (ZLibCompressedEvents == null)
			throw new InvalidOperationException($"Cannot parse events if the events have not been read. Use {nameof(ReplayBinaryReadComprehensiveness)}.{ReplayBinaryReadComprehensiveness.All} to read the complete replay buffer.");

		using MemoryStream ms = new(ZLibCompressedEvents);
		using MemoryStream decompressedEvents = new();
		using DeflateStream deflateStream = new(ms, CompressionMode.Decompress, true);
		deflateStream.CopyTo(decompressedEvents);

		using BinaryReader br = new(decompressedEvents);

		List<IEvent> events = new();
		while (true)
		{
			events.Add(ParseEvent(br));
		}
	}

	private IEvent ParseEvent(BinaryReader br)
	{
		int entityId = 0;

		byte eventType = br.ReadByte();
		return eventType switch
		{
			(byte)EventType.Spawn => ParseSpawnEvent(br, ref entityId),
			//(byte)EventType.EntityPosition => ,
			//(byte)EventType.EntityOrientation => ,
			//(byte)EventType.EntityTarget => ,
			//(byte)EventType.Hit => ,
			//(byte)EventType.Gem => ,
			//(byte)EventType.Transmute => ,
			//(byte)EventType.Inputs => ,
			//(byte)EventType.End => ,
			_ => throw new InvalidReplayBinaryException($"Invalid event type '{eventType}'."),
		};
	}

	private IEntityEvent ParseSpawnEvent(BinaryReader br, ref int entityId)
	{
		entityId++;

		return br.ReadByte() switch
		{
			0x01 => ParseDaggerSpawnEvent(br, entityId),
		};
	}

	private DaggerSpawnEvent ParseDaggerSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		Int16Vec3 position = br.ReadInt16Vec3();
		Int16Mat3x3 orientation = br.ReadInt16Mat3x3();
		_ = br.ReadByte();
		byte type = br.ReadByte();

		return new(entityId, position, orientation, type);
	}
}

public readonly record struct DaggerSpawnEvent(int EntityId, Int16Vec3 Position, Int16Mat3x3 Orientation, byte DaggerType) : IEntityEvent;

public interface IEntityEvent : IEvent { int EntityId { get; } }

public interface IEvent { }

public readonly record struct Int16Vec3(short X, short Y, short Z);

public readonly record struct Int16Mat3x3(short M11, short M12, short M13, short M21, short M22, short M23, short M31, short M32, short M33);

public enum EventType : byte
{
	Spawn = 0x00,
	EntityPosition = 0x01,
	EntityOrientation = 0x02,
	EntityTarget = 0x04,
	Hit = 0x05,
	Gem = 0x06,
	Transmute = 0x07,
	Inputs = 0x09,
	End = 0x0b,
}

public static class BinaryReaderExtensions
{
	public static Int16Vec3 ReadInt16Vec3(this BinaryReader br) => new(br.ReadInt16(), br.ReadInt16(), br.ReadInt16());

	public static Int16Mat3x3 ReadInt16Mat3x3(this BinaryReader br) => new(br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16());
}
