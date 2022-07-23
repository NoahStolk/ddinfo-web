using System.IO.Compression;

namespace DevilDaggersInfo.Core.Replay;

public static class ReplayEventsParser
{
	public static byte[] CompileEvents(List<IEvent> events)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		foreach (IEvent e in events)
			e.Write(bw);

		return Compress(ms.ToArray());
	}

	private static byte[] Compress(byte[] data)
	{
		using MemoryStream memoryStream = new();
		using (DeflateStream deflateStream = new(memoryStream, CompressionLevel.SmallestSize))
		{
			deflateStream.Write(data, 0, data.Length);
		}

		byte[] compressedData = memoryStream.ToArray();

		byte[] compressedDataWithHeader = new byte[2 + compressedData.Length];
		Buffer.BlockCopy(new byte[] { 120, 1 }, 0, compressedDataWithHeader, 0, 2);
		Buffer.BlockCopy(compressedData, 0, compressedDataWithHeader, 2, compressedData.Length);
		return compressedDataWithHeader;
	}

	public static List<List<IEvent>> ParseCompressedEvents(byte[] compressedEvents)
	{
		using MemoryStream ms = new(compressedEvents[2..]); // Skip ZLIB header.
		using DeflateStream deflateStream = new(ms, CompressionMode.Decompress, true);

		using BinaryReader br = new(deflateStream);
		List<List<IEvent>> events = new();
		int entityId = 0;
		bool parsedInitialInput = false;

		List<IEvent> eventsInTick = new();
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
				0x06 => default(GemEvent),
				0x07 => ParseTransmuteEvent(br),
				0x09 => parsedInitialInput ? ParseInputsEvent(br) : ParseInitialInputsEvent(br),
				0x0b => default(EndEvent),
				_ => throw new InvalidReplayBinaryException($"Invalid event type '{eventType}'."),
			};
			eventsInTick.Add(e);

			if (e is InitialInputsEvent)
				parsedInitialInput = true;

			if (e is InitialInputsEvent or InputsEvent or EndEvent)
			{
				events.Add(eventsInTick);
				eventsInTick = new();
			}

			if (e is EndEvent)
				break;
		}

		return events;
	}

	private static IEvent ParseSpawnEvent(BinaryReader br, ref int entityId)
	{
		entityId++;

		byte entityType = br.ReadByte();
		return entityType switch
		{
			0x01 => ParseDaggerSpawnEvent(br, entityId),
			0x03 => ParseSquidSpawnEvent(br, SquidType.Squid1, entityId),
			0x04 => ParseSquidSpawnEvent(br, SquidType.Squid2, entityId),
			0x05 => ParseSquidSpawnEvent(br, SquidType.Squid3, entityId),
			0x06 => ParseBoidSpawnEvent(br, entityId),
			0x07 => ParsePedeSpawnEvent(br, PedeType.Centipede, entityId),
			0x0c => ParsePedeSpawnEvent(br, PedeType.Gigapede, entityId),
			0x0f => ParsePedeSpawnEvent(br, PedeType.Ghostpede, entityId),
			0x08 => ParseSpiderSpawnEvent(br, SpiderType.Spider1, entityId),
			0x09 => ParseSpiderSpawnEvent(br, SpiderType.Spider2, entityId),
			0x0a => ParseSpiderEggSpawnEvent(br, entityId),
			0x0b => ParseLeviathanSpawnEvent(br, entityId),
			0x0d => ParseThornSpawnEvent(br, entityId),
			_ => throw new InvalidReplayBinaryException($"Invalid entity type '{entityType}'."),
		};
	}

	private static EntityPositionEvent ParseEntityPositionEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Vec3 position = br.ReadInt16Vec3();
		return new(entityId, position);
	}

	private static EntityOrientationEvent ParseEntityOrientationEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Mat3x3 orientation = br.ReadInt16Mat3x3();
		return new(entityId, orientation);
	}

	private static EntityTargetEvent ParseEntityTargetEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		Int16Vec3 targetPosition = br.ReadInt16Vec3();
		return new(entityId, targetPosition);
	}

	private static IEvent ParseHitEvent(BinaryReader br)
	{
		// Examples:
		// 1. When a dagger is deleted from the scene; A is the entity ID of the dagger and B is 0.
		// 2. When a dagger is eaten by Ghostpede; A is the entity ID of the Ghostpede and B is the entity ID of the dagger.
		// 3. When a Level 4 homing splash 'dagger' hits a Squid I; A is the entity ID of the Squid I and B is the entity ID of the homing splash 'dagger'.
		int entityIdA = br.ReadInt32();
		if (entityIdA == 0)
			return ParseDeathEvent(br);

		int entityIdB = br.ReadInt32();
		int c = br.ReadInt32();
		return new HitEvent(entityIdA, entityIdB, c);
	}

	private static DeathEvent ParseDeathEvent(BinaryReader br)
	{
		int deathType = br.ReadInt32();
		_ = br.ReadInt32();
		return new(deathType);
	}

	private static TransmuteEvent ParseTransmuteEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		return new(entityId, br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3());
	}

	private static InputsEvent ParseInputsEvent(BinaryReader br)
	{
		bool left = br.ReadBoolean();
		bool right = br.ReadBoolean();
		bool forward = br.ReadBoolean();
		bool backward = br.ReadBoolean();

		byte jumpTypeByte = br.ReadByte();
		JumpType jumpType = jumpTypeByte.ToJumpType();

		byte shootTypeByte = br.ReadByte();
		ShootType shootType = shootTypeByte.ToShootType();

		byte shootTypeByteHoming = br.ReadByte();
		ShootType shootTypeHoming = shootTypeByteHoming.ToShootType();

		short mouseX = br.ReadInt16();
		short mouseY = br.ReadInt16();

		byte end = br.ReadByte();
		const byte expectedEnd = 0x0a;
		if (end != expectedEnd)
			throw new InvalidReplayBinaryException($"Invalid end of inputs event. Should be {expectedEnd} but got {end}.");

		return new(left, right, forward, backward, jumpType, shootType, shootTypeHoming, mouseX, mouseY);
	}

	private static InitialInputsEvent ParseInitialInputsEvent(BinaryReader br)
	{
		bool left = br.ReadBoolean();
		bool right = br.ReadBoolean();
		bool forward = br.ReadBoolean();
		bool backward = br.ReadBoolean();

		byte jumpTypeByte = br.ReadByte();
		JumpType jumpType = jumpTypeByte.ToJumpType();

		byte shootTypeByte = br.ReadByte();
		ShootType shootType = shootTypeByte.ToShootType();

		byte shootTypeByteHoming = br.ReadByte();
		ShootType shootTypeHoming = shootTypeByteHoming.ToShootType();

		short mouseX = br.ReadInt16();
		short mouseY = br.ReadInt16();
		float lookSpeed = br.ReadSingle();

		byte end = br.ReadByte();
		const byte expectedEnd = 0x0a;
		if (end != expectedEnd)
			throw new InvalidReplayBinaryException($"Invalid end of inputs event. Should be {expectedEnd} but got {end}.");

		return new(left, right, forward, backward, jumpType, shootType, shootTypeHoming, mouseX, mouseY, lookSpeed);
	}

	private static DaggerSpawnEvent ParseDaggerSpawnEvent(BinaryReader br, int entityId)
	{
		int a = br.ReadInt32(); // Always 0
		Int16Vec3 position = br.ReadInt16Vec3();
		Int16Mat3x3 orientation = br.ReadInt16Mat3x3();
		bool isShot = br.ReadBoolean();
		byte daggerTypeByte = br.ReadByte();
		DaggerType daggerType = daggerTypeByte.ToDaggerType();

		return new(entityId, a, position, orientation, isShot, daggerType);
	}

	private static SquidSpawnEvent ParseSquidSpawnEvent(BinaryReader br, SquidType squidType, int entityId)
	{
		int a = br.ReadInt32();
		Vector3 position = br.ReadVector3();
		Vector3 direction = br.ReadVector3();
		float rotationInRadians = br.ReadSingle();

		return new(entityId, squidType, a, position, direction, rotationInRadians);
	}

	private static BoidSpawnEvent ParseBoidSpawnEvent(BinaryReader br, int entityId)
	{
		int spawner = br.ReadInt32();
		byte boidTypeByte = br.ReadByte();
		Int16Vec3 position = br.ReadInt16Vec3();
		Int16Vec3 a = br.ReadInt16Vec3();
		Int16Vec3 b = br.ReadInt16Vec3();
		Int16Vec3 c = br.ReadInt16Vec3();
		Vector3 d = br.ReadVector3();
		float speed = br.ReadSingle();
		BoidType boidType = boidTypeByte.ToBoidType();

		return new(entityId, spawner, boidType, position, a, b, c, d, speed);
	}

	private static PedeSpawnEvent ParsePedeSpawnEvent(BinaryReader br, PedeType pedeType, int entityId)
	{
		int a = br.ReadInt32();
		Vector3 position = br.ReadVector3();
		Vector3 b = br.ReadVector3();
		Matrix3x3 orientation = br.ReadMatrix3x3();

		return new(entityId, pedeType, a, position, b, orientation);
	}

	private static SpiderSpawnEvent ParseSpiderSpawnEvent(BinaryReader br, SpiderType spiderType, int entityId)
	{
		int a = br.ReadInt32();
		Vector3 position = br.ReadVector3();

		return new(entityId, spiderType, a, position);
	}

	private static SpiderEggSpawnEvent ParseSpiderEggSpawnEvent(BinaryReader br, int entityId)
	{
		int spawnerEntityId = br.ReadInt32();
		Vector3 position = br.ReadVector3(); // Not sure
		Vector3 b = br.ReadVector3(); // Target position?

		return new(entityId, spawnerEntityId, position, b);
	}

	private static LeviathanSpawnEvent ParseLeviathanSpawnEvent(BinaryReader br, int entityId)
	{
		int a = br.ReadInt32();
		return new(entityId, a);
	}

	private static ThornSpawnEvent ParseThornSpawnEvent(BinaryReader br, int entityId)
	{
		int a = br.ReadInt32();
		Vector3 position = br.ReadVector3(); // Not sure
		float rotationInRadians = br.ReadSingle(); // Not sure
		return new(entityId, a, position, rotationInRadians);
	}
}
