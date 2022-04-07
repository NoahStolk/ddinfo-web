using DevilDaggersInfo.Core.Extensions;
using DevilDaggersInfo.Core.Replay.Enums;
using System.IO.Compression;
using System.Numerics;

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
		int entityId = 0;
		bool parsedInitialInput = false;

		while (true)
		{
			byte eventType = br.ReadByte();
			IEvent e = eventType switch
			{
				0x00 => ParseSpawnEvent(br, ref entityId),
				0x01 => ParseEntityPositionEvent(br),
				0x02 => ParseEntityOrientationEvent(br),
				0x04 => ParseEntityTargetEvent(br),
				0x05 => ParseHitEvent(br),
				0x06 => new GemEvent(),
				0x07 => ParseTransmuteEvent(br),
				0x09 => parsedInitialInput ? ParseInputsEvent(br) : ParseInitialInputsEvent(br),
				0x0b => new EndEvent(),
				_ => throw new InvalidReplayBinaryException($"Invalid event type '{eventType}'."),
			};
			events.Add(e);

			if (e is InitialInputsEvent)
				parsedInitialInput = true;
			else if (e is EndEvent)
				break;
		}
	}

	private IEntityEvent ParseSpawnEvent(BinaryReader br, ref int entityId)
	{
		entityId++;

		byte entityType = br.ReadByte();
		return entityType switch
		{
			0x01 => ParseDaggerSpawnEvent(br, entityId),
			0x03 or 0x04 or 0x05 => ParseSquidSpawnEvent(br, entityType, entityId),
			0x06 => ParseBoidSpawnEvent(br, entityId),
			0x07 or 0x0c or 0x0f => ParsePedeSpawnEvent(br, entityType, entityId),
			0x08 or 0x09 => ParseSpiderSpawnEvent(br, entityType, entityId),
			0x0a => ParseSpiderEggSpawnEvent(br, entityId),
			0x0b => ParseLeviathanSpawnEvent(br, entityId),
			0x0d => ParseThornSpawnEvent(br, entityId),
			_ => throw new InvalidReplayBinaryException($"Invalid entity type '{entityType}'."),
		};
	}

	private EntityPositionEvent ParseEntityPositionEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Vec3 position = br.ReadInt16Vec3();
		return new(entityId, position);
	}

	private EntityOrientationEvent ParseEntityOrientationEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Mat3x3 orientation = br.ReadInt16Mat3x3();
		return new(entityId, orientation);
	}

	private EntityTargetEvent ParseEntityTargetEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Vec3 position = br.ReadInt16Vec3();
		return new(entityId, position);
	}

	private HitEvent ParseHitEvent(BinaryReader br)
	{
		return new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
	}

	private TransmuteEvent ParseTransmuteEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		return new(entityId, br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3());
	}

	private InputsEvent ParseInputsEvent(BinaryReader br)
	{
		byte left = br.ReadByte();
		byte right = br.ReadByte();
		byte forward = br.ReadByte();
		byte backward = br.ReadByte();
		byte jump = br.ReadByte();
		byte shoot = br.ReadByte();
		byte shootHoming = br.ReadByte();
		short mouseX = br.ReadInt16();
		short mouseY = br.ReadInt16();

		byte end = br.ReadByte();
		const byte expectedEnd = 0x0a;
		if (end != expectedEnd)
			throw new InvalidReplayBinaryException($"Invalid end of inputs event. Should be {expectedEnd} but got {end}.");

		return new(left, right, forward, backward, jump, shoot, shootHoming, mouseX, mouseY);
	}

	private InitialInputsEvent ParseInitialInputsEvent(BinaryReader br)
	{
		byte left = br.ReadByte();
		byte right = br.ReadByte();
		byte forward = br.ReadByte();
		byte backward = br.ReadByte();
		byte jump = br.ReadByte();
		byte shoot = br.ReadByte();
		byte shootHoming = br.ReadByte();
		short mouseX = br.ReadInt16();
		short mouseY = br.ReadInt16();
		float lookSpeed = br.ReadSingle();

		byte end = br.ReadByte();
		const byte expectedEnd = 0x0a;
		if (end != expectedEnd)
			throw new InvalidReplayBinaryException($"Invalid end of inputs event. Should be {expectedEnd} but got {end}.");

		return new(left, right, forward, backward, jump, shoot, shootHoming, mouseX, mouseY, lookSpeed);
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

	private SquidSpawnEvent ParseSquidSpawnEvent(BinaryReader br, byte entityType, int entityId)
	{
		SquidType squidType = entityType switch
		{
			0x03 => SquidType.Squid1,
			0x04 => SquidType.Squid2,
			0x05 => SquidType.Squid3,
			_ => throw new InvalidOperationException($"Entity type '{entityType}' is not a Squid."),
		};

		_ = br.ReadInt32();
		Vector3 position = br.ReadVector3();
		_ = br.ReadVector3();
		float rotationInRadians = br.ReadSingle();

		return new(entityId, squidType, position, rotationInRadians);
	}

	private BoidSpawnEvent ParseBoidSpawnEvent(BinaryReader br, int entityId)
	{
		int spawner = br.ReadInt32();
		byte boidTypeByte = br.ReadByte();
		Int16Vec3 position = br.ReadInt16Vec3();
		_ = br.ReadInt16Vec3();
		_ = br.ReadInt16Vec3();
		_ = br.ReadInt16Vec3();
		_ = br.ReadVector3();
		float speed = br.ReadSingle();

		BoidType boidType = boidTypeByte switch
		{
			0x01 => BoidType.Skull1,
			0x02 => BoidType.Skull2,
			0x03 => BoidType.Skull3,
			0x04 => BoidType.Spiderling,
			0x05 => BoidType.Skull4,
			_ => throw new InvalidOperationException($"Invalid boid type '{boidTypeByte}'."),
		};

		return new(entityId, spawner, boidType, position, speed);
	}

	private PedeSpawnEvent ParsePedeSpawnEvent(BinaryReader br, byte entityType, int entityId)
	{
		PedeType pedeType = entityType switch
		{
			0x07 => PedeType.Centipede,
			0x0c => PedeType.Gigapede,
			0x0f => PedeType.Ghostpede,
			_ => throw new InvalidOperationException($"Entity type '{entityType}' is not a Pede."),
		};

		_ = br.ReadInt32();
		Vector3 position = br.ReadVector3();
		_ = br.ReadVector3();
		_ = br.ReadVector3();
		_ = br.ReadVector3();
		_ = br.ReadVector3();

		return new(entityId, pedeType, position);
	}

	private SpiderSpawnEvent ParseSpiderSpawnEvent(BinaryReader br, byte entityType, int entityId)
	{
		SpiderType spiderType = entityType switch
		{
			0x08 => SpiderType.Spider1,
			0x09 => SpiderType.Spider2,
			_ => throw new InvalidOperationException($"Entity type '{entityType}' is not a Spider."),
		};

		_ = br.ReadInt32();
		Vector3 position = br.ReadVector3();

		return new(entityId, spiderType, position);
	}

	private SpiderEggSpawnEvent ParseSpiderEggSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32(); // Spider Egg Type?
		Vector3 position = br.ReadVector3(); // Not sure
		_ = br.ReadVector3();

		return new(entityId, position);
	}

	private LeviathanSpawnEvent ParseLeviathanSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		return new(entityId);
	}

	private ThornSpawnEvent ParseThornSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		Vector3 position = br.ReadVector3(); // Not sure
		float rotationInRadians = br.ReadSingle(); // Not sure
		return new(entityId, position, rotationInRadians);
	}
}

