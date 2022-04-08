namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct BoidSpawnEvent(int EntityId, int SpawnerId, BoidType BoidType, Int16Vec3 Position, float Speed) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x06);

		bw.Write(SpawnerId);
		bw.Write((byte)(BoidType switch
		{
			BoidType.Skull1 => 0x01,
			BoidType.Skull2 => 0x02,
			BoidType.Skull3 => 0x03,
			BoidType.Spiderling => 0x04,
			BoidType.Skull4 => 0x05,
			_ => throw new InvalidOperationException($"Invalid {nameof(BoidType)} '{BoidType}'."),
		}));
		bw.Write(Position);
		bw.Write(Int16Vec3.Zero);
		bw.Write(Int16Vec3.Zero);
		bw.Write(Int16Vec3.Zero);
		bw.Write(Vector3.Zero);
		bw.Write(Speed);
	}
}
