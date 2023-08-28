using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using System.Diagnostics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct BoidSpawnEvent(int EntityId, int SpawnerEntityId, BoidType BoidType, Int16Vec3 Position, Int16Mat3x3 Orientation, Vector3 Velocity, float Speed) : IEntitySpawnEvent
{
	public EntityType EntityType => BoidType switch
	{
		BoidType.Skull1 => EntityType.Skull1,
		BoidType.Skull2 => EntityType.Skull2,
		BoidType.Skull3 => EntityType.Skull3,
		BoidType.Spiderling => EntityType.Spiderling,
		BoidType.Skull4 => EntityType.Skull4,
		_ => throw new UnreachableException(),
	};

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x06);

		bw.Write(SpawnerEntityId);
		bw.Write((byte)(BoidType switch
		{
			BoidType.Skull1 => 0x01,
			BoidType.Skull2 => 0x02,
			BoidType.Skull3 => 0x03,
			BoidType.Spiderling => 0x04,
			BoidType.Skull4 => 0x05,
			_ => throw new UnreachableException(),
		}));
		bw.Write(Position);
		bw.Write(Orientation);
		bw.Write(Velocity);
		bw.Write(Speed);
	}
}