public readonly record struct ThornSpawnEvent(int EntityId, Vector3 Position, float RotationInRadians) : IEntityEvent;

public readonly record struct LeviathanSpawnEvent(int EntityId) : IEntityEvent;

public readonly record struct SpiderEggSpawnEvent(int EntityId, Vector3 Position) : IEntityEvent;

public readonly record struct SpiderSpawnEvent(int EntityId, SpiderType SpiderType, Vector3 Position) : IEntityEvent;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, Vector3 Position) : IEntityEvent;

public readonly record struct BoidSpawnEvent(int EntityId, int SpawnerId, BoidType BoidType, Int16Vec3 Position, float Speed) : IEntityEvent;

public readonly record struct SquidSpawnEvent(int EntityId, SquidType SquidType, Vector3 Position, float RotationInRadians) : IEntityEvent;

public readonly record struct DaggerSpawnEvent(int EntityId, Int16Vec3 Position, Int16Mat3x3 Orientation, byte DaggerType) : IEntityEvent;

public readonly record struct EntityPositionEvent(int EntityId, Int16Vec3 Position) : IEntityEvent;

public readonly record struct EntityOrientationEvent(int EntityId, Int16Mat3x3 Orientation) : IEntityEvent;

public readonly record struct EntityTargetEvent(int EntityId, Int16Vec3 Position) : IEntityEvent;

public readonly record struct HitEvent(int A, int B, int C) : IEvent;

public readonly record struct GemEvent() : IEvent;

public readonly record struct TransmuteEvent(int EntityId, Int16Vec3 A, Int16Vec3 B, Int16Vec3 C, Int16Vec3 D) : IEntityEvent;

public readonly record struct InputsEvent(byte Left, byte Right, byte Forward, byte Backward, byte Jump, byte Shoot, byte ShootHoming, short MouseX, short MouseY) : IEvent;

public readonly record struct InitialInputsEvent(byte Left, byte Right, byte Forward, byte Backward, byte Jump, byte Shoot, byte ShootHoming, short MouseX, short MouseY, float LookSpeed) : IEvent;

public readonly record struct EndEvent() : IEvent;

public interface IEntityEvent : IEvent { int EntityId { get; } }

public interface IEvent { }

public readonly record struct Int16Vec3(short X, short Y, short Z);

public readonly record struct Int16Mat3x3(short M11, short M12, short M13, short M21, short M22, short M23, short M31, short M32, short M33);

public enum BoidType : byte
{
	Skull1 = 1,
	Skull2 = 2,
	Skull3 = 3,
	Spiderling = 4,
	Skull4 = 5,
}

public enum SquidType : byte
{
	Squid1 = 1,
	Squid2 = 2,
	Squid3 = 3,
}

public enum PedeType : byte
{
	Centipede = 1,
	Gigapede = 2,
	Ghostpede = 3,
}

public enum SpiderType : byte
{
	Spider1 = 1,
	Spider2 = 2,
}

public static class BinaryReaderExtensions
{
	public static Int16Vec3 ReadInt16Vec3(this BinaryReader br) => new(br.ReadInt16(), br.ReadInt16(), br.ReadInt16());

	public static Int16Mat3x3 ReadInt16Mat3x3(this BinaryReader br) => new(br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16());

	public static Vector3 ReadVector3(this BinaryReader br) => new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
}
