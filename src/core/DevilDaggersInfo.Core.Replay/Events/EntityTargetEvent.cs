namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityTargetEvent(int EntityId, Int16Vec3 Position) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x04);
		bw.Write(EntityId);
		bw.Write(Position);
	}
}
