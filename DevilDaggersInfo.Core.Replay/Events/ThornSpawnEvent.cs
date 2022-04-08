namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct ThornSpawnEvent(int EntityId, Vector3 Position, float RotationInRadians) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0d);

		bw.Write(0);
		bw.Write(Position);
		bw.Write(RotationInRadians);
	}
}
