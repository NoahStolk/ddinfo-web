using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Core.Replay.Events;
using DevilDaggersInfo.Core.Replay.Structs;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Core.Replay;

public static class ReplayEventsParser
{
	public static List<IEvent> ParseEvents(byte[] compressedEvents)
	{
		using MemoryStream ms = new(compressedEvents[2..]);
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

		return events;
	}

	private static IEntityEvent ParseSpawnEvent(BinaryReader br, ref int entityId)
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
		Int16Vec3 position = br.ReadInt16Vec3();
		return new(entityId, position);
	}

	private static HitEvent ParseHitEvent(BinaryReader br)
	{
		return new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
	}

	private static TransmuteEvent ParseTransmuteEvent(BinaryReader br)
	{
		int entityId = br.ReadInt32();
		return new(entityId, br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3(), br.ReadInt16Vec3());
	}

	private static InputsEvent ParseInputsEvent(BinaryReader br)
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

	private static InitialInputsEvent ParseInitialInputsEvent(BinaryReader br)
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

	private static DaggerSpawnEvent ParseDaggerSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		Int16Vec3 position = br.ReadInt16Vec3();
		Int16Mat3x3 orientation = br.ReadInt16Mat3x3();
		_ = br.ReadByte();
		byte type = br.ReadByte();

		return new(entityId, position, orientation, type);
	}

	private static SquidSpawnEvent ParseSquidSpawnEvent(BinaryReader br, byte entityType, int entityId)
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

	private static BoidSpawnEvent ParseBoidSpawnEvent(BinaryReader br, int entityId)
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

	private static PedeSpawnEvent ParsePedeSpawnEvent(BinaryReader br, byte entityType, int entityId)
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

	private static SpiderSpawnEvent ParseSpiderSpawnEvent(BinaryReader br, byte entityType, int entityId)
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

	private static SpiderEggSpawnEvent ParseSpiderEggSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32(); // Spider Egg Type?
		Vector3 position = br.ReadVector3(); // Not sure
		_ = br.ReadVector3();

		return new(entityId, position);
	}

	private static LeviathanSpawnEvent ParseLeviathanSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		return new(entityId);
	}

	private static ThornSpawnEvent ParseThornSpawnEvent(BinaryReader br, int entityId)
	{
		_ = br.ReadInt32();
		Vector3 position = br.ReadVector3(); // Not sure
		float rotationInRadians = br.ReadSingle(); // Not sure
		return new(entityId, position, rotationInRadians);
	}
}
