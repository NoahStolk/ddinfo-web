namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct DaggerSpawnEvent(int EntityId, Int16Vec3 Position, Int16Mat3x3 Orientation, byte DaggerType) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x01);

		bw.Write(0);
		bw.Write(Position);
		bw.Write(Orientation);
		bw.Write((byte)0);
		bw.Write(DaggerType);
	}
}
