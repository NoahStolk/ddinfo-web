namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityOrientationEvent(int EntityId, Int16Mat3x3 Orientation) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x02);
		bw.Write(EntityId);
		bw.Write(Orientation);
	}
}
